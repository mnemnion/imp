#Vocabulary

Orcish is mostly about Orcs talking to other Orcs, and higher level Playas in the Possi.

There are a few absolutely essential words to make an Orc Forth complete. That's a higher bar than Turing complete, but not much. 

The thing about Orcs, they don't have to do what you say. But they'll understand you. That's a powerful combination even for a dumb brute. Not all Orcs are dumb; there's a lot of fast silicon out there, and stack machines, as we'll demonstrate, are a force of nature. 

You tell an Orc `2 dup *` you'll get a `WAGI!!!`. Orcish would be `2 D *`, because we never speak when we can grunt. An Orc who isn't feeling surly will have a `4` on the sak at this point. 

All the differences between Forth and Orcish stem from two roots: we care only slightly about reading and writing Orcish, and Orcs spend a lot of time talking to each other, rather than to a user. Also, there's not a lot of room, though by Forth standards we normally have more than enough space for a full stack. The Trinket is laughably small compared to the usual cheap 32 bit ARM candy that gets stuck onto everything, and an ordinary Arduino runs any of several flavors of Forth, hansomely, with room to spare, right now.

We use only that which may be cut and pasted. That's a core tenet of Orcish. 


###Orcish Dictionary

Orcs start with the oldest definition, and can't usually learn a new word as a result. That's a feature: Fancy backwards speakers are Dwarvish or worse, and not to be trusted. 

Orcs also don't use ASCII control codes, as a matter of course. They spit printable unprintables at you. 


###Orcish communication

Orcs can see an entire byte, but they can't hear it. Certain values are above and below their threshold of hearing: if a pin is said to be an `ea r`, they will only catch so much when a routine calls `He er`. When they `To k`, they can only say so much, the threshold being by nature identical. An `ey e` can of course `Ei` anything, and a `.` can emit anything. 

Typically, so as to be taciturn and save cells, an Orc will simply not hear you at all, and consequently, say nothing in reply. A clever Orc may hear you, and seethe with inner rage that you have addressed him incorrectly. 

Be wary of Orcs, my friend. They are contagious. 


###Wun Letta Werdz

Orcs of course have Werdz. One byte words are the core vocabulary of communication and use an eeprom offset table in compatible chipz. They have as much punch as possible.


###Tu Letta Werdz

Anything else an Orc understands is likely to be two letters long. Mnemonic wherever possible, as with the core words. Longer words will be misunderstood as a more aggressive form of communication: truncation is not performed by the Orc. An Orc can `He`, you tell it to `hear` or `listen` or `key`, something civilized like that, and the protocol droid takes care of it. Or you Orc at it. Your choice. 

These live in a dictionary: stunted and primitive, or surprisingly comprehensive and through. Many Orcs can recite all of Hamlet in the original Orcish. 


###Tre Letta Werdz

Fuk you, izz wut. Orcs mislike wasting a byte.

There are only one and two letter words in Orcish. Any additional letters are arguments to the command, if the command expects arguments. If it doesn't, the whole word is parsed, and will be an error unless the Orc speaks another language. Which may prove to be quite common.

Remember, when Orcs hear a word they don't understand they do absolutely nothing and keep listening. Unless they do something else. The least likely thing is that they'll destroy stack state and barf at you.  


###Forr Letta Werdz.

Orcs sometimes speak in four letter words. Sometimes these are five letters long, because Orcs can't spell. 

The last four letters encode something, either at 2 bits or 4 bits. The first letter, if present, may have significance. Hard to tell with Orcs. The type is encoded by choice of letter, the value by length of repetition.

When this happens, the first two letters are a command, telling the Orc it's receiving a four-letter word of a particular type. They are parsed in a consistent way then handed off to a unique handler based on type. Hence, four-letter words are five letters long, six if you count the space, which we never do. Words that are actually four letters long are Tre Letta Werz with two commands. Yeah, it's confusing. Orcs can't count, or spell. They think 10 means 16. Iz nutz.

This may prove to be the essence of Orcish. It's a gateway to communicating in noisy, degreded, hostile environments, allowing an essential message to carry a detailed one. Also, it is confusing to non-Orcish life, which simply doesn't think this way and must be induced to accept it. The principle is well known among engineers who read Shannon. Who will understand many aspects of the Orcish way, I hope.

The non-printable ASCII byte values are a noise bridge. We gate it by neglecting those values. I hope this is easy to understand. 


###Middulorc

Orcish may be spoken in a slightly expanded form that is more mnemonic. Not to Orcs, but a simple truncation program should suffice. It'll still look weird: prudes and noobs will want to use Forth. If you want to actually learn Orcish, a prettyprinter that makes it into Middulorc will help. 