#Fast or Smart?

I'm starting to realize I can make the Orc core really, really small. If I make it a lot slower. Like, a *lot* slower. It would be an actual interpreter, and most of the memory would have ASCII in it. 

Orc itself is a series of one or two byte commands. The spaces aren't needed, because we have a high bit to indicate a space. 

We use an offset table to calculate the location of each command, and jump. We retain the offset, and count down against the offset. The code itself is dense. Two letter commands are in an offset-calculated linked list, treated the same way.

Slow. Really slow! I'm not trying to pwn pocket calculators. This actually used to be a thing, there were Huffman encoded token-threaded offset-calculated tiny Forths on some gear that's just ridiculously small. 

Really, I'm just going to pay the word cost of indirect threading. Orcs are powerful, and kinda dumb, but fast. 

Still, for some purposes it may be necessary to make an Orc with more brains than brawn. Since I've spent so much time thinking about it, here's how to do it.

##Brainy, slow Orc

The dictionary allocates exactly one cell per one letter word, and two for two letter words. The code itself is all bytes, with the high bit used to indicate spaces, except for primitives, which are directly executed. e forth has 32; we use unprintable ASCII values for the primitives and make them headless. 

The inner loop takes a byte, and checks the space bit. If it's flipped, we have a single word instruction, and it searches the dictionary for it by offset incrementation. It then uses the offset to count down the instructions in the word, and executes a byte at a time. 

We then profile the existing Orcish code base and use Huffman encoding to assign the order of the dictionary. This wouldn't actually be that slow, in the grand scheme of things, and it would be possible to make it smarter per kilobyte. 

This might come in handy for pwning small but mission critical chipz, like on keyboards. It might actually never prove useful, the state of fabrication being what it is. Nanodust, though; there's always a chip with less room than you might expect.