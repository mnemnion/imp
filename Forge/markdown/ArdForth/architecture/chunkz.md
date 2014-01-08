#Chunkz

I think recursive calls from the thinks is too much for a poor Orc to handle. So what we can do, is define chunkz. 

A chunk is a word that goes in front of the pak, exactly where the next word should go. Space is allocated in the chunk for a forward pointer and a glyph, and the word is written to the Flash in the normal way. We can store a limited number of chunks, which go in the drp along with their temporary name. Orcs check the drp before the pak, when the tawka is interpreting. 

Why is this cool: we teach an Orc say three words. There are now three chunkz in front of the Pak; we could write sophisticated code to link them together.

We won't bother. We'll simply define the same words, in the same order. The Orc will write the glyph first, to empty memory, then 'write' the word. The cool thing about our Flash write is, it checks to see if the cell is already holding that value. If it is, it does nothing. So it 'writes' the word, writes the forward pointer, and advances the pak pointer. In effect, we only change the reserved celz. 

If you teach it new words instead, it overwrites the chunkz, and no harm done. In effect, the forward Flash is a scratchpad, which we may also use for string stashing. This indicates the spleen will hold a chunk pointer.