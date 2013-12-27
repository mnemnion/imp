#ArdForth

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

Something cool about this is that we can know to the cycle how long each word is on the processor. that's good mojo to push upstream: Forge can optimize out some timing stuff by just knowing when operations have to happen.