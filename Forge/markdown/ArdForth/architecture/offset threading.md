fuk mi

this is a really, really tight space optimization. I expect I'll have to use it, because 1k celz ain't a lot son. 

Every phrase requires bookends, unless it doesn't. Even a headless word needs two cells as bookends: we may find or design architectures for which this is not true, but at the moment we're working on AVRs. Acorn is next but is not two-headed, we'll need to virtualize the second stack just like in AVR land. 

What I'm looking for is a two-cell instruction that will let me execute four cells of code without bookending. I'm probably being premature here, but want to put my thinking away so I have it later if I need it. 

The first instruction jumps to a handler; there's a pointer in the nobz showing where we were. The handler uses that pointer to find the second cell, which has an area of memory. It then counts down, four, three, two, one, and executes exactly four words, then returns to the original instruction area and continues loading.  

This saves one word, while roughly halving execution speed. The handler must be used as many times as it is long in cells, or it isn't justifiable.

This is a real fine tune: somewhere between perfect and just clever. I'm almost writing this to warn myself not to do it unless I have to. 