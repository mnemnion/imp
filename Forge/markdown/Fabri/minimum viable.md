#Minimal Viable Fabri

At this point I'm boostrapping three Forth projects simultaneously: the Forge libraries have some active development, and I have a small Orcish simulator that I'll keep toying with for awhile.

Orcish is a sideline, though a relaxing one. The point is, I can't add a Forth. I'm already three Forths occupied. Heh. Heh.

The front end for Forge and some usable abstractions for buffering and composing strings is worth getting places with. Code is as good as the demo: All of this will go faster if we can plug in with an interested open-source community. 

I'd like to get Fabri running in a crude form as soon as possible. Most of the syntax and logic can be developed with a parallel interpreter, without having to start hacking apart an existing Forth or writing a new implementation. 

###Retro?

I sort of mused aloud about using Retro for the proof of concept. It occurs to me that the annotation process requires annotation of the words you're actually using, and nothing else, so we don't need full ANS coverage right away to demonstrate / develop the system. 

I've been working with GForth as the most mature environment. I rely on as few GForth words as possible, because I'd like to avoid long-term sticky GPL entanglements. One of the goals of the xterm library is to get the codebase booting against PForth also.

## Minimum Viable Interpreter

I'm at a stage that's a little odd for me: I have a clear sense of the algorithm and logic but am having trouble describing the data structure. 

The basis of it is a separate wordlist containing the annotations in some usable form. That last part is key.

The way the interpreter works is simple. It maintains a separate stack containing the annotations, and updates it with each word. It then rifles the data stack contents, and the return stack if there's an `r` type operation in play, to make sure everything is kosher. In interpretation mode, this is relatively straightforward. 

Fabri follows the same rule as Forth: it has no knowledge of anything that isn't in its dictionary, which is loaded in the same order as the Forth program. I see no use for a `defer` mechanism other than infinite regress and the need for a union algorithm. 

Followed to its logical conclusion, Fabri has little interest in the Forth. It compiles the annotation for every word on load: it merely needs to `= if` the matching annotations. I'm prevaricating here; "`=`" has to do inference as to allowable types on the fly, otherwise `dup` will cast the TOS and NOS to `cell`.  

Furthermore, there's no alternative to checking the stack on any sort of branch operation that could effect the stack outcome. There's no way to tell if a branch will effect the stack outcome without adding a layer of backtracking: the naive way of checking the actual return value of `if` and friends is likely to prove more flexible and less costly. This is an ideal point to come back to and add hooks for runtime optimization.

What it comes down to is, I haven't refined the syntax of Fabri enough to have a good sense of what an 'annotation' cell should actually look like. Ironically this is where highly dynamic languages like Lisp really shine, one would just hack it up with keywords and a-lists. 

Well, what's the dumb and ugly thing to do? Store the whole string; parse it by hand every time. Maximum flexibility, single point of logic. It's not as fast as it could be, but it's still a single-pass compiler with no need for an intermediate form. 

### What this doesn't cover

In short, dynamically stored contents in memory. It *almost* covers this but there are ways around it; good programming will go a long way towards mitigating this problem.

Basically, if you're storing into a dictionary entry, Fabri can look up the definitional annotations and find out what the areas in memory refer to. Maybe you've got one cell that's a count and 128 cells that are a pad. What have you. So if you use a memory access word that's accidentaly offset to return two pad cells instead of a pad cell and the count, this can be checked. 

If you go in there directly with `@`, you've lost the type information. If you store one type of data into the pad and try to retrieve it as another type, Fabri couldn't help you unless you used a typed version of `!` to store the information. So use typed memory access words; the extra level of indirection, as we've discussed, is the first thing a JIT strips. 

When this matters, we provide a definite type to the pad in question. Fabri can look up anything that's in the dictionary, so it can know that area should always be `bongo`s and if you try to write a `binghi` it can both complain and redefine the word's annotation to include a `binghi`. If the pad is of type `mu`, then redefining it to hold a `bongo` is correct behavior and Fabri will simply do so. 

Typing your pad spares an annotation; Fabri has no choice but to re-type a dictionary words data store if you add a typed field to it. 

This will eventually matter if we're trying to write a dynamic system on top of Forth. Sticking to the global-hyperstatic paradigm, and choosing to type memory areas that function as transcient regions, is both good Forth and comparatively easy to support. 
