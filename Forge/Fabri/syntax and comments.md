#Fabri Syntax

Fabri is a superstructure over Forth. We intend to support ANS, rather than favor it as the be-all semantics for a Forth environment. Uruk doesn't care about ANS much, Fabri considers it another dialect which should be spoken excellently. 

What we're working off are two humble tokens in Forth `\` and `(`. As long as the former meets an `\n` and the latter encounters a `)`, Forth is normally not concerned with the intermediate contents.

We intend on using these in a way that resembles existing practice, improving on it where desireable. Since machines do not interpret comments, we have more flexibility in designing comments to be interpreted by machine. 


##Premise

In essence, Fabri is a two-column programming language. The left column, Forth, is dynamic; the right column, Fabri, is static. Keep in mind Forth is the true Jedi way, and you may have the connotations of those words backwards: dynamic is 'concerned with movement' and static is 'concerned with state'. You did take physics? Excellent.

Forth, being exclusively concerned with movement, is the most dynamic language in existence. If you want state, you must provide it. Let us proceed to do so. 

An ordinary Forth stack annotation looks like this: `( a -- a a )`, which is as good as a definition for `dup`. We write this slightly differently: `\ ( a -> a a )`, followed by a newline or an actual comment, then a newline. Note that as usual in Forth, you can add spaces, but cannot take them away. 

Why the double comment? Some minimal protection against reading existing comments in Forth code as Fabri annotations. It might happen, but is easy to fix. Also, it's not a comment, it's an annotation. Last, it's important that annotations not end up in between Forth blocks on a single line, and using `\` prevents that.

Here's how we'll annotate `: r>`: `\ ( a -- nil -> nil -- a )`. `nil` is not required, but is illustrative and allowed. The other side of `--` is the return stack, and the TOS is the next token. It's the mirror token, and works like this: ` 1 2 3 -- 4 -> 1 -- 2 3 4 `, which is easy to read. 


### \ ; ephemera

A `\ ;` token pair (or `( ; `) will make the comment ephemeral. That means a decent editor won't normally show it, and if you 'delete' it, that buries it deeper. Anyone who read `/.` will be comfortable here: your comment may acquire bad karma but it won't go away without special action. I'm not sure what that will look like in actual source, but Fabri code has no hidden information. An editor such as Forge can and will hide info, but Fabri is encoded in ordinary ASCII as God intended. Well, maybe UTF-8; Forth doesn't care how you provide bytes for a word, just that you do so. 

Ordinary comments are ordinary.


### Line annotations

Line annotations simply show effect. Generally, they are calculated. They follow a line and begin with `\ (`. Both Forth comments must be closed. A line-annotated use of `r>`: `\ ( a b -- c )`, given a previous line annotation of `a b c --`.


### Newlines

I propose, here, something I know will be controversial. I believe it is correct.

A comment is associated with the line below, unless there are more newlines below than above, in which case, it as associated with the line above. 

Try it. Bet you'll like it. 


### Trace Stack

A colon annotation is a promise; a line annotation is a premise. From the promises, the correct premise may be calculated, and new promises introduced. 

The simplest way to do this is to have a pair of shadow stacks used during interpretation / compilation. 

When a word is called, the return token goes on the return stack. Fabri checks the premise, pushes the promise, and when the return token gets popped, Fabri checks that the promise was fulfilled. The premise and promise were compiled into tokens convenient to the recognizer. 

During word execution, Fabri updates the ghost stacks accordingly for each annotation it has. Which is all of them. 

This is not fast, but fast is rarely a priority when you're writing a program. Things like fulfilling your own expectations are more important. It should be relatively simple to transfer pieces of the mechanism to the main stack, so that this isn't an all-or-nothing proposition.

Actually, it sounds like Fabri could just `dup` the top of both stacks and make sure they taste right. For assertions, it would need to store an assertion token and track that in parallel, so having another stack will come in handy. 


### Assertion

Everything processed by Fabri is an assertion, that is, a statement of fact. Everything processed by Forth is a command. 

The language of assertion needs to be developed with practice. It needs to be easy to read, which should translate to easy to type; the annotations are there for the user and the program.

I'm slowly working on a spreadsheet of the ANS Forth words. It feels unnervingly like homework, but I will complete it, and that will be where most of Fabri gets written. If I can annotate Forth itself, by definition, I can annotate anything written with it.

Forth comes pre-annotated! I love the taste of low hanging fruit. 

I like the look of `:=` as an assertion. So ` \ ( -> := foo ) ` would mean that this word leaves a foo on the stack, by definition.

Every word in the dictionary ends up with an assertion token, which is a nice comfortable one cell wide, of which we'll use 32 bits to allow for cruiser-class architectures.  

### Conditional Assertion

Sometimes a word provides a test: the thing on the stack is either a type, or not.

We have a conditional assertion for the premise side of the transforma, like this: ` \ ( foo? -> [:= foo] | false )`. This also shows that Fabri isn't stuck with Forthian whitespace rules, or syntax conventions.

If we have a conditional assertion, we can reuse it if we want to know whether or not a foo? is a foo indeed.

### Forward Inference

In Forth, backwards anything is a bad idea. Sometimes you need it, and there are mechanisms like `defer`. 

Fabri will be capable of forward inference: once it knows something about a word, it will hang on to that information, and keep checking to make sure things are sensible. 

The rule in Fabri is simple: if you put it on one line, you wanted an annotation at the end of that line. Our Forth flows very much down, we don't directly employ the block technique and encourage clarity and chatty behavior. The editor can and does hide anything extraneous to the moment. 

If you have something special to say, or just want to, you can write the annotation yourself. Otherwise you'll get one anyway. 

Every word in the core is annotated, and the only other words that Fabri will refuse to compile without annotation are words with `code` in them. The type system is optional, but we like to know when we're violating it and when we aren't. "cell(s) --> cells(s)" is fine; that's a known unknown, and we can propagate those. 

Fabri complaining is utterly different from Fabri crashing. There's no 'linting' going on here because Forth has no lint, so when Fabri complains and things still work, that's worth fixing early rather than turning off the alarm. Normally this is finding an annotation and turning it into a type assertion. 


### Backward Resolution

It would be nice to turn Fabri off, but not completely. This would require real sophistication, if one didn't want to do it by hand. This is because a lot of typing is about assertion, and assertions against literal data can't be checked off a key-value data structure. You have to check against the address that put them on the stack, which is fortunately sitting on the return stack where you can get at it. 

See, sometimes a character plus a byte is an offset, sometimes it's a character. That's an assertion. If you want to keep that guarantee for whatever reason, somewhere later in your code, you have to find the place it gets generated and record it, or there's nothing to check. 

This does require some backwards reasoning, but hopefully not in a way that will confuse and frustrate the user. 

This matters quite a bit for writing other programming languages, which is something Fabri should be excellent for, as a design goal. 


### I think I accidentally a Prolog

It occurs to me that in some sense, what we have is a logical language next to a procedural one.

The Fabri statements are either true or false, depending on the truth value of the dependent statements.

The Forth code does something. It may be what you want, it may not.

Fabri can help you figure that out.

Prolog sucks for almost everyone to program in, but then, this is true of Forth also. For Prolog, it's because until your program is true, it's false, and most people do not have the fortitude to wander through a wilderness of falsehood on the way to truth. In Forth, it's because everyone has been confusing the program Forth for a programming language. The confusion is understandable. 

Forth is Tao. Fabri is Te. The Te of Forth has been an inner Te; we will take this poetry and give it wings.


### Attitude

Fabri is only strict when you ask it to be. We're not Prolog because we have a 'maybe': Fabri can tell the difference between "yes, that's typed", "no, that's not typed", and "sure, might work, don't have the information to be sure". There may be more shades, but what would we do with them? The Basic Principle.

We have trulians: true, false, and mu. Everything begins with a mu token. In principle, mu & true and mu & false are different values: to me, mu & true means "could be true, would be surprised if false". In practice, it is probably clearer to use, shall we say, first order mu resolution: mu & bool is bool. 

Three and five both have the proper structure; they call them 'odds' for a reason. 5 is more nuanced and nuance is a superpower that we should add later if we need it. 

The attitude toward the codebase is that Forth code should be marked up. Forth is not a language, Forth is a program: if concatenation you like good else not then ; if you know what I mean. 

We are building tools for fundamental system services: data structures, algorithms to work with them, including syntax recognition and transformation, etc. 

The idea is that we can build existing languages from the bottom up: implement their core data structures, define words to manipulate them, write a grammar / parser / environment, and go. If we do it in that direction, the languages can share structure: if we do what the C layer does and optimize them into incompatible doohickeys, incompatible doohickeys is what we'll have. 

A thesis here is that such languages will interoperate better, because they will decompose (literally) into a common language and runtime, with the components crafted around the problem space. 

This will also encourage elegant languages to be implemented first. Implementing Lua, or indeed Oberon, would be a pleasure; implementing JS is, as usual, a tedious lesson in the limitations of that language. 

Elegant languages have civilized codebases. I don't want to be able to port something called Grunt unless it knows how to fire a weapon.


###Homonyms 

Fabri makes possible something Forth notably lacks, namely, homonyms: words which are different in meaning depending on context. 

This is a dangerous power, in the wrong hands. It is power, in other words. The reason it is dangerous is that Fabri is scaffolding, until we use it to resolve homonyms. Then it becomes infrastructure: we have changed Forth. Which is of course allowed, encouraged, inevitable: but to be done carefully. 


### Dialects

Forth has an unnerving habit of using the same words as the rest of computer science in different, better ways. Fabri aims to continue that.

Fabri will support dialects of itself as a first class citizen. They are called this to emphasize the inevitable mutual incompatibility. A dialect is a parser and a dictionary, nothing more, nothing less. Both our static and dynamic parser may have dialects.

It is suspected that what we are calling "dialects" here will turn out to be what is otherwise called a "language". 


