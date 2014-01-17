
: type-n 		\ ( c-adr n -> c-adr+n "str" )
	\ "prints n characters from c-adr"
	0 do
		dup 
		c@ emit
		1 +
	loop
	;

: trunc-to-match ( \ c-adr count [c-str] -> <c-adr count- true> | <c-adr count false>)
	\ "truncates 1st c-str to first match of 2nd c-str, true if successful."
	>r >r 2dup r> r> search 
	if
		cr ." reached true"
		nip -
	else
		cr ." reached false"
		2drop
	then
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

: ansi-offset \ ( c-str -- c-str offset )
	\ "returns the number of bytes in the ansi offset."
	2dup skip-ansi nip - nip
	;

: get-n-p \ " gets n printables from stream"
	>r 2dup ansi-next? if
			cr ." ansi next"
			trunc-to-match
			ansi-offset r>
	else 
		r> \ finish
	then
	;
: n-printables \ ( [c-str] n -- count )
	\ "takes a string buffer. returns the offset needed to print
	\ "n characters, or a full line. ansi escaped."

	 dup dup >r -rot r> -rot \ stash copies of n
	 0 do 
		dup 
		c@ \ cr .s cr
		dup #nl = if
			i 0 = if
				drop 1 +
			else 
				drop drop \ ( n n+ -- )
				swap - i + 0 swap 0
				leave \ 0 swap 0 protects against final drop/nip. craaaazy
			then
		else
			dup #esc 
			<> if  
				drop 1 +
			else                 \ ( n adr #esc -- )
				drop 1 +        \ decrement the do counter?
				begin
					swap 1 + swap
					dup c@ 
						dup [char] m 
						= if
						    drop 1 + 
						    swap 1 + swap
						    true 
						else
							drop 1 + false
						then
				until 	
			then
		then
	loop
	drop nip \ drop advance adr and nip count, or 0 and 0 for newline.
	;

: print-n \ ( [c-str] n -> "string")
\ "prints n characters, up to a newline, ansi escaped."
\ "will not overflow a c-str with proper count."
	>r 2dup drop r> n-printables
	2dup > if \ safe to print as counted
		nip type
	else      \ print the whole thing
		drop type
	then
	; 

: skip-cr \ ( [c-str] -> [c-str] | [c-str+] )
	\ "skip a leading newline"
	dup 0 <> if 
		swap dup c@ \ ( count c-adr char -- )
		10 = if 
	 	\ 	cr .bo ." cr skipped" .! 
			1 + swap 1 - \ ( c-adr+ count+ -- )
		\ 	cr .s ." "
		else
			swap
		then
	else then 
	;


: print-advance \ ( [c-str] n -> c-adr+ count- "string" )
	\ 	"prints n characters, advances the address correspondingly "
	\   
		>r skip-cr 2dup drop r> n-printables
	2dup > if \ safe to print as counted
		swap >r 2dup type r> over >r 
	\ 	.! cr .cy .s .!
		 swap -  
	\ 	.! cr .y .s  .!
		 swap r> 
	\ 	.! cr .g .s .!
		+ swap 
	\ 	 cr .di .w .s .!
	else      \ print the whole thing
		drop dup >r 2dup type r@ - swap r> + swap
 	then
	; 




