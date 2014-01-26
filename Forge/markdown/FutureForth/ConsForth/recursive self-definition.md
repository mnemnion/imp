#Recursive Self-Definition

The execution model of ConsForth is based on two developments: inferential typing and JIT compilation.

The intention is that only the very system primitives will be written in a likely language: Rust and x86 ASM are good candidates, by the time we get to actual implementation. Or we can always just grunt our way through the ANSI C.

Even the primitives will have higher-level Forth translations that do the same thing. Why? Consider it a unit test across the entire language, and a way of specifying every possible stack effect in both Forth and Fabri. It should be possible to replace a random number of the primitives with their expansions and get the same result, presuming a function that has no time component. 

The purpose is mostly to have somewhere to put the primitive annotations that isn't a "code" word. That way, when targeting new architecture, we can take the annotations off the high-level words and use them to check the integrity of the code words as we design them. 

Another reason is the word `see`. I feel it should never drop down into the primitive level. [Perverse Forth](http://www.retroprogramming.com/2009/07/perverse-code-deviant-forth.html) demonstrates that with a number parser, `-`, `>r` `r@` and `r>` one may in effect implement Brainfuck to define the entirety of Forth. There are some minimal number of words which should return simply "axiom" and the stack effect; `dup` should probably be so defined, rather than as `: DUP >R R@ R> ;`. Other than the words referenced in Perverse Forth, there is no reason to limit ourselves. `*` in pure Forth is instructive, and I've heard of chips that don't multiply. Mostly from old Forth hands. 


##Execution Model

The execution model for ConsForth will be strictly indirect-threaded, for clarity in the user and runtime environment. While we may stick a few optimized words in, early on, the goal is to get a JIT running that will perform what we want.

In some circumstances, JITing is provably faster than static optimization. In particular, cases where the input to be handled is unpredictable: A JIT can generate an optimized loop for each situation where static analysis must simply guess. 

Combining JITting with a language with no inherent memory safety will make it even faster. Having an untyped language would take away this advantage and more: by having interactive annotations available, we can get a lot further. 

In general, I consider optimization that isn't performed invisibly at runtime to be premature in almost all cases. The exceptions are worth writing once and for all as kernel level abstractions. 

Nothing at all is worth breaking introspection and referential transparency. Optimizations are of course not easy to follow or debug, compared to pure Forth code, but if your problem goes away when you turn off the optimizer, there is no other option, and we will do what we can. 

In the meantime, we want to start with a slow, comprehensible core, and unroll it with caution from the inside out. There's no sense in even hand-optimizing without a profiler, probably type aware.