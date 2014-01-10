
: type-n 		\ ( c-adr n -> c-adr+n "str" )
	\ "prints n characters from c-adr"
	0 do
		dup 
		c@ emit
		1 +
	loop
	;

: cr-next?      
	\ "takes a c-str, returns a c-str at the next newline, w/ flag."
	s\" \n" search ;

: ansi-next? \ ( [c-str] -> [c-str+] true | false )

	\ "takes a c-str, returns offset of next ansi color sequence or false"
	
	\ we detect only the ANSI escape sequences we actually embed.
	\ adding more complicates this word, but shouldn't change the
	\ interface.
	0 
	15 0 do 
		-rot 2dup  \ ( rem c-adr count c-adr count -- )
		colors" i cells + perform \ ( r [c] c-adr count comp-adr c-count -- )
 		search \ ( r [c-str] result count flag )
 	 	if \ found ( r [c-str] result count] ) 
 	 		swap >r >r rot r> \ ( c-adr c-count r count -|- result )
 	 		max r>
 	 		drop 
 		else 
 			2drop rot
 		then 
	loop
	-rot + \ juggle that muggle
	swap dup -rot - swap 
	dup if 
		true
	else 
		nip
	then
    .!
	;

: skip-ansi \ ( c-str -- c-str)
	\ "takes a c-str, with an escape code, and skips past it."
	\ again, our only esc[ sequences end with 'm'.
	\ if you use this on an ordinary string, it will skip past
	\ a random section to the next 'm'.
	s" m" search 
		if 
		swap 1 + swap 1 - \ drop the m 
		else .bo ." error: no ansi to skip" .! then
	;

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
			nip nip \ ( n c-adr count )
			cr .di .s .!
		then 
	else
		r> 	
	then
	;