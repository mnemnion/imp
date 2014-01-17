\ code that ain't


: n-printables \ ( [c-str] n -- count )
( 	
	The algorithm: look for a newline. If the literal width is small enough,
	we don't care about printable characters.

	If it isn't, we regex for escape sequences, and subtract their size from the line.

	If it's still not small enough, we truncate.

	We also return a flag, true if we sent a whole line, false if not.

	If true, there is a newline directly after our string. If false, we're still on a logical
	line. 

)
	dup >r -rot r> \ stash n
	>r 2dup \ ( n [c-str] [c-str]  -|- n )
	cr-next? if 
		>r over r> - r> - \ ( n [c-str] c-adr difference )
		cr ." cr found " .di .cy .s .!
		dup 0< if 
			cr .s
			negate nip nip nip nip
		else 
			cr .g ." nl longer than n"
			 \ ( n c-adr count )
			cr .di .s .!
		then 
	else
		r> 	
		cr ." no cr " .di .g .s .!
	then
	;