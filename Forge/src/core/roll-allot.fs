\ require core.fs

\ ( A Simple Rolling Allocator )

\ Assigned a block of memory at birth, defined 
\ in *cells*

\ When called, taking one cell as argument, it
\ allocates memory into that block, until
\ it encounters a block that it can't fit.
\ returns the address and offset. 

\ it then *silently* restarts the assignment
\ at the zero place of the block. 

\ returns -1 in the event that the allocation can't fit;
\ naively, this will fuck up memory access and lose data,
\ but will not overwrite anything. 

\ designed for string pads, to provide some small amount of persistence. 

\ Gen 1: will just fuck up if you try to pass it something larger than it can hold.

: roll-allocator \ ( C: cell-offset -> nil := roll-alloc! )
	\ "creates a rolling allocator."
 	create dup cells , cells allot 
	does>
	dup 2@                  \ ( request.num self offset limit -- )
	rot >r
	rot 			        \ ( offset limit request -|- self   )
	2dup > if  \ buffer can hold request
		cr .cy ." true" .!
		dup 			
		rot swap             \ ( offset request limit request -- self )
		>r rot rot
		cr .g .s .! 
		+ 2dup > if       \ buffer can allocate without reset
			cr .cy ." true!  " .s .!
			r> r>
		else
			cr .r ." false!  " .s .!
			r> r>
		then
		cr .g .s .!
		\ 2 cells + +  \ 
	 \ r> \ test
	else 
		cr .r ." false" !
		r>
	then
	
	; 

256 roll-allocator rolly

: rolltest
  128 rolly ;