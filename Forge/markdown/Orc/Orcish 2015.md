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

Neither of these will happen. We'll use `abcdef`, take the hit, and wish, wistfully, that ASCII was slightly more well designed. 

##Words

Orcs parley through the simplest parser that could possibly work. If they receive a byte that isn't phonemically valid, undefined behavior occurs, which normally includes ignoring you, and may also provoke consternation or even hostility. Given a `numba`, they try to make a number out of it, and remember that number. They will stop if they see a `spaz` or indeed any phoneme which is not a `numba`. How many numbaz they eat before digesting depends on the Orc. I don't know or care what your architectural width is, nor which end it cracks the egg from. Orcish is a language, it's up to the users to understand each other. 

If an Orc can't make a `numba`, it makes a `slang`. This is either one `letta` or `roon` followed by a `spaz`, or it is one `letta` or `roon` followed by a non-spaz phoneme. 

What's a slang? Every other command we need. The shortest slangs, and ones with a `roon` in them, are reserved for the core language, while two-letta slangs or letta-numba slangs are anyone's guess. 

##Handshake

The Orcish Cheer is how Orcs establish a line of communication. 

Orcs in the wild may be presumed to be busy. Their inner loop could be quite tight, leaving little time to poll a pin for data. The Orcish Cheer is emitted at 360 baud, no more, no less. A more normal conversation bandwidth is 9720 or 19440, Orcs have a thing for highly divisible numbers. 

The Cheer consists of the string `W@g!`, which is pronounced `ⱱaaʀχ!`, more or less. The response is `@rk!`, typically, and this is pronounced "Orc!" as one would expect. 

Note that each of these is two slangz long. When an Orc makes a slang, the parser simply resets. Numbaz get eaten to a certain width and then reset also. 'certain width' being as unreliable as it sounds: I neither know nor care what a comfortable way to break up a numba might be, nor the endianity of the architecture. The Orcish language contains only the hexadecimal base, as is proper. 

There is some structure to the madness: the `@` symbol in both cases indicates message passing, and the `!` has an imperative nature when preceded by a letta. 

`W@g!` is the only valid call, while the responses vary. `@rk!` means the Orc is now listening and in the mood for conversation, at the established baud rate. `@rk.` means the Orc is an Orc, or pretending to be, but will most likely ignore you unless you attract his attention in another fashion. `@rk:` is an Orc reporting for duty: it has things to tell you, which come after the `:`, and serve to establish further conveniences. 

###Architectural assumptions

Here we're going to dial out a bit. What is the point of all this?

Simply put: leaving straps on the boot. The original concept of Orcish was closely tied to Forth, and it inherits the philosophy that a computer, no matter how small, should be interactive and capable of being programmed incrementally. 

Orcish is now a language, with dramatically fewer assumptions. Even a burnt ROM can spare enough bits to reply, in broken Orcish, that it has certain abilities, and understand enough Orcish to do things when asked.

The minimum a chip which speaks Orcish might be expected to do is tell you what it can do, and do it. An actual Orc will contain an interpreter, which may be moderately sophisticated, or may not be, but will suffice to operate the Orc, inclusive of programming it with new behavior.

###Syntax

I would like to thank Alan Kay and Chuck Moore for their relentlessness in the pursuit of the good.

Orcish is an agglutinative language. The parser is a state machine, each word puts it into a new state and each return falls out to the same starting place. This allows us to make very few assumptions, as we have combinatoric richness available. 

To illustrate, `W@g!` could be defined as precisely the same as `W@ g!` and I might choose to do so. Equally possible is that `W@` puts the state machine in a mood to recognize only the token `g!`, otherwise executing a fail-and-ignore. That would make `W@g!` the Cheer and `W@ g!` the word `W@`, which has no effect, and the word `g!` which does something else.

The Cheer being important, I won't likely do this, since it's harder to understand. Another example is the word `\`, which either discards information up to the next `\` or writes it to a scratch buffer. This is similar to a comment. 

In general, a word is a function followed by sufficient arguments. This is Smalltalk like, so `+ 10 20` not `10 20 +`. Smalltalk provides the ability to define simple infix operators, a user convenience we have no need for. `$` refers to the latest value so `10 + $ 20` is equivalent to `+ 10 20`. `$` is the same as `$0`, `$1` is the previous stack member, and so on. Stack values are consumed when the function has sufficient arguments so `10 + $ $` is `+ 10 10`. We don't really want to dup, swap, pop or lock. Nothing underflows, any input a core function doesn't understand causes fail-and-ignore. If the stack is empty, `+ 5 $` will do nothing: when + calls `$`, `$` will fail-ignore, and `+` will fail-ignore (let's call that a fail), losing the `5` in the process. 

A word may take an optional argument, in a limited way. The recognizer has no backtracking, but it may be called on the latest slang without taking another bite. So `fu br` could eat the `br`, decide not to use it, and call the recognizer on `br`. By default, failure to parse loses the information, and exactly two bytes may be handed back to the global context. 

A more advanced sequence: `+ 10 5 * $ 6 / $ 10` translates into infix as `(10 + 5) * 6 / 10`. Whether translated to a stack or straight to registers, this is very short code. 

This is more consistent behavior than Forth, because Forth words such as `:`, `s"`, and `(` cause the forward-parsing behavior to change, while most Forth words take action on the implicit results of the preceding action. In Orcish, your debug mode is to switch from fail-and-ignore to fail-and-complain, and we are always parsing forward. 

It may or may not be harder to read. Orcish is made to be legible, not familiar, and is for communication, not programming. The `$` notation uses an implicit stack with consumption, while the `#` notation uses explicit registers with all the attendant hair. What we provide are tools to manage the complexity of hardware, while guaranteeing that you can send the resulting information in an email, SMS or tweet. 

The sometimes-optional nature of whitespace may be somewhat unnerving, but shouldn't be. Well-spoken and written Orcish will include semantic whitespaces as a matter of course, there being such little harm in it. A spaz must follow any one-byte word, which is in effect a two-byte word where the second byte is either space or newline, but edge cases must work correctly: a valid letta, followed by a byte that isn't valid, followed in turn by a space, must parse as the single-letta word. So `D✣✣␠` drops the Unicode and becomes D-spaz. 

There is important resilience in this forward-only semantics. Forth words don't break silently, if they did, they'd leave the stack in a weird state. Orcish words do break silently, and they consume anything they were trying to parse. This probably leaves us in a bad way anyhow, unless the default action is to go back to work, which it often is. A function is both dependant upon and responsible for the input stream it consumes. 

## Semantics

###Self Report

Self report is the vital ability of an Orc. If it can't provide at least some behavior, there's little point in knowing the Cheer. 

This can of course be arbitrarily complex. The simplest case is an Orc programmed in C. It has functions, they expect the chip to be in a certain state (arguments). Such an Orc should be able to tell the user what its functions 'are', and the arguments they expect. 

Not so much 'what' as 'where': the minimal report is a list of addresses and function signatures that tells us that calling those addresses with those values might do something meaningful. This is useful since Orcs also know how to report their identity as a matter of habit, so if you can find a code repo you're pretty good to go. 

Other reports include contents of memory, registers, flash, EEPROM, the ability to ask for periodic polls from a pin, the usual kind of helpful commands. At this level, Orcish functions similarly to [bitlash](http://bitlash.net/), and has a similar memory footprint. 

###Assembly

Orcish will provide a syntax for assemblers. It should be relatively straightforward to write Orcish assembler as a C target, without much hairpulling or pain, and we should be able to fit such an assembler in a K or so. The bootloader for an Orc is nothing but the relevant USB functions and an incremental assembler, code is loaded as ASCII regardless of dialect. 

We really want the C compiler to follow certain conventions, so that we can add functions a bit at a time. This might take some hacking but it's worthwhile for ARM and Atmel at least to have this nailed down. Something like each function containing a cell that has a back reference to the prior function definition, so we can walk back in memory and and retrieve all function pointers and signatures with compact code. The signatures will also be needed, I realize I know but little about the assemblage and linkage of C. 

###Message Passing

When Orcs need to share large volumes of data, they do so directly. When being more interrogatory or imperative, they habitually switch to Orcish. This is a blessing for some poor sod, somewhere, reading off a logic probe. 

Many Orcs will therefore contain functions wherein they Cheer at another Orc and tell it something useful. The language contains imperative primitives commanding an Orc to do this. As a result, it's normally possible to talk to an Orc several pins away from anything you can reach. If you can talk to it, and it's not truly dull, you can program it as well. 

###Interactivity

Non-trivial Orcs will have a useful interpretation loop, with functions that make it moderately pleasant to program. It will be possible to incrementally compile these interpretations in a Forth-like manner. This mode will be cryptic, certainly, but writing and reading it using long-form expansion of the slangs should be at least useable. Early attempts at Orcish focused on this aspect, which I no longer feel is as important. 


##Details

### $ and #

`$` refers to TOS at any given time, while `#` refers to the prime register. These are almost always equivalent or can be made so, and this may be a made a semantic requirement. `$0` is equivalent to `$`, while `#0` more likely is the literal R0, likely not to be equivalent to `#`. 

Assuming there are more than four slots on the stack is unwise unless one has good reason. Stack juggling is not a part of the core language. 

###Strings

The `"` roon causes immediate string mode. The only escapes are `\"` which translates to `"` and `\\` which translates to `\`. It is terminated by a non-escaped `";`. 32 bytes is a virtual guarantee, many Orcs will be able to take a lengthy string. Any bytes within this parse space are valid, meaning you can use utf-8 and nulls don't terminate. 

###Quotations

`(` begins a quotation. As with most single-byte slangs (`"` notable as an exception) it has a mandatory spaz that follows. Up to the next `)`, the quotation builds a series of calls and places an execution pointer in `$`. Note that `x $`, which would immediately execute the quotation, will also take the execution pointer off the stack, so references to `$` in the quotation, if they exist, will be to the value that was `$1` before the execution of `x $`. 

###Definitions 

`:` causes the beginning of a definition, and always has a space. The slang following is defined by the input stream until a concluding `:;`, and is immediately available for recursive calling. 

An attempt to redefine a slang fails. As a result, the following input stream would be interpreted, not compiled, which means this is often fail-and-complain. This is why the concluding slang is `:;`, not `:`. It prevents figure-ground reversal, which is bad. You'll note we do the same thing with strings. 

The bare word `;` compiles a return, used in control structures. `:;` is a return also. These returns are conceptual, it's okay to turn them into jumps when possible. 

###Comments

Mentioned before, anything between two `\` is a comment. When Orcs reply to a command, they often do so within a comment. Orcs frequently discuss various subjects and this lets them be chatty without compelling signficant computation. It's perfectly reasonable to write a comment to scratch and do something with the information, though if an Orc feels strongly that you should act on data it will normally provide it as a command. 

About commands: Orcish is a message passing architecture, Orcs ignore anything they don't care to act on. Optionally they may interpret your attempts to boss them around as hostility and act accordingly. Language, not programming language. Programming a docile Orc is generally feasible and even pleasant. 


| S | y | m | b | o | l |
|---|---|---|---|---|---|
| A |   | P |   | k |   |
| B |   | Q |   | l |   |
| C |   | R |   | m |   |
| D |   | S |   | n |   |
| E |   | T |   | o |   |
| F |   | U |   | p |   |
| G |   | V |   | q |   |
| H |   | W |   | r |   |
| I |   | X |   | s |   |
| J |   | Y |   | t |   |
| K |   | Z |   | u |   |
| L |   | g |   | v |   |
| M |   | h |   | w |   |
| N |   | i |   | x |   |
| O |   | j |   | y |   |
|   |   |   |   | z |   |

| Rune | Name | Notes | Rune | Name | Notes | Rune | Name | Notes |
|------|------|-------|------|------|-------|------|------|-------|
| ~    |      |       | `    |      |       | !    |      |       |
| @    |  At    | Message      | #    |      |       | $    |      |       |
| ^    |      |       | &    |      |       | *    |      |       |
| (    |      |       | )    |      |       | -    |      |       |
| _    |      |       | +    |      |       | =    |      |       |
| {    |      |       | }    |      |       | [    |      |       |
| ]    |      |       | \    |      |       | |    |      |       |
| ;    |      |       | :    |      |       | '    |      |       |
| "    |      |       | <    |      |       | >    |      |       |
| ,    |      |       | .    |      |       | /    |      |       |
| ?    |      |       |      |      |       |      |      |       |





