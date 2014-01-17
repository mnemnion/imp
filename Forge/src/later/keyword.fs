
( A keyword is a value that is always equal to itself.

  In Lisp, they are traditionally defined as :foo, which
  we suggest, despite conflict with a few colon-defining words.

  We use a simple monotonically increasing counter;
  the keyword is assigned a number which will always be
  unique within the keyword space. In addition, a later
  definition will always have a larger value than an earlier
  definition. 

  A refinement would be to allow the definition of a keyword only
  if that word is not already defined. This is somewhat unForthlike
  and would change the semantics of keywords; I prefer this approach,
  which would let you have a value for :foo in one library and another
  :foo in your main program without weird bugs. 

  Since we use the dictionary, `:foo :foo =` will be true at any given 
  time. If you pass incompatible keywords around on the stack, things
  won't work.

)

variable __keyword-count 0 __keyword-count !

: keyword
	create 
		__keyword-count dup
			@ here cell allot !
		dup @ 1 + swap !
	does>
		@ ; 
