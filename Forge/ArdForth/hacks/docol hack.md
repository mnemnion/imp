#DOCOL Hack

Here's an extra level of indirection that might save us all important code space, where it counts.

DOCOL has its own address loaded, or it couldn't do its own loop. When it jumps to a new code string, it can check to see if it's got another DOCOL, if not, it executes the word instead of using it in a JMP. So if it's machine code, we're off and running. If it's not, we have a manual jump to the rest of the word. 

Most words are DOCOL or direct, in fact in Orcish most words are direct since we cheat on constants etc. to save space. Careful profiling and instruction counting will reveal if this kind of tradeoff is worth it. It's probably only one tick slower on direct words, and two ticks on indirect. Which is a lot of ticks, admittedly, for the core loop, which in AmForth is 16 ticks. 