#Time Annotations

This is... this is an idea.

By writing the Forth core for each processor in assembler, we can know, to the tick, how long each word takes. 

That can be used to perform time inference on various calculations, allowing a clever optimizer to reduce time and sleep calls. 

Perhaps. The idea is that we annotate one level above where we're operating, and then at the top level it all comes together. Using a computer to analyse itself is important, but also miserly. We really have a lot of computers. 