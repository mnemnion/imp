
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

	;