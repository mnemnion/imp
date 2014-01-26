# Hammerspace

Forth does not presume garbage. Let's not create any. We can work with different concepts instead. 

Hammerspace is a hack to allocate big chunks of the dictionary then round-robin them into and out of memory. Forge is going to need immutable data structures and transcients, period. 

We'll be building this up in stages, because we're using Forth, and because it all has to work.

## Rolling Allocators

Our simplest building block is a rolling allocator. You ask it for some memory, it gives you some. It tacks onto the end if it can, if not, it rolls over. If the request exceeds capacity, it fails, returning no memory address at all. 

Your access words must fulfill the contract not to write past the allocation. Fabri will support a concept of typed memory: any word that accesses a particular type of allocator will return a typed memory address constrained by size, and arithmetic that exceeds this address will be remarked upon as unsafe. 

These are pads: it's intended that one request, write, and then read only from that point. 

## Generational Rolling Allocators.

Here, you request memory, and receive it: the difference is that the generation (how many rollovers) is tracked. Or, you may query the allocator as to generation. This allows a handling word to discover whether or not their data still exists: the rollover generation and offset together will tell you if your data has been overwritten. 

This makes the rollover behavior complex; we make it a simple matter of calling down the rollover execution token and performing it. This means arbitrary behavior, in particular saving the entire allocation to disk, may take place before the rollover. 

We can therefore define some variants on these allocators. We have a general pattern: when we have a need to generate data, and not a moment before, we request an allocation. We may write to it with confidence, and read from it for some unspecified period of time afterwards. If the read isn't immediate, we should check if we still have it. 

## The Hammerspace Queue(s)

We could build this as a single queue; it's easier to describe, and the size-specific optimization is fairly trivial. 

Hammerspace is a single, immutable table, containing all of hammerspace. Hammerspace may grow, but it may not shrink. Each value in hammerspace consists of a box, which is an allocator of a convenient size. We should have perhaps three sizes of box, two might be enough.They can nest in the limited sense that a the area of memory taken by a large box will hold 32 small boxes.

Provisionally, we have 4 KiB boxes, 128 KiB boxes, and 4 MiB boxes. Each fits 32 of the former. Call them parcel, package, and container. 

Each box has a header, which tracks the allocations within the box. A parcel merely records the generation, offset for the next allocation, and the execution token for rollover. This fits in 2 cells, leaving 510 for user data. 

A package or container has a detailed header allowing for more detailed handling of the inner contents. 

The way it all works is this: You request memory, and indicate whether you intend to write transcient or permanent data. The Hammerspace queue returns an allocation from the first box that can hold the memory, and puts that box on the bottom of the Hammerspace queue, presuming transcience. Eventually, your data will be gone; presumably, you retain a durable reference to the source data, and the operation which produced the transcient.  

Permanent data may only be requested by responsible parties, and is put in a lockbox, taken off the Hammerspace queue and put onto the archival queue. When the process writes to the lockbox, the archival queue periodically finds changes and saves them to disk in an appropriate place. The contract is that permanent data may always be retrieved by presenting the request token and the generation that was returned with the request. That's real engineering, that.

###How to use

Naively, I'm going to use this as a huge hack. There will be plenty of hammerspace, and then I will run out. The goal is that the system will tell me I've run out, and I can manually allocate more hammerspace, and that will someday get annoying, and I'll figure out how to save stuff. 

Eventually, we build functional data structures that work. Any transient should be counted on to unpredictably disappear, while any permanent change will be saved to disk as soon as possible and forgotten out of memory only reluctantly.

Somewhere intermediate to that, I'll introduce lockboxes, which may be requested by handlers from Hammerspace. These get used and then returned, while Hammerspace memorizes who borrows what. The idea is that a handler might leak a whole box, but then the handler itself is gone, and Hammerspace can eventually figure out that this has happened, and inform the user. Who might patch the handler if she's good at that. 

Subjects attached to frames get their own boxes, file subject get their own boxes, transients and pads are allocated out of Hammerspace. 

We can optimize the Hammerspace queue in simple ways that postpone erasures: having a separate queue for half-full boxes that takes only small allocations can go a long way towards delaying transient losses. Furthermore, if we have say two containers, we don't want two sub-half allocations to use up our only remaining container-sized piece of memory. 

Simplest possible is to fill up a box at a time, by size, off the top of the queue, then roll it to the bottom. That means more box misses, and some reasonable definition of 'full', so we don't get stuck with a pathological box waiting around for a couple cells of data. 3 / 4 allocation seems like a sensible heuristic.

Really, this is a quick path towards dynamic-ish memory. Systems are easier to write if you can know that a) your data will go somewhere sensible b) this will not punch a hole in anything and c) you can intelligently find out if your data has been overwritten. 

By maintaining a master allocation table, I can write words that will try to figure out where my Hammerspace went and what it's doing. I like the idea of only ( deliberately ) leaking into memory that I control. 

Hammerspace gets really fucked up if you go writing to a transient area you've lost control of. It's important to check the generation and to stay within allocated bounds. I'm bad at this, so I'll make safe, slow access words and replace them with fast things if needed. We can ask for a whole transient container, one bounds check before a huge move is no big thing. Reading is always safe, it's `!` we have to watch out for. In general, the first write after the allocation is perfectly safe to make, and is based on a dup of the request number, meaning it won't overwrite the request by definition. 

We only like mutate-in-place where that's the efficient algorithm. Usually, we write once, read one or more times, and call it good. What we write is either of durable interest or it is not. 
 
##Naive Persistence

The simplest possible way to persist is to simply put the full box at the bottom of the queue, after it triggers a 'flushme' event into the flush queue. This compresses it to disk and wipes it clean. 

Compress? Makes sense; either we save a little space, no space, or a lot of space. We save a lot of space if we're making lots of largely identical copies of data. We could keep track of where the changes are and what they are, or we could use compression. Compression is pretty close to optimal at sussing out redundant data and encoding it accordingly.  

That way, we may forget references to data but we never forget the data itself. We have only two queues: valuable and not valuable. The generations are always tracked so the boxes are interchangeable as to purpose. 