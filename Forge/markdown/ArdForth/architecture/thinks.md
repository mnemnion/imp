#Thinks

Thinks are short-term buffered commands that an Orc gets over the wire. 

Thinks are a response to the Harvard architecture, which has separate mechanisms for addressing RAM and code store. 

We'll need a separate interpreter word, DOTHINK, for handling thinks. Conceptually, it's very simple: It loads a word at a time from memz and executes it, putting itself on the return stack. When it gets back, it does the next word, until it hits EXIT. 

We also want a way to make a think into a chunk. A chunk is a word, in front of the pak but not linked to it. 

Since the compiler is unable to define single-char words, we can use flags before the name to distinguish a definition from a chunk. The interpreter state will think-compile chunks, but the compiler doesn't know them at all. This gives us a compact way to redefine the chunk as a word if we like it, as described under chunks.

Without thinks, all a poor Orc can do is interpret, a word at a time. This wouldn't let us test anything like a realtime loop. Burning a chunk, while temporary, gives us the same code speed as an ordinary word. Thinks cost an augenblick more for single word commands, and pay off after the second: it's best to just think every command, since a newline triggers a think. 