( Rolling Hammerspace )

\ This is just a big rolling allocator. 

require ~+/core/core.fs

1 MiB cell / roll-allocator hammer 

: ham!  ( buf off -> str ) 
	\ "stores a counted offset into hammerspace"
	dup cell + hammer if
		dup >r $! r>	
	else
		cr ." Hammer is Not That Big" 
	then
	;  

: $ham ( str -> str )
	$@ ham! ;