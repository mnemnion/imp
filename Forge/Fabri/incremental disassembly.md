#Incremental Disassembly

A major justification for Fabri is incremental disassembly: janking apart foreign code and annotating it up into something sensible.

With Forth, we can 'word up' the assembler instructions as we start to understand them. When we get to the point where we can say "this word should be called sqrt, and it should take a double and return a double", we can annotate that. Now, assembler-level annotation will get arbitrarily cryptic and ugly: this might feel more like an argument with Fabri than like a conversation. But when you get an assertion right, it'll pay off: you now have more knowledge in the code than it had before, which is the essence of disassembly. 

This is general purpose gear, but it's there for the favorite sport of the Krewe: in Common we call this "Dungeoneering", in Orcish, Cavin'. This is the process of pwning a micro and making it into an Orc that does the same thing, wink wink, nudge nudge, oi oi. You bought it, you break it. 