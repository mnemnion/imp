
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
		   #esc 
			<> if  
			    1 +
			else                 \ ( n adr #esc -- ) esc-code handler
			    1 +      
				begin
					swap 1 + swap
					dup c@ 
						dup [char] m 
						= if
						    drop 1 + 
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

: .title \ ( buf off frame -> nil -- "frame" )
	\ "titles a frame"
	.save
	dup xy.frame .xy
	colrow.frame drop \ buff off cols

	over - 2 / 
	.fwd bl emit
	type
	.restore
	;