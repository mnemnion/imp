#Crypto Suite

That's almost a joke. But not really. 

For a one byte command, we actually send two bytes. We make it by consuming another byte of discarded entropy. There are two ways to make an XOR 0, and two to make an XOR 1, and we choose the two, making duals of the command. These duals are then XORed across the pad. We reverse this to obtain the command. 

Then, we send a brand new pad, in the clear. That's XORed across the original pad, which we retained. This XOR is now the new pad. 

Because we never consume the secrecy of the original pad, we're good. If we suspect compromise, we get a genuinely new pad in there somehow. Most likely we zero everything associated with the micro and start over. 

This seems almost too simple to work. I can't see what's wrong with it. If either end is compromised, the crypto is of course useless. But the two bytes should be perfectly entropic, revealing neither the pad nor the command. 

It only works if the controller doesn't do anything an adversary can view in response to the command. That makes this technique of limited utility. If you can narrow the number down at all, you've gained signal against the pad.

It can be used for superusering, which is perhaps the most important. Send 8 bytes, get 4 bytes of security, send 2 back if there's an entropy source. Otherwise, one, but you have to scrub it out. Your 5 words can be reset in the clear, 4 if you can't squeeze entropy from the chip. 

It's cheating, is what it is. Far safer is to load up a twister, pull your emergency entropy supply, and pull red bits hot off the line. A twister is a respectably large word. 

