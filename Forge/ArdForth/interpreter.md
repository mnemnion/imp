#Interpreter

Our interpreter can eat a cell at a time. A core word will be a single byte and a space, a compound will be two signal bytes. If both bytes are spaces, keep chewing, if the first is a space and the second a char, shift the char, eat one more byte to get back on alignment.

We start in hex mode and switch to word mode as appropriate. I need to understand stuff like `immediate`, `postpone`, `create` and `does>` better.

Algorithm:

```text
take a cell. 
 
If the first byte is number, 

	check the second byte.

	If second byte is space

		convert and push number

	If second byte is number keep chewing

	If second byte is printable, keep chewing to space, and
		walk linked list for word.

Else, if first byte is printable, check second byte for space.

	If space, calculate offset and get XT. 

	if not space and printable, keep chewing to space, and walk
		linked list for word. 

Else, check first byte for control sequence, else, error. 

```

That's actually the 'interpreted mode' of the 'word recognizer'. I think. We have much Use The Forth ahead; some of the terminology is old enough to contradict accepted use in Common. 
