: esc-printables? 
	drop 1 swap
	1 + dup c@
	[char] [ <> if
		drop 1 +
	else  
		begin	
			1 + dup c@
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
	;

: utf-printables? 
	dup utf-lead? if
		utf-bytes?
		1
	else
		cr .r ." malformed"
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
	    true 
	else
		drop dup c@ dup
		cr .y .s .!
		case 
			#nl of drop 0 				1   	endof
			#esc of esc-printables? 	false	endof
			otherwise text-printables? 	    	endother
		endcase
	then
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
		false = if
			dup r> + >r 
		 	(advance)	
		 	false
		else 
			dup 0 > if 
				nip nip r>  + true
			else
				nip nip r> + true
			then 
		then \ test for malformation
	until 
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
	;

: print-advance \ ( buf off n -> buf+ off- "string" )
	over 0 <> if 
		>r 2dup r@
		cr .cy .s .!
		 printables 

		dup 0 > if
			3dup nip type (advance)
		else 
			drop
			1 (advance) r@ 
			cr .g .s .!
			recurse
		then
		r> drop
	else 
		drop
	then
	;

: print-length \ ( buf off -> length )
	\ "returns the printable length, up to newline,"
	\ "of the buffered string"
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