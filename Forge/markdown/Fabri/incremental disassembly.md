#Incremental Disassembly

A major justification for Fabri and Forge is incremental disassembly: janking apart foreign code and annotating it up into something sensible.

With Forth, we can 'word up' the assembly instructions as we start to understand them. When we get to the point where we can say "this word should be called sqrt, and it should take a double and return a double", we can annotate that. Now, assembler-level annotation will get arbitrarily cryptic and ugly: this might feel more like an argument with Fabri than like a conversation. But when you get an assertion right, it'll pay off: you now have more knowledge in the code than it had before, which is the essence of disassembly. 

This is general purpose gear, but it's there for the favorite sport of the Possi: in Common we call this "Dungeoneering", in Orcish, Cavin'. This is the process of pwning a micro and making it into an Orc that does the same thing, wink wink, nudge nudge, oi oi. You bought it, you break it. 

Forge is a visual hardware interface: if it knows what a number is supposed to mean, it will helpfully translate for you. You can always fall through to pure hex at any time. 

This is the main reason I need Fabri, actually, to track locations in memory. 