#The Jones Bootstrap

There are a number of wonderful guides to implementing Forth. Too many, in fact.

I'm going to follow the JonesForth philosophy, in as far as I can, given that we have a Harvard architecture with one (not two) built-in stacks. I'll read out of pforth, flashforth, and anything else I need to to stay oriented, but our code will be MIT, not GPL. I take plagiarism seriously, but see no need to black-box foreign code. Given my usual rambling state of mind and copious note taking, OrcForth will be, clearly, my own creation. 

Jones is using 3 flag bytes. Can we afford that? We have two, while preserving ordinary ASCII. Stay tuned.

Jones is also using a dramatically different architecture. Our cell is two bytes wide, we have more registers but only one built in stack, our code, SRAM and EEPROM are different, etc.

To make things really entertaining, I've never done ASM before. Don't know either dialect. Welcome to today's tutorial on real computers. 

First thing we need is a `.next` macro. This is going to look really different in AVR, I think, because AVR does fetch-and-execute in one cycle. Somehow. My head hurts already.

`.next` is used for direct threaded code, the kind we use for primitives. 1k Forth will be as primitive as practical, but we will need indirect threading also. At least at first.

Really, I'm stuck diverging from the Jonesforth approach almost immediately, because intel provides two stacks, and AVR only one. Time for some straight deep diving.

##AVR Forth to the Rescue

I'm strongly leaning towards just understanding AVR Forth. I need to grok assembly, Forth, and AVR all at once, and the FlashForth codebase is completely opaque. GPL is just nonoptional here. It wouldn't be so weird if Orcish had the curse, now would it.

We can reimplement it when Fabri has a decent assembler or even cross assembler. That'd be cool, right? I'm told good things about the Forth approach to assembly. Such as, it strongly resemble writing Forth. We like that.

AmForth is not complicated stuff. You load the inner interpreter, you put in some dictionary, you add more dictionary. Apparently there are limitations in the AVR that require that Flash loading words reside in a certain region. 

This reduces the problem space considerably: figure out the really core-core AmForth words, then figure out a way to pack them in. 