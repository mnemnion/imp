
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

: next-ansi

	\ "takes a c-str, returns offset of next ansi color sequence,
	\ as -- adr stop start true, or -- adr original-count false "
	
	\ we detect only the ANSI escape sequences we actually embed.
	\ adding more complicates this word, but shouldn't change the
	\ interface.
	0 
	15 0 do 
		-rot 2dup  \ ( rem c-adr count c-adr count -- )
		colors" i cells + perform \ ( r [c] c-adr count comp-adr c-count -- )
		cr ." string " 2dup type ." effect "
 		search \ ( r [c-str] result count flag )
 		." flag " dup .
 		cr .s 
 	 	if \ found ( r [c-str] result count] ) 
 	 		 
 	 		swap >r >r rot r> \ ( c-adr c-count r count -|- result )
 	 		max r>
 	 		cr .s
 	 		drop 
 	 		cr .s 
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