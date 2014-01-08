#OrcOS

How small can this Forth business get, and still retain the fundamental interactivity?

Small.

A Forth word is anything that isn't a space. Since we're talking to our machine indirectly, we can use any value that isn't ` ` as a word.

Basically, we want to Demoscene as much Forth as we can into 1k of Atmel. This sounds like a lot of fun to me. Perhaps I'm strange. 

There are 98 single characters which can be typed without difficulty using my keyboard. That's quite a bit more than Chuck's 32 opcodes. We need 16 of them for the hex numbers, so that's 82 we can use for op words. 

There are 8036 words we can address with two characters. Far more than we need. 

Here's the premise: we use our laptops do do the fancy business. Like writing a mojo Forth in 1k Atmel. We then use that as the base layer of every chip in the family. 

We have a larger Arduino that basically just sits around being a dictionary. We plug an 8gb SD into it and never think about it again. Everything that crosses the wire just gets written in; it's up to the laptop to use what's there instead of rewriting things. The papa arduino is a protocol droid that speaks ordinary Forth and can translate it into the tiny language. It also understands reading and writing cards. The Arduino Uno has 32 kb onboard; when it reaches the bottom of that dictionary, it hits the stacks. All relevant functionality will survive swapping in a clean SD; the archives are there to make life easy. It has 2k of SRAM, which is tight for almost anything, but will suffice. 

I really have no idea how to write a picoForth that fits in 1k of tiny Arduino. I'm just pretty sure it can be done, that's a fair amount of computer to set aside for such a task in the early Forth days. I may even be acting miserly with the single character names business, but miserly is good when you've got 8 blocks to work with, like the Trinket and Gemma. 

Why even bother? Well, even with 512 bytes of SRAM, we can still (probably) read in and interpret say 16 bytes at a time of data. So this way our processors can tell each other what to do without resorting to direct object code. Forth is typically much more compact than the resulting object code, which is much of the point. 

##Problem Oriented Language

What I'm proposing is so unfriendly, we could almost go ahead and just use assembler. But then we wouldn't have a high level interactive language, would we? It turns out these are not properties of superficial syntax; Problem Oriented Language makes that clear. 

The fundamental parse loop is: try and make a number, if that fails, try and make a word. If that fails, you have failed. It's a clever, readable Shannon tradeoff, preserving the bottommost numbers as instructions. For that reason, we should probably extract `a-f` from word consideration. The eternal question: is a1 worth making a number? For us, probably; we use ordinary ASCII and try to be mnemonic, so the raw bytestream is not upsetting to look at. It's cryptic, of course; it's a compressed form of what we write. 82 operators left for the base set. many/most are already defined.

I've never really tried to think in assembler before. This is fun. We really only get 1023 bytes, because Chuck says "never put anything in your zero byte" and I trust Chuck. It's the Tao byte: it should be zero, if it isn't, something happened.

that's 511 words and a byte. Probably leave the whole bottom word alone. One may readily see why I don't wish to waste bytes on superfluous ASCII when I could own a mastercontroller. I barely need that, but there are good reasons to get chaining into the workflow early: an Arduino, unlike a laptop, can be stepped, and can have its state exported and inspected elsewhere. We may simulate the protocol droid in its entirety, lacking only the realtime guarantees. 

Something cool about this is that we can know, to the cycle, how long each word is on the processor. that's good mojo to push upstream: Forge can optimize out some timing stuff by just knowing when operations have to happen.


##Architecture

We're going to need every dirty trick in the book, my friends. These notes will be subject to frequent revision.

We really actually want it to fit in 1kB, that's 512 words and can't possibly be enough, so we're using up to 256 bits of the eeprom as well. It can't possibly do everything; it can only do enough.

There's no requirement that these bytes be densely packed in. Two or even three regions of Flash would suffice, and for various reasons we want 0-256 of the EEPROM. 

We absolutely require an interrupt-driven, indirect threaded inner loop. We can't afford the overhead of using a linked list for our core words, however. 

###1024 + 256

We count down in bytes. If you see an odd number of bytes on the left, you see a problem.

The AmForth inner loop is 16 instructions. Can we improve it? Hah. I see one instruction that can maybe be shaved, a jump from `interrupt` to `execute`. Call it 16.

###1008 + 256

We now need words. Some can be headless, meaning there's no way to reach them from the interpreter if you don't happen to know the address. Our core control vocabulary is 78 instructions, the number of printing characters needed to avoid collision with hexidecimal. 

We handle these by building an offset table for each instruction, which may be up to 32 operations long. Instead of following a linked list, we accumulate offsets until we have a jump from the table head. No need to provide the character or a link, just code with `next` or `exit`. 

That's as dense as my ingenuity can make it. Unless I come up with something better, this is how we'll do it. 

##Dictionary

The rest of memory is, par Forth, a dictionary. We have special optimizations for cell-length words, but otherwise carry on in the usual indirect linked list fashion. We hope to fit at least the tiny tip of the dictionary in the core operating system. 

##Tail Optimization

Not "tail call" mind you.

With a bunch of headless words sitting around, we can perform tail optimization: running backwards from each exit point, looking for common code patterns. If we write carefully, we can shave some precious words where it matters most. 

If a headless word ends up at the end of a function, we can also inline the headless word and use that tail. In fact, if we have to break up a word so that every headless word ends up inlined inside a definition, that's probably a tradeoff worth making.

Note that 'exit' and 'next' are different beasts for this purpose, because they have different stack effects. 

This could be efficiently accomplished by splitting a word up into two quasiwords and a word that calls both. Say they're both 8 words long (so a big word), and the quasiword we need is the last 5 of the first 8. 

Premature optimization is the root of evil. Knuth also extolls literate programming, and the literature to program ratio is certainly quite high today! 

This is something we do later: we start by defining the minimal OS, then we squeeze it. 



