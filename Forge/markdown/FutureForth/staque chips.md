#Staque chips

More Forth thinking. Hardware level.

Normal Forth machines are two-headed: there's a rack, primarily for the call graph, and a stack for holding and passing data. For various reasons, I've contemplated adding a "bag" or back stack to next-gen Forth, and banning `>r` and `r>` at a type system level from being used casually. 

Today I'm thinking about another strategy: put things at the bottom of the data stack, and retrieve them. GreenArrays chips have a 'round stack' which is impossible to leave, though it's quite possible to overwrite: in keeping with the spartan nature of Chuck's work, this stack queue is quite small. On a more general-purpose node, 32 cells for the data staque and at least 64 for the rack is reasonable. 

We would introduce two opcode-level commands, `down` and `up`. Down takes top-of-stack and puts it at the bottom, `up` reverses this procedure. This poses a question, since a rolling stack normally has a 'top' but not a bottom. It boots at 0, presumably with the cells also at zero: `2 drop drop` will put the stack head at -1. Pushing values past the ceiling wraps around to the floor, and so it goes. 

No way around it, we have another internal register that tracks the bottom. This gives a nice benefit that we can set a flag when the top hits the virtual bottom. We could do that just as well by flagging the moment the cell above TOS becomes 0, that is, whenever we push to -1, but then we wouldn't get the fancy. 

So, `2 3 down . .` prints "2 3" and leaves the stack at -1. Follow? Lovely. 

##A RISCY Forth

It seems to me that the orthogonal instruction set of ARM has something useful to teach us about a fast Forth chip. If you know your register assembly, it's mostly "add this register into that register, move this into that, call the instruction pointer and increment it, add and call", etc. 

With Forth, we only need some very basic stack effects and math operations. The instruction pointer nominally takes care of itself. What I find interesting is the notion of making the stack effects and compare/jump/arithmetic operations orthogonal. So we'd have `dup-add`, `over-add`, `swap-add` (useless but they're orthogonal remember), `rot-add` `up-add`, `down-add`, `drop-add`, `nip-add`, `-rot-add`, `tuck-add`, etc. 

Since everything happens to TOS and NOS, or just TOS, for any basic operation, we don't need the width to specify source and destination for operations. There's no reason to waste a cycle moving stuff around before doing whatever we're going to do to it. This would tighten up the performance quite a bit, if it can be efficiently baked into silicon. I'm not the fellow to ask; my knowledge of that scale is confined to moist, carbon-containing structures. 

##Optimization 

Keeping a chip fed is increasingly challenging. Bits only move so fast. Inevitably each of our nodes will have multiple levels of store. Each needs a local store that can feed the chip at 1-to-1 rates; any code in the local 32-64 kiB cache can saturate the clock. 

Next closest is the caches of nearby nodes. These are read-only, in the main; the chips can only toss numbers onto each other's TOS and throw an interrupt onto the rack to handle them. After this is the main store, which is off in the boondocks. 

Clearly, to keep the nodes fed, *something* must be able to write into the local cache. These are either 'boss nodes' hanging out near main memory, or an external chip, perhaps in a more conventional architecture. The nodes should be configurable for peer mutability, that is, the difference between a peer node and a boss node is firmware and merely determines whether the relevant instructions are executed or no-op'ed. 

None of this is automatic. We don't have to support an entire genome like x86, and we want to be able to do things like run a sequence on one node while profiling the code off another, hot-swapping in the reduced sequence. 
