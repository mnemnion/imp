#Notes on Orcish

Orcish is an approach to computing with an emphasis on multi-chip, heterogenous, opaque, and potentially hostile environments. The original notes on Orcish were still closely tied to Forth as a development and programming environment. Conceptually, Oricsh is now more minimal and ambitious than this. 

Orcish is a language. Not a programming language per se. Some Orcs may be incapable of being reprogrammed, or simply unwilling. It is a brute language, meant as a Lingua Franca for chips, one that humans can reasonably learn and work with.

Try not to map the concepts in Orcish directly to more familiar metaphors. Orcish itself is quite literal and easy to understand. But it is a language, more in the sense that English is a language than in the sense that C is. Forth lies somewhere between. 

##Phonemes

The Orcish language is clean, printable ASCII. Anything outside of that spectrum is undefined behavior under most circumstances. In general Orcs ignore anything they don't understand: a glare is frequently all the error message needed. 
Orcish is made up of werdz. **werdz** are separated by whitespace. That means either `20` or `0A`, and they have different meanings. If you insert tabs into your Orcish, you are stupid, and deserve the resulting bugs. Either of these characters is considered a `spaz` by an Orc.

Besides spazez, Orcs recognize `numbaz`, which are in the range `0-9a-f`, `lettaz` in the range `A-Zg-z`, and `runez` which are the set `` `~!@#$%^&*()_-+={[}]|\:;"'<,>/? ``. Note that `a-f` are always **numbaz**, independant of context. 

The `spaz`, `numba`, `letta` and `roon` are the phonemic categories of Orcish, from which we construct an equally brute, simple-minded syntax. 

#### jklmno

Arguably our `numba` range should be `jklmno`, which is `6A-6F` in ASCII and means we apply a consistent bitmask to all `numbaz`. I abhor waste, but won't do this unless I can prove that it's faster than converting a more conventional representation, on at least Atmels and Arms. 

The really steez alternative is to replace them with `:;<=>?`, we mask for the `3` and take the low four. I won't do that, it would render the language unreadable. 

##Words

Orcs parlay through the simplest parser that could possibly work. If they receive a byte that isn't phonemically valid, undefined behavior occurs, which normally includes ignoring you, and may also provoke consternation or even hostility. Given a `numba`, they try to make a number out of it, and remember that number. They will stop if they see a `spaz` or indeed any phoneme which is not a `numba`. How many numbaz they eat before digesting depends on the Orc. I don't know or care what your architectural width is, nor which end it cracks the egg from. Orcish is a language, it's up to the users to understand each other. 

If an Orc can't make a `numba`, it makes a `slang`. This is either one `letta` or `roon` followed by a `spaz`, or it is one `letta` or `roon` followed by a non-spaz phoneme. 

What's a slang? Every other command we need. The shortest slangs, and ones with a `roon` in them, are reserved for the core language, while two-letta slangs or letta-numba slangs are anyone's guess. 

##Handshake

The Orcish Cheer is how Orcs establish a line of communication. 

Orcs in the wild may be presumed to be busy. Their inner loop could be quite tight, leaving little time to poll a pin for data. The Orcish Cheer is emitted at 360 baud, no more, no less. 

The Cheer consists of the string `W@g!`, which is pronounced `ⱱaaʀχ!`, more or less. The response is `@rc!`, typically, and this is pronounced "Orc!" as one would expect. 

Note that each of these is two slangz long. When an Orc makes a slang, the parser simply resets. Numbaz get eaten to a certain width and then reset also. 'certain width' being as unreliable as it sounds: I neither know nor care what a comfortable way to break up a numba might be, nor the endianity of the architecture. The Orcish language contains only the hexadecimal base, as is proper. 

There is some structure to the madness: the `@` symbol in both cases indicates message passing, and the `!` has an imperative nature when preceded by a letta. 

`W@g!` is the only valid call, while the responses vary. `@rk!` means the Orc is now listening and in the mood for conversation, at the established baud rate. `@rk.` means the Orc is an Orc, or pretending to be, but will most likely ignore you unless you attract his attention in another fashion. `@rk:` is an Orc reporting for duty: it has things to tell you, which come after the `:`, and serve to establish further conveniences. 

###Architectural assumptions

Here we're going to dial out a bit. What is the point of all this?

Simply put: leaving straps on the boot. The original concept of Orcish was closely tied to Forth, and it inherits the philosophy that a computer, no matter how small, should be interactive and capable of being programmed incrementally. 

Orcish is now a language, with dramatically fewer assumptions. Even a burnt ROM can spare enough bits to reply, in broken Orcish, that it has certain abilities. This chip, which we can't really call an Orc, might not even know that `a0 D` is equivalent to saying `a0 a0`. 

The minimum a chip which speaks Orcish might be expected to do is tell you what it can do, and do it when asked. An actual Orc will contain an interpreter, which may be moderately sophisticated, or may not be, but will suffice to operate the Orc, inclusive of programming it with new behavior.

###Syntax

I would like to thank Alan Kay and Chuck Moore for their relentlessness in the pursuit of the good.

Orcish is an agglutinative language. The parser is a state machine, each word puts it into a new state and each return falls out to the same starting place. This allows us to make very few assumptions, as we have combinatoric richness available. 

To illustrate, `W@g!` could be defined as precisely the same as `W@ g!` and I might choose to do so. Equally possible is that `W@` puts the state machine in a mood to recognize only the token `g!`, otherwise executing a fail-and-ignore. That would make `W@g!` the Cheer and `W@ g!` the word `W@`, which has no effect, and the word `g!` which does something else.

The Cheer being important, I won't likely do this, since it's harder to understand. Another example is the word `/`, which either discards information up to the next `/` or writes it to a scratch buffer. This is similar to a comment. 

In general, a word is a function followed by sufficient arguments. This is Smalltalk like, so `+ 10 20` not `10 20 +`. `$` refers to the latest value so `10 + $ 20` is equivalent to `+ 10 20`. `$` is the same as `$0`, `$1` is the previous stack member, and so on. Stack values are consumed when the function has sufficient arguments so `10 + $ $` is `+ 10 10`. We don't really want to dup, swap, pop or lock. Nothing underflows, any input a core function doesn't understand causes fail-and-ignore. If the stack is empty, `+ 5 $` will do nothing: when + calls `$`, `$` will fail-ignore, and `+` will fail-ignore (let's call that a fail), losing the `5` in the process.  

This is more consistent behavior than Forth, because Forth words such as `:`, `s"`, and `(` cause the forward-parsing behavior to change, while most Forth words take action on the implicit results of the preceding action. In Orcish, your debug mode is to switch from fail-and-ignore to fail-and-complain. 

It may or may not be harder to read. Orcish is made to be legible, not familiar, and is for communication, not programming. The `$` notation uses an implicit stack with consumption, while the `#` notation uses explicit registers with all the attendant hair. What we provide are tools to manage the complexity of hardware, while guaranteeing that you can send the resulting information in an email, SMS or tweet. 




