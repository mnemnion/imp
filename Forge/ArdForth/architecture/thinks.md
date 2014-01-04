#Thinks

Thinks are short-term buffered commands that an Orc gets over the wire. 

Thinks are a response to the Harvard architecture, which has separate mechanisms for addressing RAM and code store. 

We'll need a separate interpreter word, DOTHINK, for handling thinks. Conceptually, it's very simple: It loads a word at a time from memz and executes it. 

In practice this will be tricky code, especially if we want recursion, tail call elimination, and the ability to store subroutine words in short-term memory. We should be able to do all these things, but I'm not good enough with asm yet to really picture the flow here.