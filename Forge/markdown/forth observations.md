#Miscellaneous Forth Observations

What the World calls a programmer, Forth calls a 'user'.

C rewards cleverness. Lisp rewards intelligence. Forth rewards wisdom.

## create and does>

I finally figured out `create` and `does>` do.

`create` causes a dictionary entry to be 'created'. It has a name, which is the next word in the input stream.

`does>` defines what that word is going to do, when found in the dictionary. 


##Cons cells and Forth words

On a concrete level, Lisp lists end on a `nil`, or `0`, and Forth words end either on `exit` for a coded word or `next` for a primitive. 

Words and symbols work similarly, except Forth has no environment: the data structure for word resolution is exposed and subject to introspection and modification exactly the way that the program structure is in a Lisp. In Lisp, this does include the environment also, though on this battlfield many bloody wars were fought. 

## Interpretation and Compilation

In Forth, interpretation is a proper superset of compilation. Compiling is an almost invisibly different thing that the interpreter does. 

When reading in a Forth program, there is always both interpretation and compilation; the result is not a compiled program, it is simply a program. If you want to strip the Forth, additional steps must be taken, and the Forth, though headless, remains. What the world calls compiling, Forth calls optimization.
 