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
 \ 	cr .cy ." utf?: " .s
	dup utf-lead? if
		utf-bytes?
\ 		cr .cy ." bytes: " dup . 
		1
	else
		-1
	then
	;

: text-printables?
	dup 127 < if
		2drop 1 1
	else
		nip utf-printables?
	then
	;


\ tab handling? Currently returns 1 for any ctrl character < 127.

: (1-print) \ ( buf off -> count flag)
	\ returns next printing value
	\ flags: 1 is a printable, 0 an unprintable sequence, -1
	\ a malformed character
	dup 0 = if
\ 		cr .r ." end of buffer"
	    true 
\ 	    cr .s
	else
		drop dup c@ dup
		case 
\ 			cr .bo .w .s
			#nl of drop 0 				1   	endof
			#esc of esc-printables? 	false	endof
			otherwise text-printables? 	    	endother
		endcase
	then
\ 	cr .g ." 1 print return: "
\  	cr .s .!
	;

: (advance) \ ( buf off count -> buff+count off-count )
	\ " advances the buffer by the counted quantity"
	tuck - >r
	+ r>
	;

: 1-printable \ ( buf off -> count flag )
	\ "returns the number of bytes needed to print 1 character on the screen"
	\ "pretends all Unicode is single-width"
	\ "negative counts are the number of malformed characters to skip"
	\ "true flag indicates 1 printable; false means we need to keep parsing"
 	0 >r
	begin	
		2dup (1-print)
\  		cr .cy .s
		false = if
			dup r> + >r 
		 	(advance)	
		 	false
		else 
 \ 			cr .g .s
			dup 0 > if 
				nip nip r>  + true
			else
\ 				cr .cy .s 
				nip nip r> + true
			then 
\ 			cr ." after: " .s
		then \ test for malformation
	until 
\ 	cr 175 xterm-fg .$ .s
\ 	.!
	;

: printables \ (buf off n -> count )
	>r 2dup r>
	0 do
		2dup 1-printable
		dup 0 > if
			(advance)
		else 
			2drop leave
		then
	loop
	drop nip swap -
	.!
	;

: print-advance \ ( buf off n -> buf+ off- "string" )
	over 0 <> if 
		>r 2dup r@ printables 
		dup 0 > if
	\ 		cr .g .s
			3dup nip type (advance)
		else 
	\ 		cr .y .s
			drop
			1 (advance) r@ recurse
		then
		r> drop
	else 
		drop
	then
\ 	.!
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