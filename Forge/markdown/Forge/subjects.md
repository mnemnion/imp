#Subjects

Forge is not a text editor. It's barely anything right now, but it never will be at text editor per se.

Forge is a subject editor. A subject is simply an allocated amount of mutable data. Subjects are inert, they require handlers to function. At which point they're almost like objects, except structure and function are orthogonal. Were you planning on displaying columnar data, or doing array manipulations on it? Different handlers. 

A buffer is just a named subject: we can compose and use anonymous subjects without naming them. So "split by whitespace and tabulate each unique substring by line location into the buffer attached to the out frame" will create at least one anonymous subject and assign a conventional name to the new subject (now a buffer) attached to the frame. The intermediate subject will be in the history queue, but no other references will be retained.

Subjects are mutable; there's nothing fancy here. We will employ dirt-simple reference counting insofar as humanly possible. We have a lot of memory, and a fair amount of clock, no need to confuse ourselves.

Garbage collection is for when you're generating a lot of garbage. With Fabri, we can add as many of these as we need, and we shouldn't need them in general. Dynamic memory is valuable for an editor that aspires to work with large data, however.

It may be a good convention that allocated memory have a single owner, and if it must be passed, it's passed. So anonymous subjects are owned by the allocation queue; if acquired by a buffer, they inform the allocation queue of this. When the allocation queue overwrites a memory address that it doesn't own, it just forgets it. If it owns it, it `free`s it. The buffer manager handles the rest of dynamic allocation for subjects.

My buffer manager will be stupid. But the stupid will all have one point of ownership. I figure, it's easy enough to write constrained code and then generalize by induction as it were. 