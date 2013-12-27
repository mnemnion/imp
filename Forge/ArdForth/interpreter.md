#Interpreter

Our interpreter can eat a cell at a time. A core word will be a single byte and a space, a compound will be two signal bytes. If both bytes are spaces, keep chewing, if the first is a space and the second a char, shift the char, eat one more byte to get back on alignment.

We start in hex mode and switch to word mode as appropriate. I need to understand stuff like `immediate`, `postpone`, `create` and `does>` better.


