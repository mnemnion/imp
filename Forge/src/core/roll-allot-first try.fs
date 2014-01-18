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

: (pad-or-reset)  \ ( request limit adr offset -> )
	\ "internal word: either pads the allocation or resets."
	\ b. limit > request + offset 
	swap >r         \ ( request limit offset -|- adr )
	dup >r 		    \ ( req limit off -| off adr )
	rot dup >r      \ ( limit off req -|- req off adr )
	+ > if  \ b.	\ ( nil -|- req off adr )
		cr .cy ." case b"	
		r> r>       \ ( req off -|- adr )
		+ r>        \ ( req+off adr --  )
		\ round to cell
		swap dup 
		cr .r .s
		8 mod 
		cr .y .s
		dup 0 = if
			cr .m ." round" 
			cr .cy .s 
			+ + 1
		else 
			cr .g ." padding to cell"
			8 swap - + + 1 
		then
	else
		r> r> r>
	then
	;

: roll-allocator \ ( C: cell-offset -> nil := roll-alloc! )
	\ "creates a rolling allocator."
 	create dup cells , cells allot ;

 : roll-allocate-does
\ 	does>					\ ( num -> buf flag )
	dup 2@                  \ ( request.num self offset limit -- )
	\ 3 conditions:
	\ a. the request plus the offset is less than the limit
	\ b. the request plus the offset is greater than the limit
	\ c. the request is greater than the limit:
		\ c -> return 0 ( breaking address ) 0 ( false flag )

	\ we want to handle c first.
	swap >r swap >r
	2dup   			        \ ( req limit req limit -- )
	 < if               \ not c. 
	 	cr .cy ." space exists" 
	 	r> r>
		(pad-or-reset)
		cr .w .s .!
	else
		cr .r ." no space"
		cr .r .s .!
		r> r>
	then
\ 	r> r>
\ 	cr .g .s .!
	; 

256 roll-allocator rolly

: rolltest
  129 rolly roll-allocate-does ;