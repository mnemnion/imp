#Subject Oriented Forth

This is just an idea.

Forth and objects don't mix. Fabri will let you design an object system, but you'll want a different language than Forth to use it with.

But Forth, on a nice roomy system, especially a parallel CPU one, offers another option: subject oriented Forth. Each subject is its own Forth environment, with stack, dictionary pointer, parser, and all the usual goods. Ideally, they communicate in ordinary Forth code, to keep everything honest and easy to debug. 