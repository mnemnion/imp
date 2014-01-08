#A Problem Oriented Environment

So now we have Ax. I'm neither done talking about it nor writing code surrounding it. 

It was certainly a clarifying exercise. [Stanislav Datskiovsky](http://www.loper-os.org/?p=1390) recently sold his Dukedom in Urbit, provoking a certain amount of discussion. My [reply](http://www.loper-os.org/?p=1390&cpage=1#comment-8502) hints at a certain direction, which I intend to amplify here. 

As foundational texts go, Chuck Moore's [Problem Oriented Language](http://www.colorforth.com/POL.htm) is a gem. It predates Forth, in the sense that [the McCarthy Paper](http://www-formal.stanford.edu/jmc/recursive.html) predates Lisp; it is rationale for that which follows. 

Forth solves Chuck's problem, admirably and well. The ColorForth array, with its 32 opcode words, is breathtaking in its simplicity and elegance. This was not Chuck's original problem: he had a more prosaic issue, which he describes like so:

	Most applications can be programmed very nicely on a small computer: say 4K of 16-bit words with a typical
	instruction set, floating-point hardware if needed. If, that is, the computer is augmented with random access
	secondary memory, which I will call disk. The capacity of disk is unimportant, even a small disk providing
	plenty for our purposes, and is determined by the application. However, it is important to be able to copy
	the disk onto another disk, or tape, for back-up. Thus I envisage a small computer with 2 secondary memories,
	and of course a keyboard or card-reader and printer or scope for input and output. 

Existing Forth systems are faithful to this recipe, and do things with memory that may remind readers of a certain age of their Depression-era grandmother. 

We need a problem-oriented language, but we have a different problem: One user, hundreds of discrete computers written in a dozen dialects of three or four major architectures, thousands of sensor-class devices. There are constraints: our console is an entire computer with an operating system baked in, and we do not presently have the luxury of changing that. Fortunately, we are no longer impoverished, and our laptop can and will function as console and virtual machine host. The late stages of colonization lay siege to the desktop: there are fatter and more tempting targets. Unix is Embassytown already, primed to cut us a piece of real estate; we should oblige this genial tendency. They haz graphics! It's actually a hard problem. 

Then there are computers like the [BeagleBone](http://beagleboard.org/products/beaglebone%20black). With tight control of the hardware, you could run a Gundam off that thing. It ships with Linux, and a cloud IDE that you client/server from your laptop. That's a problem worth orienting a language around. 

##Inspection is Easier than Introspection

Loper is aiming to build that ancient and mighty tool, an introspecting computer. Good: professionals deserve professional tools. This doesn't solve my problem, which is more like Chuck's: do reasonable things with existing hardware, despite it being, y'know, weird. On purpose, and with malice aforethought. 

The place to start with any given system is to take over the CPU and USB and impose stack discipline. This is effectively a Forth situation; our differing constraints may lead us in different directions. 

Forth is an excellent vantage point for both introspection and inspection. It simply hasn't been used for that purpose. That wasn't the problem. Ultimately our language will not be Forth, but it will be Forthright, because it's solving a different problem with the same recipe. 

##Type Annotation

Forth's great strength is that it doesn't lie to you. Types aren't real: they're a form of discipline, and a useful one. Forth's only discipline is stack discipline, and it's enforced through crashing. It happens we have more than 4k of 16 bit words at our disposal, and we can do more, while ultimately doing nothing at all. 

What we need is a problem-oriented environment. The first step in dealing with a problem is recognizing that you have a problem. Our problem, again, is a plethora of powerful hardware shackled to a lame static paradigm, with buckets of code we'd rather use than rewrite. 

Types are a vexing topic. There are only two kinds of types which make sense to me: A number is a type because I say so, and a number is a type because it passes a validating function. Hoon offers a sort of type where you start with a number, feed it to a function, and another number is returned. This is too much type for me, because that sounds like a function, what with the transforming business. 

Forth is serenely pure about type. Instead, it has annotative comments, with a rich tradition. By formalizing those comments into an extensible language for typing, we might have something powerful indeed. 

The BeagleBone is a formidable target for this kind of inspective cross-compilation. Let us climb molehills before Matterhorns. Arduino-class processors are pipsqueaks. 

##Fabri and Forge

Fabri is the dialect, Forge is the program. Fabri is a Forth, in the Chuck Moore sense. I'm using Gforth right now, because the term handling is better than pforth. Public domain is simply better, and I'll migrate before any gut working gets accomplished. 

Fabri has additional architecture that is used interactively in the master loop. An example of this is the annotation queue, which keeps track of each any every stack operation in a rolling queue that provides a decent history. Clearly this is a very slow thing to do, so we only do it when we're 'debugging'.

I add the scare quotes because what we're actually doing is paying close attention. I tend to write code with a lot of scaffolding and print structure, then kick it away. Forge will semi-formalize that kind of process. 

###Start With the Basics

I mean the *really* basics. An editor which is not self-aware will be useless for building higher levels tools. Fabri will be annotated as to type, stack effect, i/o, and all other relevancies: Forge will offer the Fabri code, C equivalents, and assembly code for inspection and alteration, though by necessity trifling with the C code will crash the world. We'll do what we can to maintain state, eventually. 

It is quite trivial, if you know the stack effect of every single word in the dictionary, to calculate the stack effect of a word defined using those words. One might call this forward type inference if one was feeling fancy. It's a good idea, but you **must** have a comprehensively annotated dictionary. 

On this foundation we build a quite ordinary stepping debugger. If it is fancy, it is because we are quite generous in providing the pilot with information. For an example: were I to type `variable foo 42 foo ! foo` into gforth, then `.s` to see the stack, I would see something like `<1> 4309231616  ok`. Ok? Could we say: `<1> 'foo ok`, or even use color to indicate a variable? Indeed we could.  

###First Class Dictionaries

Forth's approach to dictionaries is cavalier. Fabri will have a reasonable set of words for handling dictionaries and using them in a modular fashion. We'll need this, because Fabri and Forge are tools for bringing interactivity to small and obscure systems. We'll start with easy targets that we already understand, then turn to mysteries. Such as WiFi chips. 

Namespacing is crucial here. Consider the editor. Clearly, the best word-name for a command that is normally mapped to k is 'k'. This demands modular names. 

Parsers will also find themselves somewhat promoted, because I like parsers, and because concatenation is not always the right flow for coding. There's no particular reason switching parsers or dictionaries shouldn't be as simple as switching base. A modern computer is really quite roomy. 

We need at least a parser for comments. This is simply redefining the word `\`.

###C and the Dreaded Foreign Function Interface

We dont precisely need or want a C foreign function interface. We we need is a way to rip object code to pieces, annotate it, and use it within our own nefarious Scheme. Compiling C is heinous. Relating C code to the generated object code, less so. There is hope, especially if we have header files. 

Fabri itself will be written in C. You expected SBCL? I am not quite that metacircular. We have C machines, we have a decent public domain Forth, we have term handling libraries, we will use them. There will be calling conventions for C. The Fabri type system will understand them. 

The small computers we're gunning for come stuffed with opaque object code. If it isn't actually C, it may as well be, and some of it is even Forth. We're looking for ways to manipulate those computers, starting with the existing code. This amounts to more annotation, and a relatively transparent way to share it. Git suffices in practice. In Forge, we want to name a register or port and have that translate into any code we inspect, especially object code. Any hint of meaning is useful out on the frontier. 

An Arduino just isn't that big. I should be able to dump the thing, step one operation forward, dump it again, and look at all the guts, push buttons, and so on. Maybe I chain a couple together, or play off a BeagleBone running something real time. Whatever it takes: we need to pull these systems together into a coherent whole. 

The first step is to illuminate the bottom layer. I may hope that writing a basic Fabri and Forge might swing other thoughtful coders over to my cause. 

##Bootstrap 

The first generation of this tool is going to be written in ordinary Forth. I'm still gaining wisdom in this art. Writing out a simple text file that type and stack annotates the core words of Forth is itself a useful exercise: writing an engine that parses it should be fun. A simple smart stack viewer, memory inspectors, step debugging, that sort of thing. I want to test out some new-old ideas about code editing, while I'm at it. 



