( Rolling Allocator

	Create: 2 cells, one for limit one for offset, plus N cells memory:

	\ { n-cells -> !limit !offset := roller }
	 
	limit and offset are in bytes. The allocator keeps allocations cell-aligned.

	Does: \ { request -> < offset | 0 > flag }

	request is in bytes.

	Behavior: There are three conditions.

	If the request exceeds the limit, we exit in failure. 

	If the request plus the offset is less than the limit we must:

		add the request to the limit -> new-offset.

		save the new offset into the roller. 

		add the *old* offset to the address.

		return this, and the flag 1.

	IF the request plus the offset is greater than the limit we must:

		reset the offset to *2 cells* plus the request and store it.

		add 2 cells to the address 

		return this, and the flag 1.

 )

\ note: in the case that we request an even number of cells, an extra cell is allocated.

\ as no effort is made to clear the buffer between resets, this is not a reliable pad.

\ It's just faster and cleaner, and these buffers are for short-term use by definition.

\ an actual problem: it allocates an extra two cells at the end which are unreachable. I want to fix that.

 : (rollocate)  \ ( adr req -> )
 	cr .cy ." rollocation  " 
 	over 2@
 	rot rot over +
 	rot < if 
 		cr .y ." room in buffer"
 		cr .s
 		over 2@ drop \ ( adr req limit -- )
 		dup >r + 
 		over cell + !
 		r> + 1
 	else 
 		cr .b ." no room in buffer"
 		cr .s 
 		over cell + !
 		2 cells +
 		2
 	then
 	cr .w .s
 ;

 : (over-limit?)
 		swap 
 		dup cell mod 0 <> if
 		 		dup cell mod cell swap - + \ pad out to cell width 
 		then
 		swap     
 		dup           \ ( req adr adr )
 		2@            \ ( req adr offset limit  )
 		cr .g ." offset limit: " .s
 		nip rot tuck  \ ( adr req limit req)
 		<
 ;

 : roll-allocator 

 	create ( limit-cells -> ,!limit-bytes ,!offset ,buffer := roller )
 		dup  
 		cells ,       			\ limit in bytes
 		2 cells ,		  		\ allocated near rolly limit: test
 		cells allot             \ room for our buffer
 	does>  ( request -> < offset flag | 0 false > )
		(over-limit?) if
 			cr .r ." over limit!" 
 			2drop 0 0 
 			cr .r .s
 		else           \ adr req
 			(rollocate)
 		then
 		.!
 	;

128 roll-allocator rolly

