#Interactive Back Word Inference

In general, Fabri performs no back inference, whatsoever. 

We could add it. We could add it and remove all actual annotation from the code. The problem is that the user wants the problem to be in one place. With back inference, the user is told: here is the problem. It's a problem because of this other place, over here. The inferences only show up when they're wrong; it's confusing. 

Our type system doesn't work that way. It can barely keep track of what a range of memory represents right now; it has no idea what it used to be, and doesn't care. Forth being concatenative, the only point of comprehensive back inference is to remove the annotations, which are there for user convenience and can / should be interactively generated during the coding process.

Which brings us to back word inference.

## Back Word?

Yes, back word. When designing a word in a Fabri equipped environment, Fabri can back infer the necessary stack condition from the operations. 

This is hard, but really cool. You write a word, with line-by-line tracking of the changes, and with the before-after picture next to the word definiton filling in itself. When you hit semicolon, you have an annotated word. You may change the annotation of course; there are an arbitrary number of types that may pass through a word. Things like the stack effect will be determined absolutely by the words in sequence. 

The goal is that we only ever edit annotations that can't merely be inferred. Sometimes this is a type restriction (we want only `foo` addresses, even though we're doing nothing `foo` specific), sometimes it's an assertion, which Fabri cannot perform by definition. Sometimes we have a moderately complex word and wish to assign 'local' nicknames to the values being juggled. Er, manipulated. 

Interactivity is great, but we need some kind of intermediate "format on load" kind of behavior where Fabri can take bare code and annotate it up into the running program, saving out the annotated version for later editing. Preferably not without a commit before overwrite; git and friends make it pointless to maintain more than one canonical file in the 'directory' at a time.