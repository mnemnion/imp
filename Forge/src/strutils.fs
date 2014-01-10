
: type-n 		\ ( c-adr n -> c-adr+n "str" )
	\ "prints n characters from c-adr"
	0 do
		dup 
		c@ emit
		1 +
	loop
	;

: next-cr      
	\ "takes a c-str, returns a c-str at the next newline."
	s\" \n" search ;