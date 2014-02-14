# Forth 2 and Forth 1

In the Lisp community, we are cursed with a holy war. Either there is a single namespace for symbols or there is not; Lisps admit no middle ground. 

This is that worst of things, a syntactic struggle. In the phrased `(foo bar baz)` either `foo` is special, as the Common would have it, or as Schemers and Clojurians prefer, it is not. 

The Forth umbrella is not united on syntax. There are dialects which parse within a word. The unity is near-total by comparison, and not particularly provocative. In general, a word has whitespace around it, and that is that; `word, number, fail` is also practically a fact of nature. 

Forth is sharply divided on a different question: should the interpreter be modal, or stateless? ANS Forth and friends have an interpret/compile distinction that is always active. 

This reminds us of the notion that Forth is a program, not a programming language per se. The differences of opinion concern the semantics of the runtime environment. This is more like vim and emacs than it is like Common Lisp and Scheme. 

Chuck Moore, the designer of Forth, decided that the interpret/compile distinction was a big mistake. ANS Forth was and is locked in. There are cases to be made for both; I'm coming around to the conclusion that Chuck's right about this one.

Chuck Moore facts: Chuck wanted his own chip, so he wrote an editor (in Forth) to write a language (a Forth) to specify a chip that runs Forth. These are like the [other Chuck facts](http://www.chucknorrisfacts.com/) except for being true and not about a garden-variety actor best known for being whupped by Bruce Lee. Which is an honor; but I digress.

## There are never two modes.

At first, this seems like the right idea. That's why it happened first. In interpret mode, you do words, one at a time. One of those words turns on the compiler. In compile mode, you compile the words instead of doing them. 

Great. What if you need to interpret during compile time? Well here are some options: `: interpret-word do-stuff ; immediate` gives you a word that interprets at compile time. What if you want to compile at interpret time? Well, yeah, you can: ` ' a-word compile, ` will cause the execution token to be compiled at that location. This is not useless, you can do weird things like `[` which, during compile time, turns off the compiler. So ` : adder [ ' + compile, ] ; ` will have the effect of making `adder` look like `: adder + ;`. 

Okay. What if you want to compile an interpreted behavior for execution? Er, what now? Well, an immediate word does stuff during compile time, so you might need to, say, chew up a few words and do things to them. That's what `postpone` is for. Look it up because lord knows this gets confusing. 