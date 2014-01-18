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

		reset the offset to *2 cells* and store it.

		add 2 cells to the address 

		return this, and the flag 1.

 )

 : over-limit? 0 ;

 : rollocate  \ ( adr req -> )
 	cr .cy ." rollocation  " 
 	over 2@
 	rot rot over +
 	rot < if 
 		cr .y ." room in buffer"
 		cr .s
 		over 2@ drop \ ( adr req limit -- )
 		dup >r + 
 		over !
 		r> + 1
 	else 
 		cr .b ." no room in buffer"
 		cr .s 
 	then
 	cr .w .s
 ;

 : roll-allot 

 	create 
 		cells ,       \ limit in bytes
 		2000 ,		  \ allocated near rolly limit: test
 	does>
 		swap 
 		dup cell mod cell swap - +
 		swap     \ pad out to cell width 
 		dup           \ ( req adr adr )
 		2@            \ ( req adr offset limit  )
 		cr .g ." offset limit: " .s
 		nip rot tuck  \ ( adr req limit req)
 		< if
 			cr .r ." over limit!" 
 			2drop 0 0 
 			cr .r .s
 		else           \ adr req
 			rollocate
 		then
 		.!
 	;

256 roll-allot rolly

