#Tools

Forge, like any decent software, will be a collection of small sharp tools, that work together.

The primary purpose is visualization and interaction with a running Forth editor. I envision a REPL line from which
all interaction descends, and various frames containing useful information. 

In approximate order:

## Dictionary Aware Stack Reader

A frame that continually updates, showing the contents of the stack, with easy things like substituting words for
their addresses, with color highlighting as to type (constant, variable, execution token and whatnot). 


## Card Reader and Printer

The basic data structure in Forge is the card, described elsewhere. Cards will contain programs, among other things,
so writing a card reader is the next step.

## Semantically Aware Code

This part will take awhile, because fundamentally we're writing a whole type system in Forth, that runs in parallel,
allowing us to do fancy dynamic runtime nonsense when we're developing. This is about visualization; it might prove
useful to keep some of the check words around in a running system, but that is not the point. 

Ideally, we can hook the control dictionary up to a running system with errors and inspect it from a comfortably slow
environment, chock full of useful information. 

## Line Editor

At that point we'll be in a position to write an editor. I'm going to start from an old-fashioned place and see where
it takes me.

I think a line editor could be quite powerful if combined with a frame showing the text and a concept of cursors and
marks within that text. Normally you edit one line at a time, so you type the line number and it shows up in your line.
down arrow gets you the next, up the previous, and so on. Except for the fact that your edits are in a line, and don't
automatically update (though you can set it to), it's a lot like a normal text editor.

When you're not editing a line, you're entering Forth commands into a dictionary and parser optimized for the purpose. 
It has potential. 

## Urbit Awareness

Forge is intended both as a souped-up Forth environment, and as the bottom level of the Urbit stack. As such, I intend
to embed a jetted Ax dictionary as soon as is practical. Ultimately, the intention is to use the tools we have to make
a Hoon-aware editor, which is nonetheless capable of popping the hood and checking in on the Ax and jet levels with 
acceptable levels of clarity. 

What that would require is a type-aware system which itself has no types at all. That's difficult to write without
Forth. 
