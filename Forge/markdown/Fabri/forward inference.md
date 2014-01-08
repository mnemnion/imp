#Fabri Forward Inference

The Fabri type inference algorithm is designed to be quite simple, at least conceptually.

In generl, if an algorithm is simple, there are ways to make it clever. If it starts clever, there's no hope. Forth dictionaries can be optimized into whatever you want, as long as they work the same way: a dictionary could be a hash bucket as long as we have a 'global hyperstatic' environment. 

In principle, probably in practice, Fabri uses a different set of codewords. In an indirect-threaded Forth, every word has a codeword at the start of the data, which either jumps an instruction forward for a 'direct' word or calls the addresses one at a time after pushing them onto the return stack, for an indirect word. 

Fabri's handlers will retrieve the assertion token first and put it on the assert stack, caching the stack effect number underneath it. The assertion token jumps directly to the assert handler, which is triggered by special jump versions of `next` and `exit`. 

This is how we handle annotation level: Fabri compiles fast-threaeded (aka ordinary) words for anything below the annotation threshold. You're supposed to push annotations down when you're confident of them. Fabri still uses the annotations when interpreting, to do inference. 

##Inference

Fabri needs to be able to generate annotations or it's going to kinda suck. Keeping track of stack diagrams is nice and all, but a typing system without type inference is a sad thing. 

I know enough about ML and friends to know I don't want those kinds of restrictions in my life. I'm pretty sure Forth and Fabri, together, can give that "I build my type system and as soon as there weren't errors it Just Worked" feeling. But before you get there, you'll have broken code with annotations that complain at you. 

My least favorite code moments are when I don't know what to type next. Static type systems are normally responsible. 

I consider backwards inference hard to reason about and unForthlike. Fabri will only have access to the annotation directory, which gets filled up during compilation time: there's no room to go backwards and put new type information in there. 

We also offer no overloading or polymorphism of any sort. We're building *tools* for overloading and polymorphism, providing them directly would be counterproductive. If a language has no garbage collector, you can write a garbage collector in it. If it does have a GC, you can only write a GC *for* it; if it's a solid language, you can at least use it for that purpose. 

So if you're implementing one of those dumb languages where `+` adds numbers and concatenates strings, and you know at compile time which kind of variable you have, you can compile in the appropriate word. 

I'm going to assume a separate STATE for interpreting and compiling, though FreeForth doesn't have these, and I can see building on FreeForth being a lovely thing, though it may make an ANS compatibility layer hard.

###Mechanism

In interpretative mode Fabri simply checks the annotation of the word you've typed and updates the annotation stack(s) accordingly, complaining if you've violated some premise. 

In compile mode, Fabri reaches in and infers a new annotation for each *line* that you type. This is the only way to skip annotation completely. For a variety of reasons, our Forth flows strongly *down*, leaving plenty of room for documentation and annotation. For systems programming, I estimate the correct ratio of actual Forth code to annotation, documentation etc. to be around 1:10. The only way we're going to oust C is by doing a better job. 

Fabri always produces an annotation. If you want to change it, change it: Forge will be designed around this workflow. 

Here's our type system, in order of precedence:

mu

cell

flag true false

literals: 

u64
s64
u32
s32
u16
s16
ub
sb
char

literal floats: 

fp32
fp64

adr


...I think that's the whole shebang. All other types are based on these. Assemblers have never heard of array-specific operations, and even if that changes, we optimize specific compound types. Any weird types your processor has, you can use if you need them.

This is all very easy until you go using `+` on two different types. I'm deeply tempted to use maybe logic, but it complicates everything. The basic rule is that general types always lose to specific types, and if two types are of the same general type, the type on the **bottom** of the stack wins. An operation on a boolean produces `false` for zero and `true` for anything else; the literal value is of course untouched. 

`mu` indicates that you don't even know the stack depth. This happens, for instance, in implementing multiple return value functions for a language which supports them (Lua for one). It's just fine to do this, because `mu` words have no influence on the inference of tokens involved in that word, because they're the most general option. 

Note the type system itself is just a set of assertion words. If you're implementing Urbit, you can make `true` a 0 value. Fabri will keep up. We'll have to do something kinda fancy to set up the fundamental relationships, but that's okay, it's the essence of Fabri's inferential abilities. 

So if we define a literal type called `utf8`, an assertion word over `byte` values, adding a `byte` to a `utf8` will give a `utf8`. If we define an address called `@ut8` which contains a zero-terminated UTF-8 string, and we do `6 +` to the address, we still have a `@ut8` pointer, *and* Fabri knows that there's a `utf8` byte at that address, since it knows the 'struct' values. 

there are not structs, you may have noticed. You can do anything at all with an address, static, dynamic, doesn't matter. If you define a semi-traditional struct word, you can do so mostly in the Fabri code, simply using `here n cells allot` or `create does>` in the Forge code. This avoids the word pollution of the gforth-style struct: the type names are off in the corner. 

In general, Fabri permits any operation allowed on the general type to be done on the specific type, and the result is still specific. If the results of a Fabri inference are not to your liking, for whatever reason, you may fix them through assertion. 

What about two specific types? Two ways: the last to be defined wins, or it defaults to the primitive. I like the latter, which might be useful for implementing some of the weirder object systems; this is just a question of sensible defaults, and I don't know what's sensible yet. In general, words that take and return specific values will have assertions in them, and do moderately complex things: we're not going to use a `cell cell -- cell` word like `+` on two disparate address values. *Usually*. If we do, we might want a special assertion word. 

The most important thing is that the user should be able to guess what Fabri is going to do. No surprises, Keep It Simple. 

The type system tracks only two things: assertions, and effects. Effects on the data and return stack, and probably i/o effects. The simplest way is to use mirror stacks, where the data and return offset can be multiplied by some even value and end up in the mirror stack. That way, even if our return stack is quite deep, and we only have a few annotations on it, they'll be in the correct place. That keeps things fast, and stacks are not expensive on modern hardware. 

#Rationale

Forth is designed to work towards the problem from the bottom up. By using comprehensive, inferred typing, with simple set logic, we can safely leave behind types that are behaving well. If we need to go back and introspect the code, the types are right there. 

For a static program, when everything works, you turn off Fabri. For a dynamic program, you leave in the parts you want, and/or build other checks and balances around your exoskeletal types. Fabri is useful for optimization of both static and dynamic programs, in principle; of course, these systems tools would have to be built.

You need a type system, or you can't have types. Every attempt to add them directly to Forth, ruins Forth. Inference is the big win of the last ten years in typing, so we put the inference in annotations and use it directly during development. 

Ultimately, and this will take a working Fabri and a fully annotated core dictionary, you simply type your Forth words, and unless you're typing something, the whole thing simply works.