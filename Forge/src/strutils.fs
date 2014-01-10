
: type-n 		\ ( c-adr n -> c-adr+n "str" )
	\ "prints n characters from c-adr"
	0 do
		dup 
		c@ emit
		1 +
	loop
	;

: next-cr      
	\ "takes a c-str, returns a c-str at the next newline, w/ flag."
	s\" \n" search ;

: next-ansi \ ( [c-str] -> [c-str+] | 0 )

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
		noop
	else 
		nip
	then
    .!
	;

: skip-ansi \ ( c-str -- c-str)
	\ "takes a c-str, with an escape code, and skips past it."
	s" m" search 
		if 
		swap 1 + swap 1 -
		else ." error" then
	;