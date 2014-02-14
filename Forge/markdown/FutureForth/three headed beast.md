#A Three Headed Beast

Forth runtime environments are mostly what we'd now call Virtual Machines. The exceptions are interesting, in that several chips have been designed to run Forth, and unlike Lisp machines, you can buy one right now. 

As I've indicated, I consider Forth to be a type of computer program, rather than a language per se. The Forth one writes for a Forth chip is different from, say, the Forth one writes to be as cross-compatible as possible. That's ANS, which is a standard interface that Forth programs may implement. 

ANS as a project is essentially complete. Like Common Lisp, there is lip service to extending the standard but progress is effectively blocked by the sheer number of stakeholders. 

This is all for the good: standards processes that keep mutating haven't exactly produced a standard. 

I see more than enough room for a Forth environment where ANS is treated as a point of departure. Forth is a problem oriented language, and problems are a moving target in general. 

This isn't the place to justify such a project. I'm rather here to mull over the internals of the runtime environment.

##Ortho Forth

An orthodox Forth environment exposes two stacks and a dictionary. On a flagship-grade chip, this is a virtual machine, because all our flagships are register based. It remains very close to the hardware, because of machine operations that directly support instruction pointers and at least one stack. 

The entire opset, at least as accessible through the assembler, is also exposed: integrated assembly is very much a part of any Forth worth using. In general, I'm referring to what's available in the ordinary user word set. In essence, that's two stacks and a dictionary. 

Normally, some optimizations are built in, notably keeping the top of the data stack (the TOS) in a register. A detailed description of the assembler-level mechanics of a Forth runtime takes approximately two pages. 

This is an excellent, conservative architecture. It fits on anything you're ever likely to see. I'm pretty sure someone's written Forth for the abacus. As a direct result, it leaves lots of registers just laying around.

There are proposals to expose a couple virtual registers directly. That's one way to do it. For a number of reasons, I propose a full-on third stack.

##The Return Stack

We say stack and rack for short.

The rack is really there to store the call graph. When a word calls another word, it pushes a token on the return stack, and uses it to return to the original call point. 

It's also overloaded for the notorious and shady practice known as 'stack juggling'. The words `>r` and `r>` toss values back and forth from the stack to the rack, with careless abandon. 

The user must balance each rack operation, in each logical branch. It would be an easy win to simply add static analysis to the compiler, and provide warning when there's reachable code that tampers with the rack. 

There is a whole school of thought that holds that, if you're stack juggling, you're doing something wrong. It happens I juggle actual things, and I do so with two hands. There are dozens of easy patterns where control flow is made simple by a second stack. They may usually be made single-stack, at the cost of clarity and sometimes speed. 

My objection is more subtle: pushing anything but a return token onto the rack pollutes its semantics. 

###A Clean Rack

Forth is homoiconic in ways that differ subtly from Lisp. In particular, there is never an abstract syntax tree. Forth code specifies a call graph directly, and provides, even in ANS flavor, some of the metaprogramming tools to manipulate it.

This provides great potential for optimization. I favor JITing over static analysis. A clean rack, which deals entirely with control flow, makes this job simpler, faster, and more reliable. 

There are a few situations, in particular the traversal of trees, where being able to inspect and even change the calling context is useful. These words become brittle, and break in hard-to-understand ways, if the rack is polluted.

Also, control constructs such as loops have data that needs stashing. Putting it directly on the rack interferes with juggling: getting this portion of the ANS standard correct is reputed to be the hardest part. 

I propose a third stack. The data stack is the 'front' stack, wherein data is passed. This is the 'back' stack, where data is stashed and retrieved. We call this third stack the bag. 

##A Bag of Holding

The fundamental operations are `>b`, `b>`, and `b@`. To prevent dupping in common cases, we may provide `b!` and `b!+` as well. Use of the bag is encouraged. The compiler will require assertion of affect for a word that's supposed to leave things in the bag; typically a word clears the bag state, just as `>r r>` balancing is a must. 

I suppose one could pass variables in and out from both sides, but just... don't. Just don't. There's no need for this at all, and the two paradigms aren't compatible. The stack proper is where information flow happens. Control flow lives on the rack. The bag is a stash.

We want to be able to optimize the bag operations onto registers whenever possible, or eliminate them completely in favor of nondestructive tests. Again, keeping the bag and rack conceptually distinct simplifies this kind of optimization.

###Bagged Definitions.

I favor a two-state Forth. The relationships between `immediate`, `postpone`, `[ ]` etc are moderately complex but lead quite close to a useable metaprogramming environment for Forth. Here's the other big win for a three-stack system. 

In an orthodox Forth, a compiling word such as `:` puts data on the stack. There's no portable way to get at information under the definition. It's easy to turn on interpretation while compiling, but we have to treat the stack as effectively clean. 

In a three-headed Forth, we put the compiling information into the bag, leaving the data stack unchanged. To explain a simple use case: we might use `:noname` to compile an anonymous word, then define a word. The body of that word definition would consist of a macro, that writes out a longer word and inserts the anonymous execution token at a particular place. This is the cure for boilerplate, which Forth is sadly not immune to. Anticipating a more common use, I propose calling `postpone` simply `` ` ``. 

##Rationale

I'm convinced that if a runtime is easy for the user to understand, it will be easy for a program to understand. That's what optimization is, in essence: a program understanding another program and transforming it into as little program as necessary. 

By adding a type layer, we expose additional information to the optimizer. The type system is not a part of the runtime per se: it provides a sanity check during interpretation and compilation, and offers assistance to the optimizer. Forth is not typically dynamic in its use of memory, making these techniques, in combination, quite powerful.

For the user, I expect the ability to move data from the interpret state to the compile state will be empowering. It enables generative code, and a flexible, readable approach to conditional compilation. Some benefits of macros pertain only to dynamic systems. For the rest, this will suffice, though a better story around `create does>` words is important. 
Keeping the rack clean means control flow is literal and easy to follow. This makes it easier to transform inefficient code calls into the least path. There is a tension between reusing code and speed, which optimization resolves. Doing so at the latest possible time has few downsides and is demonstrably superior for important cases. 

What if we want to port to a Forth that doesn't have a bag? It depends. Putting bag activity back on the rack will mostly serve, but any code that depends on a clean data stack during definition will need some serious shimming to work correctly. 

Personally, I'm mildly concerned with running ANS Forth in the Ficus runtime, and not at all concerned with porting Ficus code to any other Forth environment. It is by no means a clean slate rewrite, but it is intended as a break with tradition. 

The long term prognosis is thousands of cores, heterogenous machine architectures, partially shared memory, and paranoid computing. We need a master environment for taming this level of complexity. Forth, because of the nature of its interactivity, provides an optimal environment for this, or nearly so. Clarity of design in our Unix runtime will have payoffs as we move towards next-generation systems.