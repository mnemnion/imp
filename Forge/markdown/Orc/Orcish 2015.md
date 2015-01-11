#Notes on Orcish

Orcish is an approach to computing with an emphasis on multi-chip, heterogenous, opaque, and potentially hostile environments. The original notes on Orcish were still closely tied to Forth as a development and programming environment. Conceptually, Oricsh is now more minimal and ambitious than this. 

Orcish is a language. Not a programming language per se. Some Orcs may be incapable of being reprogrammed, or simply unwilling. It is a brute language, meant as a Lingua Franca for chips, one that humans can reasonably learn and work with.

Try not to map the concepts in Orcish directly to more familiar metaphors. Orcish itself is quite literal and easy to understand. But it is a language, more in the sense that English is a language than in the sense that C is. Forth lies somewhere between. 

##Phonemes

The Orcish language is clean, printable ASCII. Anything outside of that spectrum is undefined behavior under most circumstances. In general Orcs ignore anything they don't understand: a glare is frequently all the error message needed. 
Orcish is made up of werdz. **werdz** are separated by whitespace. That means either `20` or `0A`, and they have different meanings. If you insert tabs into your Orcish, you are stupid, and deserve the resulting bugs. Either of these characters is considered a `spaz` by an Orc.

Besides spazez, Orcs recognize `numbaz`, which are in the range `0-9a-f`, `lettaz` in the range `A-Zg-z`, and `runez` which are the set `` `~!@#$%^&*()_-+={[}]|\:;"'<,>/? ``. Note that `a-f` are always **numbaz**, independant of context. 

The `spaz`, `numba`, `letta` and `roon` are the phonemic categories of Orcish, from which we construct an equally brute, simple-minded syntax. 

##Syntax

Orcs parlay through the simplest parser that could possibly work. If they receive a byte that isn't phonemically valid, undefined behavior occurs, which normally includes ignoring you, and may also provoke consternation or even hostility. Given a `numba`, they try to make a number out of it, and remember that number. They will stop if they see a `spaz` or indeed any phoneme which is not a `numba`. How many numbaz they eat before digesting depends on the Orc. I don't know or care what your architectural width is, nor which end it cracks the egg from. Orcish is a language, it's up to the users to understand each other. 

If an Orc can't make a `numba`, it makes a `slang`. This is either one `letta` or `roon` followed by a `spaz`, or it is one `letta` or `roon` followed by a non-spaz phoneme. 

What's a slang? Every other command we need. The shortest slangs, and ones with a `roon` in them, are reserved for the core language, while two-letta slangs or letta-numba slangs are anyone's guess. 

##Handshake

The Orcish Cheer is how Orcs establish a line of communication. 

Orcs in the wild may be presumed to be busy. Their inner loop could be quite tight, leaving little time to poll a pin for data. The Orcish Cheer is emitted at 360 baud, no more, no less. 

The Cheer consists of the string `W@g!`, which is pronounced `ⱱaaʀχ!`, more or less. The response is `@rc!`, typically, and this is pronounced "Orc!" as one would expect. 

Note that each of these is two slangz long. When an Orc makes a slang, the parser simply resets. Numbaz get eaten to a certain width and then reset also. 'certain width' being as unreliable as it sounds: I neither know nor care what a comfortable way to break up a numba might be, nor the endianity of the architecture. 

There is some structure to the madness: the `@` symbol in both cases indicates message passing, and the `!` has an imperative nature when preceded by a letta. 

`W@g!` is the only valid call, while the responses vary. `@rc!` means the Orc is now listening and in the mood for conversation, at the established baud rate. `@rc.` means the Orc is an Orc, or pretending to be, but will most likely ignore you unless you attract his attention in another fashion. `@rc:` is an Orc reporting for duty: it has things to tell you, which come after the `:`, and serve to establish further conveniences. 