#Forge

Labrys is taking me in the direction of some deep Forth hacking. To make that work I'm going to want a portable, minimal,
somewhat quirky editor.

Basically so that I can write flint and work with it usefully, so that handle's type annotation is the second implementation
of the idea. Hacking Forth to follow Ax conventions is a project in itself, and the flint thing should be targeted to ANS 
Forth first. 

I also want to write an ANSI/Xterm mode Hoon editor, or be a major player in that game. Forth... forth is like Lego. I dig it. 

I'll stop before I turn it into a shell and add Git integration. Because we want it to be an Arvo shell with Clay integration!


##Principles

Vim syntax, Emacs inspiration, Forth structure. 

##Buffers

Any text editor needs buffers, of course. I'm going do do mine with 'one weird trick'. It's probably a very old weird trick,
but basically the buffers will be twice the necessary size, and characters will be on odd offsets. Even offsets will store 
up to 256 commands. So basically metadata slots for each character. 

We might want ordinary old buffers at some point. But I'd rather handle large stuff by, well, buffering it. 

Every buffer has an offset for metadata at the beginning. This should be 256 bytes.   

Most of this is used for mark sets, syntax highlighting, and so on. Forth being what it is, it's clever and quick to leave data in place.

Really we have 16 bits of data, 8 of which is our printing char. We can interpret the meta byte in reference to the para byte, quite 
profitably, in many cases. It's a rich data format.  

The actual zero byte will be an offset to where the buffer starts, so we have 254 bytes of metadata available per buffer, which needn't be fully allocated for a given buffer. We can fit a whole card in there with room to spare, which is exactly how we interpret an offset of 136: the rest of the 0 slot is reserved, and slots 1-16 hold a card. 

Note that normally buffers go on cards, rather than cards belonging at the front of buffers. I'm sure it will be handy to be
able to stick one in, though; they are our basic compound type. 

Buffers are not as similar to files as they are in most text editors. Some files will be a stack of modal buffers, which presents a seamless appearance but different behavior depending on cursor location. So eventually we can edit Marmalade and switch modes when we enter code blocks.

Buffers do much less than in Emacs, while cards do more. 

Important note to self: Unicode will not function if the printing bytes aren't handled carefully. UTF-8 allows for up to four bytes per printed character. I'm not starting by writing a full Unicode library, so using characters that want to be double wide might break things for awhile. But I do want to make sure we don't chop characters in half, which sucks, so some UTF-8 awareness is mandatory. 

One can't simply print a byte at a time to the terminal, because bytes aren't characters in Unicode. We concatenate commands and strings and execute as one long print. Faster anyway, I expect, not that it matters. Really we should be 'printing' a buffer on read into an intermediate form, for fast scrolling. 

##Cards 

Forge will have one compound type: cards. A file is represented as a card with one or more buffers. A frame is a card with certain behaviors, holding other cards including whatever buffers it's displaying. Something like a spellchecking module is a card, not a mode or something distinct like a 'module' : cards can have their own Forth dictionary that can be lifted into container cards, effectively namespacing each card so decorated. Cards can be loaded by modes, and often are: they can also be sideloaded, and there is of course an earl card that holds all cards in the deck. 

That's basically the master Forth dictionary plus all the rest of the everything, so "Earl Card" and "Forge environment" are roughly synonymous. 

A card is fundamentally a link to a Forth dictionary (eventually an Ax noun) with a set of cells, conventionally sixteen, which hold cards, buffers, and the like. There's a type table at the front (0 cell) describing the nature of the card and the contents of the cells. The sixteenth cell is either zero or a link to the next card: a linked list of such cards is a 'fold'. When stored, the sixteenth cell holds a card checksum instead of a continuation address (or -1), and 0 for the end of the card.

A "deck" is a single card, which could be a fold, compactly represented with all the data on it, recursively, to some useful point. The whole of Forge is just a card, and could be and should be decked out for transportation. Cards living in memory take up more room, and the environment itself is worth just straight loading from the store. I want Forge to boot like Nano and act like Emacs. That 
means no Calvinball. 

I'm pretty sure this is what Forth calls a "block", given 64 bit cells. Just in case, we'll allot things that are bytes in bytes and things that are cells in cells. 

Note that good Handle, and I daresay good Forth, is car down, that is, when I say the 'first' cell it's the one that ends up on top of the stack after a load. 


##Modes

Each buffer has a single mode, pre-composed of major and minor modes.

Modes have two functions: dictate the meaning of keys, and interpret the metadata. Buffers are not a compound type, remember.

Modes are stored on cards, and have interpretation tables for metadata and keystrokes, which dispatch directly on bytes via pointer arithmetic into the word table. The mode card has a dictionary that loads the offsets and holds the word definitions, which aren't exposed in the mode itself. You reverse lookup against the card: A mode itself is a set of composed words with offsets
corresponding to metadata values and keystroke values. 

This limits the Calvinball effect that Emacs is notorious for: you are always in a particular mode, composed by the buffer your cursor is in, and a keystroke will constant time do exactly one thing. We are manipulating a concatenative stack language, so naturally, you press a maximum of two keys at a given time: since we have a stack, there's no reason to ever ever Ctr-letter Ctr-letter, because Ctr-letter letter and Ctr letter letter will do the same thing and the second Ctr will be a no op, since Ctr just shifts to the Ctr register and drops down to either the command register or the insert register when the word exits, depending on which flavor you favor.

There are major and minor modes. A major mode is composed first and is dense, while minor modes are sparse and loaded afterwards in a specific order. A buffer just has a "mode", with data on a card (or in the metadata) showing the components, major and minor, of the mode. 

I'll be playing with the UI, but I think I'd favor typing ctrl release to get to the command plane and having insert-self be the default behavior, with Command itself as a temporary jump-up into the command plane. The UI is of course completely Choosaphonic; I intend to provide sensible defaults, for my own value of sensible. 

We have five registers we can work with, shift, command/super, alt, option, and hyper/function: that's a lot of keys. 


##Frames

This is probably enough abstraction with which to start writing. A frame is a card that displays other cards which hold buffers. A card is fundamentally an offset to a null-terminated array of addresses, so if the frame is showing only one buffer we need the frame card, the buffer card, which has the mode in it, and the buffer.  

Frame cards control things like cursor position, mark sets, and so on. There is always an out-of-band card holding a copy of the display, so we prevent alignment problems by the simple expedient of checking that our display byte matches our buffer's char byte for that location. This copy is also a buffer, and the metadata is probably just the width of the following byte on the screen.

Some deep Unicode wizardry, added later, can make this work correctly. I'm going to write it fairly well and avoid the hard parts of Unicode world. Forge will most likely never be bi di, at least not by my hand. Handling the intricacy of the ANSI terminal is quite enough for me. 

The really dumb way to avoid cutting 2 bytes in half is to have a metabyte between that says "don't cut here". I like doing stupid 
when stuck with other people's smart. 

#Ax

Forge is a place to forge Forth and chip flint, but also a place to forge Ax. Basically we'll have an Ax machine that's its own Forth stack as soon as possible, and port Forge to it piecemeal. Since Ax is symbolic, the line between a "jet" and a "word" is very blurry indeed. 

I feel like we'll be in pretty good shape when the Forge environment is carl and Ax is earl over it. So we'll "put" nouns to Forth when we want to talk to it, and the terminal and keyboard will be underneath the Ax, so that we br 0 for input or to pull Forth memory into the Ax environment. I'm actually pretty okay with writing carl software directly, and it's kinda like a jet that uses `10`, which is amusing when ya thing about it. 

## Nouns

Forge is a prototype for a noun-aware Urbit editor. We want it to be noun-aware, eventually; we'll start with word-awareness.

Forth, conveniently, has a dictionary, so we can keep around our source and annotations and do things like show word definitions in a separate frame. 

Forth has word definitions and statements, and that's pretty much it. Some successor to forge will use buffers as input and manipulation areas for nouns; that should be easier to write in Hoon. This is another reason we allocate double memory for metadata. Syntax highlighting is nice, but syntax awareness is the actually important part. 

In the meantime I'm writing a Forth environment to write more Ax in. Step one is to put together a dictionary of card words. 