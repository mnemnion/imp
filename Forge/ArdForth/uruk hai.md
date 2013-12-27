#Uruk Hai

Uruk is our metacompiler and sargeant operating system. It speaks an ordinary Forth, and translates to various species of Orcish as appropriate. We intend that it will fit on an Arduino Uno, thus:

ATMega328, 32 kB Flash -.5kB bootloader, 2 kb SRAM, 1kB EEPROM.

That is moderately tight. We'll add a SD card, making storage enormous but palpably slow. We can afford to do some swapping into our Flash, but not as part of algorithms. 

Uruk needn't be smart, merely fluent. His role is to command subordinates in their dialect, and to keep track of everything that ever goes into a micro. 

During development we can use an onboard protocol droid running Forge; Uruk is for working with installed systems. We need a sargeant per an undetermined number of systems, probably having more to do with the pinouts on a given board than the capability of the micro. 

Uruk runs OrcForth, but with proper, human readable words. It also knows the Orcish for the fundamental commands, within the dictionary, so that it can translate any given word into Low Speech. It maintains dictionaries for commanding specific micros, who by the nature of this compound operating system have idiosyncratic ways of talking. 

The last bit is one reason we need an SD. A given Uruk will have as many useful words as possible for commanding subordinates, in Flash. It will keep a block of memory free for loading other words in from the master dictionary. There will be a moderately complex routine that does fetch and store from the master block, which is monotonically allocated and never freed. Higher ranked computers can clean up old and dusty sargeants, if this ever happens. Which, at 8 gB vs 8-32kB, isn't going to be common unless there's significant logging. 

Since the language of the Uruk Hai is a combination of Low Orcish dialect and the Common Tongue, we need OrcOs running first. 

If at all possible, we will include assembler words for AVR systems. This should fit: we have roughly 20 times as much room as we're alloting for the operating system.