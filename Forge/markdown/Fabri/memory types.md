#Memory Types

Fabri will need to be able to type at the assembly level, and keep track of what kind of data is in what kind of memory, right down to the register level. 

I don't even know where to begin. This is why I'm writing my own AVR assembler. One reason, anyway: another is that I want verbose instruction words, since I favor reading over writing and ASM lines are shallow anyway. 

These assertions will be the first that we'll turn off, since they're a spiral of recursive descent hell into extremely slow execution. Checking each register when you load it? How do you do that without some registers? We'll use the scratch, obviously, and just pretend that the scratch is always of type `assert`, which it will be at that point. 
