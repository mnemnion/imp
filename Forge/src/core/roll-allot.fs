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

( : roll-allocator \ ( C: cell-offset -> nil := roll-alloc! )
	\ "creates a rolling allocator."
( 	create 1 - cells allot 
	does>
	20 dump

	; )
