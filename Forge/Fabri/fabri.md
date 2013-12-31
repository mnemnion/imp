# Fabri

Fabri is an extension of Forth, designed for exploratory systems programming.

That is an unusual combination, and the raison d'etre. 

##History

C beat Forth, fair and square. Unlike with Lisp, there were no shenanigans: Forth legitimately shot first, C brought more firepower to the table. Why? Well, because C has struct and typedef, and Forth does not. 

It wasn't 'syntax'. Forth genuinely has no problem with infix notation. You don't need 'reader macros', you simply define a parsing word (called a recognizer) and use it for awhile, then recognize a word that puts you back into interpretive mode. If you're not doing low level stack and number manipulation, 'postfix notation' merely means that the computer reads and executes your commands in order. That's not exactly unfamiliar territory. 

Forth was the official language of astronomy for years. I guarantee you it's suitable for writing mathematically oriented software. 

Struct, on the other hand, is a somewhat unnatural formalism. You can use `create` and `does>` to do more or less anything you want, but without any resulting safety, the structure is awkward to use, and it's often more idiomatic to do something else instead. 

Typedef is where Forth falls down and doesn't get up. When doing systems programming, sometimes you need a type, and need to track and enforce it. I say 'need' advisedly, because the evidence appears to show that without this, systems of a certain type of complexity simply do not get written. 

Despite C being completely static, weird, and frustrating to work with, it was easier to build pseudointeractivity into C (debuggers) than it was to build pseudotyping into Forth. Mostly because adding type syntax to Forth ruins it completely. It's not that it hasn't been tried before. 

There is another factor, namely that Algol, as the name suggests, was designed around the informal language for specifying algorithms which mathematicians came up with in the early days of computer science. That is, back when machine code was your only option, mathematicians worked out a clear language for communicating with each other, and with themselves, about the problem they were compiling a solution to. 

That's not syntax, per se, it is hermetics: the art of combining such disciplines as hermeneutics, mathematics, syntax, and semantics, into a coherent whole. When the only goal was clarity, we got proto-Algol. 

No one should be surprised that a semi-formal language based on this informal language became prevalent. The problem is that we've never made a physical computer which runs Algol, even though we probably could, so we lose introspection. Hence, we have no idea what the computer is actually doing without monumental effort: hence, our state of affairs in the early teens. 

This makes C unsuitable for exploratory programming of any sort, including systems. I define 'systems programming' loosely as designing tools for other programs to use: this is normally heavily concerned with type, and other interface definitions that Forth developed without needing. 

Again, I say unsuitable on the basis of evidence: everyone uses something else written in C to do their exploratory programming, unless they have to use C. Then, they grit their teeth, and dream of Rust 1.0.

This is because Forth code composes in a very particular way, if left to its own devices. It is not trivial to bridge that interface and add other approaches to the same hardware. When you do it the other way, running Forth in say Unix, you either have to import the Unix way of doing things, losing many advantages, or simulate the Forth way, losing your Unix privileges in the process.

##Introduction

Fabri is designed to add typing to Forth in a user-driven, annotative way. Let's look at some Forth code with Fabri annotations.

There is no Fabri engine yet, which is half of why we're writing this.

```forth

: juggle                \ ( a.noun d.noun [b c].cell -> noun )
		swap			\ ( a [b c] d    --    )
		>r 				\ ( a [b c]      --  d )
		over            \ ( a [b c] a    --  d )
		-rot            \ ( a a [b c]    --  d )
		chip            \ ( a [a b c]    --  d )
		swap            \ ( [n] a        --  d )
		r>              \ ( [n] a d      --    )
 	    chip            \ ( [n] [a d]    --    )
        cons            \ ( [[n] [a d]]  --    )
    ;

```

This is taken from Labrys, my Ax machine in Forth. I've changed the annotations to reflect my current thinking on Fabri.

The lefthand column is ordinary Forth. It defines a word, `juggle`, that performs certain stack manipulations to carry out one of the Nock reductions. 

The righthand column is a mockup of Fabri code. The first line is a word assertion, the rest are line assertions. 

Because there are no primary assertions, all of this Fabri could in principle be automatically generated. There is a line of Fabri for every line of Forth, and Forge normally takes care of this. 

When I say there are no primary assertions, I mean that the types of the beginning and end state are already known to the system, as are the effects of all the words used. That means the effect of `juggle` can be inferred from the effects of its composing words. The only thing we do in the word assertion is give names to the values. The only thing we'd have to do by hand is rename `[a b c]` to `[n]`, and only if we want to for clarity. 

This also shows an ordinary, interactive use of Fabri. The generated version of the second `swap` woudl contain `[a b c] a`, but we changed it, and Fabri is kind enough to track that change. Note that the names of values do not come typed unless a type has been asserted for that name. 

As it happens, `cons` is a defining word for `noun`, that is, the product of `cons` is noun by definition. So our word assertion, that we will produce one noun and put it on the stack, checks out. You can take my word for it that the other assertions also work, or read the Labrys source. 

Here's a very simple primary assertion:

```forth

: is-a-foo \ ( mu -> := foo )
	noop   \ ( := foo )
;

```

This literally does nothing but declare the top stack variable to be of type Foo. That is, Forth will completely ignore this word. If you're running full-bore Fabri, it will in fact do nothing for long enough to let Fabri update the annotations, then continue. Otherwise, this will be compiled out of the word. 

`mu` means the contents of the top of stack is unknown, but that there is something there. `:=` means the word has the effect of declaring a `foo`. `mu` is an untyping word: the only thing `mu` can't be is `nil`, which is not a type, but a word indicating that nothing has been placed on the stack. `( something -> )` is harder to read than `( something -> nil )` so we use the latter to indicate a word that only consumes stack.

An aside: I feel as though the Lispian equation of `nil` and zero is profoundly misguided and a source of error. Stacks can be in a nil state because they're indirected and can have a genuine void. A box may be empty, or full; a stack may be clear or stacked. We call the former `nil` if we feel the need to indicate it at all. Fabri is namespaced separately from the Forth it annotates, so if you're writing a Lisp, or indeed anything else, this will pose no dilemma.

That's pretty much all that Fabri does. Notably, it does not natively provide the Forth with a way of using the type information. If you want a type check, you should put some type information somewhere, because that's what you do in systems programming. 

Remarkably often, it's possible to forgo this. That is afterall the premise of C: by typing in advance, you don't need the types in your actual code. Since Fabri is always on when you're designing code, there's a lot of cleverness you can get away with. Of course, if you need type *safety* you need something more than just types, so you should build that, cousin. 

There is a syntax for words that have the effect of typing a value. Two options: `\ ( foo? -> := <foo|false> )` means that a possible `foo` is on the stack, and the word either returns a `foo` or your `false` token, which must be concretely not a foo. We need another word word that can distinguish `foo` from `false`, or we cannot check this premise. `\ ( foo?! -> foo )` means you take a noun, and you either return it if it's a `foo`, or you return a `foo` anyway. 

Fabri is for *exploratory* systems programming. If you want to change your type system, change it. Your code won't break, but Fabri will; fix both sides until it works again. 

The idea is to have a stable tripod: Forth, Fabri, and Forge. Forth allows you to tell your computer exactly what you want it to do. Fabri lets you use the computer to track the significance of those actions. Forge allows you to explore your codebase and the execution conditions created by your status and dynamic changes. Action, Premise, Result. 

You can actually write the Fabri first, that would be test-driven design. But you probably won't want to, given Forge's ability to automate Fabri on an ongoing, inferential basis. When composing a word, if you know your premise and result, put it in there. When you type `;`, it'll either check or it won't.

With Forge, I intend that even the normal systems programmer won't have to write more than a single line per colon word, if that.

##Using the Forth

Fabri will begin with and build on ordinary ANS Forth. We intend on using PForth as a platform for license and philosophical reasons.

Eventually, as is the nature of the Forth program, we will be using a customized dialect. It will retain the ability to read and annotate ordinary ANS Forth code as a compatibility layer: the main changes will be the treatment of the dictionary and parsing words, which will have a bit of additional support in the syntax. 

These are mild changes, revolving around the desire to use a token other than `bl` to separately tokenize what classic Forth would recognize as a single word. With dictionaries we intend to provide this in the ordinary interpreter, so that ordinary module language as found everywhere else will work. Probably using `foo.:bar`, just because I like it.: easy to read, not as crowded as `::`, and a single dot should be a user glyph. 

This would strictly break compatibility, ANS requires the ordinary interpreter to parse `foo.:bar` as one word, which isn't what we want. 

Fabri won't have an ordinary interpreter: it ships with two, at first, one for the annotations and one for the Forth code. The intention is that a different parser will become the standard one, with ANS offered as a word that shifts the parser to that standard. 

##Interactive Optimization

Another reason C won is that GCC focused impressively on optimization. Optimized Forths were available commercially, while freeware Forths tended to suffer from the scorn of the professional community. Important word there: the Free Software Foundation was essentially subsidized by the academy, leaving them free to gift their software and create the open source ecosystem. Good for us, but Forth was developed and used by salt of the earth computer programmers, who called themselves "users" and expected to get paid for their hard work. Believe it or not, writing a programming language environment and then selling it used to be a respectable way to earn a living. 

It would make me sad to try and sell a programming language. It will also make me sad if I go broke after writing one. Never seen that happen though, if the language is worth anything. 

My toolchain isn't in much of a hurry to bring that power to the table, but it must be done. What I hope to provide is interactive optimization. That is, the ordinary FabForth engine will provide, deliberately, naive threaded interpreted code. Fast, but not as fast as it could be, almost by definition. 

However, when you follow it with your stepper, it will do exactly what you expect, every time. The Fabri annotations will be identically naive, and hence, easy to correlate. 

If you need faster, the old school way is to write it yourself, in assembler. You can check the result against your existing code, after all, and Fabri can only assist here. 

That's handicraft, and handicraft is always best. Not always fastest, but *always* best, given a skilled craftswoman. 

Oh, about that: in Fabri the user's pronoun is always female. The code or computer is sometimes male. That's just how we do it. 

What we do intend to offer is interactive optimization. Clearly, you can turn interactivity off, but you can't just go ahead and add it later. What Forge will do is help you profile code, offer to rewrite hotspots for you, and show you the resulting work. 

JITting is cool, kinda, but it's a tough commitment to go back on, and we should add it only if we still need it. Something tells me we will, but only when adding dynamic script-class languages to the ecosystem. These are often deliberately limited in their power, making it impossible for the composition of actions to be efficient in and of itself. JITting is the proper response here. 

For ordinary systems programming, interactive optimization promises enormous rewards in reliability and comprehensibility of the codebase. Fabri can do really cool stuff, like keep a running count of to-the-tick instruction counts, and which values serve to multiply or otherwise change that value.