#Forge Memory Model

I'm beginning to see that memory management is very simple in Forth. Forth doesn't abstract many things, just stack discipline,
Endianity, and memory: the very arithmetic manipulations that suck badly if not abstracted for you. 

High level assembler indeed. Forth is exploratory like Lisp but in a totally different domain. If you don't know what your data 
looks like, but still want utterly fine-grained control over it, there's no other sensible choice.

Forge, like an old-fashioned Forth, thinks of everything in terms of blocks. A block is the same size as a card; it's a Forth
word. In this doc, we'll use card to mean either the amount of memory a card takes, or a card complete with structure. It'll be
fine!

Cards are really rather small, at 8 to a KB. We allocate cards in packs of 64, which come 64 to a box. So 16KiB and 1MiB. 

4 packs are special, and we call them, of course, a shoe. You play cards, right? We can count shoes down with a single byte, so 
we generally allocate a shoe at a time unless we know we'll need more. The word "allot" is for the croupier, in general;
Labrys gets special privileges, probably amounting to its own Forth environment complete with virtual memory pointer. 

Everything gets fit into these larger boxes, which have colophon cards in the zero offset. Not earl cards; the rest of the pack
is not on the colophon. You know how card packs come with one card that has the copyright, date of printing and that jazz?
Just like that. 

For example, we'll allocate several boxes for buffers, since we have a text editor. I have 8 GiB in memory; I feel comfortable
giving my text editor say 4 MiB just to hold text, esp. since my format allows slightly less than 2 MiB of actual text in that
box. If we're editing, say, a huge log file, we'll use strings not buffers; we're ages from needing that ability.

When something changes, we flip the change bit and deck it down immediately into the scratch. If someone pulls our power switch,
we want our precise environment back. When actually putting Forge away, we make some effort to check what's been changed and put
it somewhere meaningful. 

Clearly we don't do reference counting, garbage collection, or anything headachey like that. If we need to make room in memory,
we clear out boxes and reallot them. 

If you think about it, it makes zero sense to keep track of how much memory you have. I know how much memory I have: it's 8 GiB.
Keeping track of what's in that memory is easy if you use some simple discipline and aren't responsible for interfacing with
ancient behemoth code from back when the question was always "will it fit" not "where do I put it". 

Disks we'll handle similarly. 256 Mib has the 28th bit high; a 'pallet' is a 256 MiB unit and a system-level handler can map references
to moving pallets accordingly. Our address space is in the Exabytes, remember, so we can get pretty far by defining convenient high-low
masks and converting on load and transfer. 

Pallets are about what a 'disk' thinks in terms of, and is about as coarse as memory gets these days. All of Arvo, Forge, and the rest
of it should fit in a few boxes. Add a rich GUI and we're talking a pallet for sensory assets. Systems without onboard entropy go through
pallet-sized buffers of the stuff. 

64 pallets to a container makes 16 GiB, which is comfortably roomy for the data we normally handle. It is quite a lot of cards; you can
fit an "HD" Sopranos in it.

Instead of reference counting, we have reference "accounting". It's better. In general we don't have much need to get fidgety and move
stuff around. 

You can tell I'm much less focused on typing and using data and much more so on collating and stacking it. That's because I'm not into hosted environments basically: I want all the core stuff running in AxHoon on Forth, with drivers and that jazz as a thin layer of C 
bossed around by Open(C|G)L libraries. Writing a GUI for Urbit is not my bailiwick, except to point out that driving it exactly like a 
souped-up ANSI terminal is the only sensible thing to do. Unix fucked this up to everyone's lasting discomfort; let's not be them. 

Also, to the user, the GUI is just a bunch of nouns, and to Forge, it's a deck of cards. All of this is, I hope, obvious. Heck, I plan to leave mouse support up to those with the interest. 

Back to Holler...

