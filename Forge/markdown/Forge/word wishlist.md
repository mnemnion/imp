#Words I Wish Were in Forth

# Prior

The fact that a colon word doesn't immediately compile itself is a mistake Chuck has since rectified. 

It's a serious mistake. `: a-user-word create ... does>` puts one copy of the address on the stack. Want another? Juggle that puppy. ` ' a-user-word ` will not do the obvious. Forget recursion: this makes multi-state conditional execution of a `does>` clause pointlessley painful.

It's also dirt simple to have a word `prior` that lets the colon word call the previous version of the colon word. Which supports the only use case presented for this error, namely `: foo foo other-stuff ;` which would of course be `: foo prior other-stuff`. `'prior` would pollute the namespace less, perhaps. 

# The Bag

It would be nice if everything on the return stack was, y'know, a return token. That would allow for some metaprogramming: the kind that's dubious as a trick, but incredibly powerful as systematic practice.

I'm picturing a three-headed stack machine. It happens we have a lot of registers in amd64, and can afford this.

The bag works just like the rack: the fundamental words are `>b` to put the dak on the bak, `b>` to reverse. Oh, our stack positions are `dak, duk, dik`. The mnemnions are `data stack, alpha position` `data stack, under position` and `data stack, inside position`. We have the `bak, buk, bik` as well as the `rak, ruk, rik`. These are very low level terms: we might talk about whether the `bak` is a register with a `buk` pointer, or if, as is more simple and likely, the bag has a `bak` pointer only. Sure, `over` puts the `duk` on the `dak`, the `dak` in the `duk` and the `duk` in the `dik`, but this only comes up when we're building `over` for something. 

Orcs have no rack, doubling up with the bag, in common Forthian fashion.

The bag is an excellent candidate for register optimizations. This is a dumb compiler thing: trace the bag path, assign registers to safe `>b` cases, and read from them with the corresponding `b>` or `b@`. This segregation should also make trace optimization cleaner, because the return stack contains only returns. 

### Bag and defining words

Having the bag around can help us get around one of the major problems in writing Forth in a functional idiom, namely,
that `:` covers the data stack. 

We want to be able to leave values on the data stack, start a definition, and call immediate words that have effects on those values. A simple case would be a `:noname` lambda, which leaves its xt on the stack when we reach `;`. It would be nice if we could use `literal` to compile that XT where we want it: this can be combined with `postpone immediate` to macro-generate code. 

If we build words on the bag, we can make this work. Off all the weird things you can do while compiling, switching to interpretation mode to read-write the return stack is the weirdest. Even if we switch to interpretation mode and call a word that uses the bag, bag effects are expected to always be balanced, so this should be fine. 

### Another note about juggling

Stack juggling is the most feared and disliked aspect of Forth. That's because you have to learn it with a blindfold.

It is far and away the most powerful feature, of course. No other flow control paradigm I've encountered allows for the kind of flexibility it provides. Fabri is largely designed around keeping track of the juggle: learning complex five-ball routines without colored balls is pointlessly difficult, the kind of thing no serious player would ever do. 

Conflating the return stack and the bag is an understandable mistake; I'm told there are languages that use only one stack, though I can't imagine how they function. I'm told 'frames' are involved, which sounds scary and hard to follow. 

Indeed, a slightly less dumb compiler could implement the bag using the rack, on some kind of constrained system. If all user code touches only the bag, *and* is properly balanced viz. the return stack, you can do this. 

There are a number of sensible algorithms that 'pour' a stack back and forth, like the classic Towers of Hanoi in Forth.

Building a finite state machine is always hard: one of the best ways to systematically cheat is to look back into the return context. This is particularly useful for implementing complex trees. Without three stacks, these two techniques cannot be combined orthogonally. 

The bag being on the return stack is a complecting, to use a Clojurism. It complicates analysis for the user and the computer.  

### The Bag and Pain

One advantage of using the return stack is that if you try to pass values between words using it, you will almost certainly crash. That's only an advantage because of the defiantly primitive environment of modern publicly-available Forths. Fabri will be very stern with you if there's anything on the bag or return stack that shouldn't be, when you exit your definition.

### Lambda

interpretation semantics: stores the contents of tos stack somewhere
compilation semantics: compiles the last stored value followed by 'perform'

:noname does something important ; lambda : named-word does something lambda something else ;

lambda lasts until you reuse it. It's up to you to make sure there's an XT in it. 

