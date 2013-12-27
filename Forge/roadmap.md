# Forge Roadmap

This is an iterative bootstrap.

Both C++ and Objective C were born as preprocessors to the C language. Forth is of course much more flexible than that.

Fabri will start as ANS Forth, and may never shake that inclination entirely. I do feel like we need to do a better job with dictionaries; I'm spoiled by the ubiquity of key-value relationships, and don't care to muddle them all into one data structure. That's what compilation is for; it even sounds right, you compile a dictionary from a library of codices. 

In the meantime, we use Forth the way it is. Parser words are ordinary words, and we have a dictionary. I am unafraid of Ünicøde, and neither is pforth or gforth. 

The first step is to keep working on visualization tools. What I can see, I can reason about. 

Second step is to brush up on parsing words and define a simple annotation tool that does sensible in-memory things with code. With this, we build an inspector, and stepper. I mislike the word "debugger", which implies that we only step around when things are broken. This discourages the use of a tool which should be second nature. 

This is the core of the Forge environment: a live editor and inspector of running code. Compilition and optimization deserve to be interactive dialogues, not mysterious and frustrating steps performed by opaque wizardry. Profiling, unit testing, inference, inspection and alteration, all the bells and whistles. Ultimately we'd like to see the entire chip state: this should be possible for client systems and may even be possible if we run some sensible operating system over ARM or Intel. A fellow may dream. 

## Stack Inspection

Almost code writing time! 

What I want is a smart stack printer. To begin with it can't be all that smart, because all it will know is whether a number is in the dictionary. We rapidly need to make it smarter, because I'd like a counted string to look like two-stack, quoted version of the text. At least the beginning of it. 

This *begs* for a second stack. I believe there is a way to run multiple Forth environments in ANS Forths which support it, and GForth at least does. I probably need to redefine one of the core words in the Forth loop, or more likely embed a different loop inside a paired set of control words. 

Forth is growing on me, slowly. The way here is to solve one problem at a time, and make a word out of it. 

I'm realizing the most important file is an annotated set of the fundamental ANS words. That's grunt work and poetry all in one. There's a standard format, and I'll keep to it where it makes sense. 

Forge in Forth will need to read Forth code and do useful things with it, like display the source on command. At some point our annotation and type system will be good enough that we'll want to influence the generated code, at which point we'll deep dive into pforth and make something good out of it. Hopefully we'll have a merry band by then: realistically this is about a year away. 