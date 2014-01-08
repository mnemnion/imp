#Dictionaries Demystified

On a concrete level, a dictionary is an instruction word that points to a region in memory, which contains a routine that takes a value as input and returns a value as output. Often, that value is a string. Sometimes, it searches a linked list. 

In Forth, dictionaries are usually more specific than that, and often there's a "the" dictionary. This strikes me as premature optimization. In roomy environments it's common to want lots of dictionaries, and for the search object to be arbitrary. 

Hashes or tries with a linked-list order of search strikes me as a good compromise. If I was big on abstraction, making the whole shebang into a zipper tree would be one way to confuse everyone.

We're not going to add local variables because they're dumb and we're Forth users, dammit. If you give us a ton of registers we'll save a couple extra for optimization, stripe cache into circles, and split up by task. Sharing memory is also stupid, unless by sharing you mean one Forth writing with some other number reading from it and not caring.

Also we have Fabri, so you can give names to stack objects if you want to. Or just type them, or not, your call. A closure is quite easy in Forth, by the way; you allocate a piece of memory with the word, and manipulate it accordingly. Forth is perfectly sensible if you know how to use it well.

So what's sensible terminology? A shelf has the dictionaries, in a certain order. The first dictionary for a given environment is the journal; an environment has a parse pointer, instruction pointer, two stacks, and a shelf. You might call this a 'program', although that probably should include the journal.

When you load a dictionary, by default it goes behind the journal, on the shelf. That makes the words accessible, but puts them behind current definitions. You can write them into the journal, or make them the journal, in which case your previous journal goes behind it, unless you delete it explicitly. There will be words to handle all this, naturally, centering around our one Unforthian habit.

We use words like `word.:dictionary` to refer to words that are on the shelf but behind the journal. A program requires a whole shelf; journals are modular, and must make explicit reference to words not kept on their own pages. We try not to talk about 'files' since we think that idea is broken: 'pages' might turn out to be a 'deck' of 'cards' or something like that. I'm not writing an OS yet.

The order there is backwards from the Java etc. flavor. I think it's more Forthright to say `+.:bignum` instead of `bignum.:+`, not to mention being easier to read. Note to self: if the parser sees `foo.bar` and can't find it, it should search for `foo.:bar` and suggest the fix if that's found. I like helpful errors: we can do the same for `bar.:foo` when you mean `foo.:bar`. `include bignum` will put it on the shelf, `load bignum` would write it into the journal, `include bignum as bn` means you can say `+.:bn`. `include` is a parsing word and can eat as many post-position words as I want it to, without having to use up the colon definition of a word like `as`. I think.

Forge lives in Embassytown, but plans to move out eventually. We always expect to have large sytems to work with unless we're cross compiling. Forth provides a pretty good solution to the environment problem, and we're only planning to refine it slightly. It's cheap to leave most dictionaries in memory and reuse them across programs.
