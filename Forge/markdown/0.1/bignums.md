#Bignums in Labrys

I see two good ways to do it. 

The easy way, and what I'll do first, is allocate blocks of memory prior to noun creation, in fact prior to all other allocation, and 
compile the address above the BigNum space. Currently, we test for atoms by checking negativity, and test for cells by checking for positive. This is of course very fast. 

It is probably just as fast to make the cellularity test above the hard-compiled memory threshold. Surely, if it is slower, it's by one cycle at most. We then have a third condition which we have to handle, where the bignum address is a noun that must be manipulated with
different words. Anytime a bignum gets pushed onto the stack we have to do slow things for awhile, but when it's gone it's gone and we're back to status quo. 

The other way isn't that different, and is putting the bignum on a card. In this case, the cards would be below threshold. 