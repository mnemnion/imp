#Fabri: Philosophy

We have inherited a rich collection of gizmos, and continually invent more. 

We then proceed to impose order, from the wrong direction. 

Fabri, and Forge, are a bottom-up approach to this problem. 

##Stack Discipline

We are beginning with pure hardware and some sketchy idea of what it does. Typically it will come preloaded with object code, which we may change with difficulty if at all. 

The first step is to impose stack discipline. Any real processor may be made to function in this fashion; the smaller the chip, the more likely that it's optimized for this pattern. If you stop here, you have Forth. 

In a sense, we stop here. There's little room on our carl chips, and we can afford little distortion of their existing function. 

##Exoskeletal Typing

Ever looked at the [Low Level Virtual Machine](http://llvm.org/)? I spent almost two hours briefly contemplating writing a Forth that targets this monstrosity. Then I figured out why there doesn't seem to be one. 

Forth, and hence Fabri, is a High Level Actual Machine: In general, the real computer does what you'd expect it to, given the virtual model. The idea is to push this philosophy to its natural conclusion. 

The Fabri type system is annotative, and optionally runs aside the actual code. This aids both the compiler and the pilot. 

The philosophy is this: a computer can only run the object code that it understands. Fabri runs close to the metal, and Forge provides a way to inspect and relate code all the way to the instruction level. 

Any type discipline is exoskeletal: it supports the execution of the exact word set invoked by the pilot. The compiler, if it's being used, may profitably make use of the information provided. 

A simple example: we have a word, `sha-1-128`, that takes two numbers off the stack: TOS is the size of the block and POS is the address. It returns the address of the hash, which is 128 bits long. We might simply annotate that: `\ (adr offset) -- := hash` meaning "we take an address and an offset, and return, by definition, a hash." 

Fabri can then do the simple, ordinary things with this information: subtract one from its stack promise when inferring the stack effect of other words, check that the stack indeed contains an address and an offset, and infer that the address deposited by the word is a hash.

Fabri does this off in its own stack environment, without troubling the running code. We need this for three phases: when we're building code, when we're optimizing it, and when something isn't working correctly. The rest of the time, we enjoy the speed of ordinary calculation. 

It's helpful to compare this to the overwhelming preference for designing abstractions and then implementing clever tricks to cook those abstractions down into running code. This of course requires one to be very clever indeed with the underlying hardware. We are not an army of engineers: we are scruffier than that. 

Meanwhile, Forge can do helpful things: highlight the word according to its effect, display and relate the Fabri code to its naive threaded interpreted assembly, as well as to any optimized version the compiler may produce. Addresses with names or types can retain that information visually: those without may be assigned names of convenience.

##Stupid Code is Smart

Which is not to say that smart code is stupid. Though have you ever second guessed a compiler? Were you correct? How long did it take to figure that out?

The most convincing case for 'stupid is smart' comes from LuaJIT. Lua is a moronically simple language that's never met a compound type that isn't a table. As a result, anything you do, you do in the same dumb way, over and over again. This is the charm: Lua is small, and plays well with Real C Code written by grownups. 

LuaJIT turns this relationship on its head. Mike Pall, who is your basic genius, has written a compact tracing optimizer, that figures out what all your loops do, and produces the same result through Special Magic. 

My premise is we might also do this, quite neatly, and that all this type and stack annotation and inference business just might help with that. 

Optimizing compilers are not new in the Forth world. There is virtue to the 'threaded interpreted' code, which may be followed more exactly both in reading and in stepping around. Fabri, to be useful, will be keeping around many definitions of a word, in multiple architectures, and using whichever actual code is appropriate to the situation. 

We could even do crazy stuff like polymorphic words that have different effects depending on the state of the stacks, so that 'count' could do sensible things with different types. That would almost not be Forth, but it might be cool. 

##First Class Dictionaries and Parsers

The master Fabri environment will have a library, which is composed of books, from which any given program will have a dictionary assembled. Something like that; we simply have to have a concept of modules and namespacing or we're screwed. If one thing knocked Forth out of pole position, it was this. 

Parsers are an interest of mine. One reason I'm rolling with Forth: writing a Scheme in Forth would be a pleasant exercise. Writing Forth in Scheme, pointless and frustrating. I have no interest in compelling everyone to write in Fabri: it's a problem oriented language, oriented around a problem many people have. Meanwhile, other people have other problems, like an inability to express themselves without algol, or s-expressions. Very well; I don't see the harm, really. 

A parser is pretty easy stuff: you start a word, and that word turns out to be a parser word, and everything else the input reads is read by that parser until it gives up, and hands control back to the Fabri parser. Which doesn't actually have to happen, if all goes well and that's what you want. `."` is a parser word, and `"` ends it; this relationship may be arbitrarily complex. 

Other than typing this as a 'parser', there may not be much to it. We need some kind of annotative language to denote parse switches. Good thing we can interactively develop the parsers! 

###Literate Fabri

One of the early and important parsers will handle documentation and commentary. We'll use Markdown, with as few modifications as possible, and Forge will assemble the whole into a coherent memory structure. 

This is crucial, because an editor which is not language-aware and self-documenting is not worth learning to use. Rather, we have those in abundance, and our pilot is assumed to be proficient in at least one. 

We also need this because code archaeology is a major purpose of the Forge environment. We will wring hard-won understanding out of opaque and sometimes obfuscated object code: we'll need a way to sensibly mark it up and share our findings. 

This is distinct from the annotation parser: we will have comments and annotations, and EDN for arbitrary metadata, because JSON is broken in too many ways. 

C is a language. Lisp is a thesis. Forth is a program. It is the only high-level metassembler that actually makes sense given either Princeton or Harvard architecture. We could even learn to emit LLVM, if there were some reason to do so. We consider compilation itself an optimization strategy. 

The important thing about Lisp is not the syntax, which is painful to write with an ordinary editor. It is the environment. Even cobbled together from Emacs, paredit, slime and swank, Lisp is simply joyful to program in. Forth provides some modest measure of that same joy; the experience can be dramatically improved. 

Forge might not aspire to be your operating system, exactly. It does want to read your email. It definitely wants you to know exactly what's going on in any client computer you're programming, and as much as possible of what's going on in the computer you're using to program it. Certainly, it aims to be transparent at the program level.

###Macros

You want what? Sorry, I can't hear you over the sound of my parsers crunching through text. 

What I mean to say is, there's almost certainly advantage to writing a very nice Lisp that is in essence a parser and a dictionary of words to go with it. S-expressions are almost as elegant as pure concatenation, and may certainly be implemented with reasonable efficiency using a stack machine.

 feedback loop that we don't have a solid handle on. That starts with a language that understands small hardware, and knows that it's living in enormous hardware, dedicated to nothing but understanding and transforming code into software. 

