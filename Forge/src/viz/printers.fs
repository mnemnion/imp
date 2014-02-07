
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

: esc-printables? 
\ 	cr .m ." esc "
	drop 1 swap
	1 + dup c@
	[char] [ <> if
\ 		cr .bo .w ." 2-byte"
		drop 1 +
	else  
		begin	
			1 + dup c@
\ 			cr .cy ." 3rd byte: " dup emit
			csi-end? if
				swap 2 + \ extra for [, spares some swap 
				true
			else
				swap 1 + 
				swap false
			then
		until	
		nip
	then
\ 	.!
;

: utf-printables? 
 	cr .cy ." utf?: " .s
	dup utf-lead? if
		utf-bytes?
		cr .cy ." bytes: " dup .
	else
		-1
	then
	;

: text-printables?
	dup 127 < if
		2drop 1
	else
		utf-printables?
	then
	;


\ tab handling? Currently returns 1 for any ctrl character < 127.

: 1-printable \ ( buf off -> count )
	\ "returns the number of bytes needed to print 1 character on the screen"
	\ "pretends all Unicode is single-width"
	\ "negative counts are the number of malformed characters to skip"
	dup 0 = if
		cr .r ." end of buffer"
		nip 
	else
		drop dup c@ dup
		case 
			#nl of drop 0			    endof
			#esc of esc-printables? 	endof
			otherwise text-printables? 	endother
		endcase
	then
	;

: n-printable \ ( buf off n -> count )
	\ retrieve enough characters to print n text cells.
	0 swap
	0 do
		>r 
		2dup 1-printable dup 0 > if
			tuck dup r> + >r
			- >r + r> 
			r> 
\ 			cr .m .s
		else 
			dup 0 = if 
				2drop
				r>
				cr .g .s
				leave
			else
				r> drop
				nip nip
				cr .r ." malformed" .s
				leave
			then
		then
	loop
	nip nip
	.!
	;

\ : print-n \ ( [c-str] n -> "string")
\ "prints n characters, up to a newline, ansi escaped."
\ "will not overflow a c-str with proper count."
\ 	>r 2dup drop r> n-printables
\  2dup > if \ safe to print as counted
\ 		nip type
\ 	else      \ print the whole thing
\ 		drop type
\ 	then
\  	; 

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