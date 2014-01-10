#Quipu

I'm picturing a data structure, that's basically a rope.

Except each line has exactly 64 cells for word addresses. These can contain real words, or links to fixed-allocation strings.

In a nice, literal Forth, that means you can assemble the strings trivially, push the whole line into a buffer to be edited, then run it through a special interpreter that reconstructs a new version of the line. 

If we use a double-linked list, and why not, we preserve the individual history of each line. Basically make the whole thing immutable.

If you 'define' a word the dictionary doesn't know, it's made into a counted string. Ordinary comments, counted strings. 

This would mean that you'd have only one `dup` per running Forge environment. Ropes actually have decent perf characteristics, especially for large codebases, and this kind of shared structure, which is a Forth kinda thing to do, doesn't slow them down even slightly. 

Technically, if you redefine a word, some ropes will refer to the original definition and others to the new one. We have to watch this tendency: providing proper documentation in a global hyperstatic environment depends on getting this correct. 

If you pretend we're not sharing structure, a rope is easy to picture. This rope is Forth-specialized and gains compactness and intelligence from that fact. 

Thing is, we need the physical address of the word string, not the execution token. `see` does this, so it can be done. 