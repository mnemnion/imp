#Notes on Orcish

Orcish is an approach to computing with an emphasis on multi-chip, heterogenous, opaque, and potentially hostile environments. The original notes on Orcish were still closely tied to Forth as a development and programming environment. Conceptually, Oricsh is now more minimal and ambitious than this. 

Orcish is a language. Not a programming language per se. Some Orcs may be incapable of being reprogrammed, or simply unwilling. It is a brute language, meant as a Lingua Franca for chips, one that humans can reasonably learn and work with.

Try not to map the concepts in Orcish directly to more familiar metaphors. Orcish itself is quite literal and easy to understand. But it is a language, more in the sense that English is a language than in the sense that C is. Forth lies somewhere between. 

##Principles

The Orcish language is clean, printable ASCII. Anything outside of that spectrum is undefined behavior under most circumstances. In general Orcs ignore anything they don't understand: a glare is frequently all the error message needed. 
Orcish is made up of werdz. **werdz** are separated by whitespace. That means either `20` or `0A`, and they have different meanings. If you insert tabs into your Orcish, you are stupid, and deserve the resulting bugs. Either of these characters is considered a `spaz` by an Orc.

**werdz** are either numbaz or slang. `0-9a-f` turn into hex, which is the only base Orcs understand or use. Orcs try to make a numba after each spaz, then they make a slang. A slang is any printable ASCII sequence that doesn't start with a numba and is either one or two bytes long. 

That is the phonetic structure of Orcish. Anything more complex than this is worthy of a complete language, of sufficient size to the task. Orcish is intended to be the lowest-cost abstraction possible, so that it's just good sense to choose an Orcish phrase for any given communication. 

##Handshake

The Orcish Cheer is how Orcs establish a line of communication. 

Orcs in the wild may be presumed to be busy. Their inner loop could be quite tight, leaving little time to poll a pin for data. The Orcish Cheer is emitted at 360 baud, no more, no less. 

The Cheer consists of the string `W@g!`, which is pronounced `ⱱaaʀχ!`, more or less. The response is `@rc!`, typically, and this is pronounced "Orc!" as one would expect. 