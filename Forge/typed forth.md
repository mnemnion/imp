# Typing Forth

Forth is a powerful local maximum. As usual, I'm interested in pushing past the max.

It's tough, because there's two things I wouldn't touch: the syntax, and the semantics. That doesn't leave much room for playing around. Handle, the proposed successor to labrys, is just Forth assembler-optimized to chop cons cells and spare the return stack on fundamental operators. 

Can we add something like typing without destroying everything good about Forth? I believe we can. 

ANS forth words are documented in a standard fashion as to their function. Examples:


dup ( x -- x x )

emit ( x -- )

swap ( x1 x2 -- x2 x1 )

Though the rabbit hole goes deeper than that. But we might say that dup is `ana`, emit is `cat`, and swap is `meta` -bolic. Those are basic types for verb words; there are more. 

The principle is that those annotations become our compile-time type system. If I say a function is ( a a -- b) then it should be, and the compiler can count to see if it is, sometimes.

This is more like a linter in a sense, because really these are just warnings. Maybe you're wrong about what you want the word to do. 

So we should call it flint! 

Harder to add but useful would be tagging words as of a particular type, then saying ( a foo -- bar ) to say that a word should have two values, consume an arbitrary one and a foo, and produce a bar. These would come in two types: assertion (it's a type because I said it is, forward only) and compile-time tests (it's a type because we can execute a word on it and get a true-false return as to type). 

I like to save parens for commenting out blocks, and there are advantages to line-level annotation. So we're talking about comments that
start with `\ ` and have `--` in them. 

So `: word \ a -- b c`, or something very much like it, would go next to a word that leaves two values on the stack, consuming one. Lines within word definitions can be annotated to, but here we have different meanings: ` >r  \ a b -- c ` means we've pushed c onto the return stack. The stack token `--` must exist or the line is parsed as an ordinary comment, so ` \ a str -- ` means we have three values on the stack, the two cells of a string in correct order on top, and any old cell underneath, and we're storing nothing on the return stack.

This is just good Forth annotation in my book (I've been writing for maybe five days, and reading a lot). Especially low-level words are basically metassembler, and annotations aid readability considerably. Annotations are easy to check against the definition of a word: Execution: ( x -- ) ( R:  -- x ) is the definition for `>r` in ANS, which we could elide to ` \ x -- : x ` or whatever is convenient. Words that leave value on the return stack are uncommon but all the more important to document, as they require a retrieval word within the calling definition.

The way to figure this out is a deep reading of the core words in the ANS forth spec. Which I don't have time for this instant, but am interested in, because I want to write an ANS Forth syntax highlighter that covers most of these bases. 

##Attributes of a Forth word

A Forth word can:

* consume the stack

* change the stack contents

* leave one or more numbers on the stack

* consume input

* produce output

'input' and 'output' may be multiple. 

