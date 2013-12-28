#Miscellaneous Forth Observations


## create and does>

I finally figured out `create` and `does>` do.

`create` causes a dictionary entry to be 'created'. It has a name, which is the next word in the input stream.

`does>` defines what that word is going to do, when found in the dictionary. 


##Cons cells and Forth words

On a concrete level, Lisp lists end on a `nil`, or `0`, and Forth words end either on `exit` for a coded word or `next` for a primitive. 

Words and symbols work similarly, except Forth has no environment: the data structure for word resolution is exposed and subject to introspection and modification exactly the way that the program structure is in a Lisp. 

 