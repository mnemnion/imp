#Core Tools

Forge needs detailed control of the inner loop. Also, many of the ANS conventions... need a new direction.

The core toolset I'm evolving slowly, while moving as quickly as possible, because there's only so much writing stuff a sane language should have that I'm willing to put in. I want shiny tools. 

core is also known as the BForth Incompatibility Layer, because we're going to redefine some ANS words, mostly relating to string handling at the moment. 

##Rolling Allocator

I intend to use this pattern and variants of it quite heavily.

The simplest rolling allocator takes a request for memory and returns it if possible. Two succesful exits and one error: the allocator either returns a block of memory, or resets and returns a block of memory. It will fail rather than corrupt memory in the event that a word makes an improper request for more memory than the allocator contains.

Allocators handle memory on cell boundaries, because our strings (later) require cell alignment. 

Currently this is considered a user error, more sophisticated allocator systems may rely on this behavior to select a pad with sufficient room for the operation.

We can create interesting variations on this: generational allocators, which keep track of how many times they have reset: lock allocators, which may be set to 'full' by a particular handler and freed only by that handler; this is all static memory, remember, so free just means the allocator can accept transcients again.

Also, functional allocators, which save their allocation block to disk any time they roll over, tracking the generation accordingly. Our operating systems maintain an in-memory disk cache, so this is a fine way to push data around, get it back, and never ever lose it. 

We badly want our type system before we tinker with any of that business.

##The Bag

I want to keep `>r` and `r>` off the stack. Chuck says overuse of the return stack is a classic sign that you haven't factored enough. I say juggling is easier with two hands. We are orienting around different problems; my computers have more than 60 words of RAM. 

The Bag is meant to be a core part of BForth, optimized into registers where possible. I'm convinced we can do interesting context-aware programming and optimization if the return stack contains *only* executable tokens, and manipulation of the return stack therefore consists only of flow control operations.

The bag is also useful for interactive stack juggling, where the return stack and bag stack are separate concerns. 

It is **very bad form** to back pass, using the bag to transfer data between words. Fabri will complain about this as soon as possible.
