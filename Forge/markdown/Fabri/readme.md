#Fabri

Forth is a notoriously untyped language. This is the source of its expressiveness and power.

Fabri works to build on that foundation by adding static, inferential type validation. It runs with the outer interprer and has no direct effect on the compiled code. 

Fabri is conceptually simple. It builds on the word annotations that good Forth practice insists on. This is an informal type system, specifying the stack effect and 'types' of the begin and end stacks. There are standard variations which specify most of the states in which stack effects can occur, and ways to express some side effects also.

Fabri will successively formalize the relevant pieces of this language, extending and changing it where necessary. It will then interpret the annotations in parallel with the outer interpreter, validating the transformations a word at a time and checking the correctness of newly annotated words. 

With development, we should be able to infer the minimum stack effect of a single word, rendering Fabri almost invisible. The annotations may be created and hidden dynamically, and never effect the actual threaded or optimized code. 

It should be possible to compile an image, forget the Fabri code, and run the resulting Forth with a corresponding loss of introspection. From the other side, the Fabri environment will have a vocabulary supporting introspection of running code, smart dumps and stack printing, annotation generation, and conceivably quite a bit more. 

##Guide

The Markdown to code ratio here is almost criminal. Anything not reachable from this page should be considered off-trunk on purpose.

These files should be read in about the following order:

* [Typed Forth](typed forth.md) the only virtue of this is that I wrote it first. 
* [Forward Inference](forward inference.md)
* [Syntax and Comments](syntax and comments.md)
* [Annotation Strings](annotation strings.md)
* [Interactive Back Word Inference](interactive back word inference.md)
* [Minimum Viable Fabri](minimum viable.md)
