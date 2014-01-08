#Fabri on FreeForth

This has potential. I'm averse to using a dialect, but it's free, and it's context free. I like that. It's also written in ASM, which helps with staying away from poisonous code. 

Fabri starts as annotations; eventually I'll write some words to do things with those annotations. Originally Fabri will be a separate text parser, that uses the ordinary Forth stacks and memory structures. Adding it as part of the main loop requires going into the Forth program almost by definition. 