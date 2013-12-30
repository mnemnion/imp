#Words

A word, in Forth, is potentially lengthy. In Orcish, it is short: a character when possible. There are 16 characters needed for numbers;
although we could insist on a leading zero, we favor allowing for 78 core words. 

I'm willing to trade off size in the interpreter, and link walker, for really, *really* small core words. Even a few instructions, but let's not get crazy.

##Core Orcish

We want those words to take up as little room as possible. Each one needs an instruction to end it, so that's one word, per word. 

How do we get to that word? A linked list would cost us another word-per-word, plus the byte so we know we've found `q`. That's not cost effective with our kind of margins.

Here's a squeeze: We have some EEPROM. Why not make an offset calculated straight from the single letter show the jump into the table? Then we don't need the byte, or the offset. So take #. It's the third character, so we set a counter equal to three and count the first three offsets from the EEPROM into an accumulator. We now have the jump into the Orc table. We now need one word per word to get out of there, either 'next' or EXIT. So there's 78 words of padding out of 512, leaving us with 434 words of Flash and 174 bytes of EEPROM. 

Technically, we have 3*78 = 234 bits that we can use within the eeprom, because we flag off the offset before adding it. This is still much faster than traversing a linked list. 

EEPROM is maybe not the best choice in which case we need an extra 39 words for the offsets, leaving 395 words of Flash but the full 256 bytes of eeprom. The eeprom can be written ten times as often as the flash, but is awkward to fetch instructions from. 

That saves us two words: a primitive now needs one word, and a word defined in terms of other words needs another. Clearly we save code if we have to use another word, if that word is longer than one instruction, and they all are. 

##A (Dictionary) Word

If it's not one word, it's two words, or we're speaking Uruk at least. At this point the dictionary kicks in. We might still benefit from offsetting, since we have all those tasty flags. We need to be able to compile in strings and at least allow for lengthy word names; it's only fair, and Uruk is based on Orcish and Forth. But we need another byte, which means we use another word to stay aligned, and it's linked list time. 

If the first character is printable, the second char is the whole word. Otherwise all bets are off: we can, and will, keep lots of good stuff in the dictionary. That's why Chuck put it there. 

The minimal structure for the dictionary would seem to be a compact, monotonically increasing dictionary, with a minimal cost of two words per entry. I don't think we can do it for less, and even that is one word less than we're used to. 

Basically, the high byte has the definition offset, and the low byte has the data offset: add them together, you get the next entry. I think we can afford to limit names to 16 characters considering we'll normally use two, so we have a luxurious sixteen flags in the high byte. 

I don't see an advantage for linked lists in a Harvard machine since actually interpreting is a quick job regardless. Adding will actually be faster for 1-char words, but slower than just 'check and jump', since we check, dup, mask, add, and jump. But we save a word, per word. We're also using word arithmetic, so we can allocate an impressive half a kilobyte of dictionary space as one thing. That's a lot of dictionary in orcish. 


##Quasiwords

We'll want some words that are absent such graces as links and names: pure execution with a `.next`. We might even use `` ` `` like Lisp, but I like `~`, in Uruk, where we will perforce refer to them. I'm thinking that weird little i/o quirks and various kinds of nonsense will be quasiwords. They're small: think of the actual words of the OS like shell commands, and quasiwords as weird hacks that you can do if you have to, that mostly the OS would want to do. Really, unless you're programming in pure Orc, the difference won't be apparent, but a quasiword can't correlate its execution token to its own name. They must be at least three letters long, including the quasi. 

A protocol droid or Uruk well familiar with the internals of the imp in question can call quasiwords, presuming there's no superego to prevent it. 
