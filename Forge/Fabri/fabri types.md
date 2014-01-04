#Fabri Types

I've never written a type system before; I'm starting to realize it's not trivial, no matter how you approach it.

I have a feeling I'll actually understand Hindley-Miller by the time I'm done here. Or Hoon. Something like that. 

The challenge is resolving types when a word returns `mu` or some multicellular `mu`.  For a common example, we routinely do offset arithmetic on addresses. First off, we have to know that we still have an address. That's relatively easy by deciding that `mu` words produce the most specific type possible. The concept of general vs. specific types has to be formalized, of course. 

The tiebreaker rule is that the bottom member of the stack provides the type. So if you say `address typed-value cells+`, you still have an address, even if `typed-value` is of the same specificity. 

What we really want is an idea of a compound type that lives in memory, a struct, basically. So if `funk` is an address and has four cells each alloted for `foo`, `bar` and `baz`, then fetching four cells from the top of `funk` will give a `[[foo]]` on top of the stack, whereas adding four cells width to `funk` will give you `funk@bar` on the stack. 

These can of course be recursively defined, so that `funk` has a `funk` typed field. We can name these whatever we want, `head-funk` and `body-funk` say, and Fabri will figure it out. `-type` means a value is of that type. 

Lastly, and I sincerely don't know what to do here, there's the infamous "floor wax and dessert topping" type. We provide a simple `& | !=` language, braced with `< and $ or | not >` angles. Operator precedence is like Smalltalk, not C, so use extra braces if you're not sure of the meaning. Anything more than simple set intersections, we don't have. 

Fabri aims for an unusual combination: a static type system during compilation that's dynamic in interpretation. Casts are all no-op words, by default: if we want to dynamically track and change our type information, we have two basic options.

The first is to leave Fabri running, at least in part. It should be possible to make the actual Fabri system fairly fast, and to remove the parts that we don't feel that we need. If we just want a couple checkpoints in otherwise static code, this is the way to get it.

The more complete option is to use Fabri to design a type system. Fabri is meant as a compiler tool, among other things, so if we're implementing Lua, say, we're going to build an actual tagged type system based on the existing Lua internals. We use Fabri to build it, then turn off Fabri during testing. In effect we have two type systems that are supposed to do the same thing, and Fabri is our unit test. 
##Implementation

To really make it sing we'll need to provide a second inner loop. Also, we'll need to provide a guarantee that reverse-lookup of a word will function correctly, gForth can't do this consistently with things like deferred words. For Fabri to work, we must get the XT from any word, given the word.

Then we build a second dictionary based on the XT, and compile the type information of each word under the XT in that dictionary. When compiling, we retrieve the assertions and check them against the state. When interpreting, we can choose to do the same thing. 

This will check everything and not run fast at all. We'll have a simple symbol, like `--`, that goes between `\` and `(` and turns off compilation of an annotation on load. Then we can specify a mode that compiles the annotations and the checks into the top of the words that are still expected to have one. 

That will run off the ordinary inner loop, and only slow down for checks that presumably we feel are worth having. We can retain or retrieve an equivalent program, with full typing, using a compiler word directive. 

If someone wants to write some optimizers based on the type information and the code flow, well, I welcome that approach. Optimized code is smaller and runs faster, and Mike Pall has convinced me that dynamic optimization can be the most effective approach to dynamically changing input. 

That's beyond Fabri at this point, which is a superstructure for writing systems tools, not a collection of them. One might write a garbage collector in Fabri, but it won't come with one, nor need it. It's quite Forthright. 