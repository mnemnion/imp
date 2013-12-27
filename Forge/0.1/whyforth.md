#Why Forth

If unfamiliar, Forth may seem like an odd choice for what I'm proposing. Even the familiar may be skeptical.

Sitting on my desk, in the obligatory Altoid-sized container, is a BeagleBone. A simple beast as full-fledged computers go,
it comes pre-packed with Linux, as you might expect. Plug it in, you have a warm Altoids tin that can run Ruby on Rails. Urbit On
Unix is no means a stretch; I simply haven't gotten around to it, being more interested at the moment in making my Arduino go blinky.

What's cool about this gizmo is that it's got a burly PWM, sensor ports up the wazoo, and amenities like USB built right in. It also
has a well-supported OS-free mode. 

You cats clearly have Urbit over Unix on lockdown, and there's no other sensible way to be doing it right now. Urbit over Xen is a 
natural extension of the existing codebase.

I've talked about Urbit over Lua, which I think remains a good idea. Dynamic, garbage-collected Urbit can go onto browsers and existing
phones, which is groovy when the client-server distinction starts to come into play. Fundmentally that's a shim, however, and not an 
exceptionally hard or interesting one to write. 

I'm looking right at the floor of the problem: how to efficiently run Urbit semantics on a huge variety of hardware, much of it limited,
some of it opaque. I've decided that a Forth environment is basically the right approach. But why?

## An Elegant Weapon

Most important thing to know: Forth is not a programming language. Forth is a program. That program is built on a set of conventions for 
operating a digital processor: really, Forth is those conventions, not so much any given program. 

Perhaps the distinction is unclear? Where Lisp has parentheses, Forth has the number 32, in decimal. In addition, Forth helpfully gives
you a few calculational machines: a data stack, a return stack, a memory stack, and some words to manipulate numbers on those stacks.

Forth has two behaviors for numbers between 32s: if they can be parsed as an ASCII number, their value is pushed onto the stack, 
otherwise, the number is looked up in the dictionary, its address is pushed onto the stack, and Forth executes the code at that address.

This is all that there is, folks. `type` in Forth means to print a string to the terminal. For serious. Everything else is a set of
conventions for handling numbers. Floating point is in many distributions a library you must explicitly import, though ANS makes it core.

ANS Forth is a language, more or less. Lots of Forth heads don't think it was the right direction. We're talking less than 400 words here;
Common Lisp it is not. 

If I love Forth so much, why don't I just marry it? Well, trees compose, stacks don't. On the other hand, I've yet to meet a processor 
that knows what a tree is. Even the register machines grok a stack; the stack machines grok nothing else. 

So Nock, Hoon, Urbit, offer precise high-level control. They bring order to the top of the stack. Ax, and Forge, are complements of that:
we have the luxury do design our own operating *system* to run our operating *function* in a way that is similarly elegant.

Elegance matters; that's Apple's thesis and it's my thesis too. I'm a math guy, it comes with the territory. 

## An Interactive Assembler

What does Forth offer that C does not? Libraries are written in C, for the most part: is there anything we gain?

Well. For one thing, there are a metric grip of Forth implementations, including ones that are designed to work with C object code in a 
Forth-familiar way. That is to say, you push your arguments on the stack, push the address of the function, and call a word that executes 
it all within a data context and returns the value to the stack. This is just a way of saying "the bad is not so bad". It's a step down
from working native.

A situation snuck up on us, and we're coming to grips with it slowly. We now have a situation where you can buy decent little 8kB 
microcontrollers retail. They're the size of a dime and cost the going rate of Thai noodles, and they can do a lot. 

It is enlightening to realize that motherboards are populated with six to a round dozen of these tiny computers, each one running its own
OS, almost always with reprogrammable firmware. This is of course the popular route for pwnage by real adversaries; the user kernel, it 
does **nothing**. 

What is the most important computer in your computer? Clearly, it's the one(s) that run the ports and radios. The CPU is dumb and blind,
only those little tiny jobbies know what's up.

So if we're going to pwn them in turn, and we must, it would be niced to be armed with an intelligent interactive assembler. The 
guess, compile, pray cycle is a recipe for karoshi under adversarial circumstances, such as taking on an LTE stack. Bad enough frying 
all those chips.

The advantage of Forth here cannot be overstated. Forth since its inception has been designed to run on really really stupid hardware. 
Better yet, it's designed to run on really stupid hardware, while controlling hardware so dumb that it's questionable to call it a 
computer at all. Running Forth on a machine just to run Forth on another machine isn't just routine, it's right in the middle of the
use case curve. 

So over on our laptop, we can be running a really sophisticated Forth, with type annotations, inference, memoization and rewinds, and
an intimate knowledge of the Ax/Nock/Hoon/Urbit system above it. That way, all our little micros have to do is barf their Orcish brains
into our Forge environment anytime they fuck up, and we'll do the haruspicy up where we have breathing room. 

The tendency has always been to leave the primitive tools primitive in the rush to provide polished high-level user environments. I 
resist that: I feel that the only defense against complexity is to have very good tools for literal inspection and evaluation of every
level of abstraction the system is using. I'm with Loper on that one, but I don't coonsider Pure Land Lisp the right answer for 
all questions.

Forth is currently dominated by elderly-if-formidable engineers who prefer to track their conventions with pencil and paper. Writing 
a new generation of tools is necessary to bring a new generation of coders over to the true Jedi ways. What do you think boots that
lightsaber? Fuck yeah it's a stack machine. 

So Forge might eventually be an "editor", by virtue of being a live, interactive environment for complete control of a remote computer via a Unix-hosted system. Ultimately, Urbit-hosted. But the tools I'm building on the way are mostly about visualization and only
secondarily about manipulation. It seems to be the right order. 