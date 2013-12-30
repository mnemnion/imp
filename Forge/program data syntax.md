#Forth and 'Language'

A language is really three things. A program, data structures, and a syntax for manipulating that program and those data structures. 

Forth has a syntax that rubs most people the wrong way. That's because Forth is a programmable program, not a 'language' in the usual sense. In Forth, we call a programmer a 'user', which I like. 

Forth does have a syntax, and a data structure. They grow directly out of the program itself, which is a different approach to the 'subroutining' problem than frame-and-register. Note: in classical Forth on Princeton architectures, there is exactly one data structure provided. It is moderately sophisticated, given the 'one size fits all' character and the extreme simplicity of the machine instructions needed. 

Forth itself is a fine program for interactively developing data structures and words that manipulate them. My thesis is that there is none better, in fact, for this particular task. The reason Forth is a ghetto is that Forth users become accustomed to the syntax of Forth, and don't see the need to design Forth software for manipulating their data structures using a different syntax from that dictated by the Forth program.

Looking up from the bottom, it's easy to see the disadvantages of imposing an unnatural syntax on the computer. From above, and most people are not looking to solve the kinds of problems solved by the Forth program, using the Forth language seems equally unnatural. The  its absence of type, stack discipline, and concatenative syntax, all seem masochistic. 

It bears repeating: Forth is a program, not a language. It has implementations, not dialects: ANS is an interface, a dialect only in the sense that certain words are *initialized* to have certain behaviors, with some effort made to provide consistent behavior across architectures. Not much, but some. 

In less than fifty words of ANS Forth, the skilled practitioner can transform all aspects of the system beyond recognition. It's not a programming language, it's a programmable program. Emacs is a programming language disguised as a program, with Forth, it's the other way around. 

##A Language-Oriented Problem.

One of the tasks I'm setting out to solve with Forth is building a comfortable toolkit for implementing syntax over Forth. Why? Because I like syntax, even though as I gain wisdom I see the sense in abandoning it in the search for knowledge. 

OrcOS is by definition deep in the bowels of machine instruction and so on. Whatever I write or adopt for ARM will be much more roomy and making it polyglot and capable of sophisticated data structure is important. That's why we'll have Fabri as an overlay to ANS Forth, providing type annotation and other useful metadata so that compilers may do their thing. 

I'd enjoy implementing Lua on such a machine. I'd enjoy implementing some of Clojure on such a machine. Under one condition: I'd need to design some sophisticated combinator parsers so as to be able to turn a grammar into a parse dictionary. 

Lua would simply be excellent. The Forth we run on ARM has to be able to stay out of the way of Clang's registers, and vice versa, so that we can make native crosscalls. Still, I want a Lua engine in pure Forth, because I don't want C crowding out the wisdom of Forth. It's always a compromise to add C to your system, and it should be viewed as an ornate meta-assembler with a large, suspect, legacy codebase. 

We could carry over a decently large, dynamic, garbage collected codebase. Also, from the Fabri end, we could approach a new Lua less constrained by certain decisions: 0 indexed, with a proper numeric tower, and perhaps a slightly tighter syntax. 

Here's a new Law: Every operating system grows until it can load Gmail, or dies trying. I don't want to have to do this: I want the tools to be so excellent that someone does it singlehandedly as part of their master's thesis. 

I am interested in Lua, because syntax is not-so-hard when you have the data structures and the recognizer word set. I really like associative dictionaries. Linked lists are nice and all but well, so are maps. I think implementing a few of the core Clojure functional structures would be fun. 

A scripting language is absolutely needed and modesty suggests not tricking it out right away. Language skills should be transferable: we want Portuguese, even if jacking up Interlingua into a proper modern Latin is the purer path. 

I bet we can get it running Hoon pretty fast 😈.

