#Memory Space

Ideally, we fit it into 1k at the bottom of the address space. 

Less ideally, we fit it into 1k, but it is somewhat spread out. The holes have to be usefully large, because we're not wasting space, period. 

First we write it, then we pack it in. If I can get the first edition in under 4k I'm doing great. 

##ID

We provide a 64 bit checksum, that is the edition. It is not self-calculated, though Uruk will need this capability. Checksums are not weighty operations. There's another 8 bytes gone. Cost of doing business. 


##Count


