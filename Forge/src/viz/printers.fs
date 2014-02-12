-127  	constant 	{pr}
-27 	constant 	{0pr}
-1 		constant 	{skip}
-2 		constant 	{nl}
-3 		constant 	{eob}
0  		constant 	{done} 

: esc-printables? 
	drop 1 swap
	1 + dup c@
	[char] [ <> if
		drop 1 + \ 2 byte sequence
	else         \ CSI
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
		{pr}
	else
		cr .r ." malformed"
		{skip}
	then
	;

: text-printables?
	dup printable? if
		2drop 1 {pr}
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
		case 
			#nl of drop 0 				1   	endof
			#esc of esc-printables? 	false	endof
			otherwise text-printables? 	    	endother
		endcase
	then
	;



: 1-pr \ ( buf off -> count flag )
	\ "returns count for next 'print unit' "
	\ "flags: "
	\ " {pr}: count generates 1 printable character"
	\ " {0pr}: count is a zero-width print (ansi-escape)"
	\ " {skip}: skip one byte"
	\ " {eob}: end of buffer"
	dup 0 = if 
		{eob}
	else
\ 		cr .cy .s .!
		drop  \ for safety, should compare this to the return count...
		dup c@ dup \ ( buff char char -- )
		case
			#nl of  2drop 0 			{nl}   endof
			#esc of esc-printables? 	{0pr}  endof
			otherwise text-printables?         endother
		endcase
	then
	;

: (advance) \ ( buf off count -> buff+count off-count )
	\ " advances the buffer by the counted quantity"
	tuck - >r + r> ;

: (skipper)
	drop 1 (advance) ;

: (next-pr)
	3dup nip type 
    dup >r (advance)
	2dup 1-pr 
	case
		{pr} of 
				nip dup >r type 
				r> r> + true endof
		{nl} of
			nip nip r> swap	endof
		{eob} of 
		    nip r> swap endof
		{0pr} of 
		    recurse 
		    r> rot + swap 
		    endof
		{skip} of (skipper) recurse endof
		otherwise r> cr .b .s .! endother
	endcase
\ 	r>
\    cr .g .s .!
	;

: 1-print \ ( buf off -> count flag )
	\ "prints 1 char to the screen"
	\ 5 return states:
	\ {1pr} : we have 1 char; print
	\ {skip} : advance and recurse
	\ {nl}   : we're done
	\ {eob}  : we're done
	\ {0pr} : get another, if:
	\     	{pr} : we (now) have 1 char : print
	\  		{skip} : print *then* skip, done
	\   	{nl} : print return {done}
	\ 	    {eob} : same
	\       {0pr} : recurse
	\ flag is true if we advanced the cursor,
	\ false if we've reached nl or eob

	2dup 1-pr
	case 
		{pr} 	of 	nip dup -rot type true		endof
		{skip} 	of 	(skipper) recurse			endof
		{nl} 	of  nip nip false      			endof
		{eob} 	of 	nip nip         			endof
		{0pr} 	of 	(next-pr)					endof
	endcase
	;

: print-advance ( buf off -> buf+ off- "char" )
	2dup 1-print if
		(advance)
	else 
		drop 
	then
	;

: cr-skip ( buff off -> < buf off | buf+ off -> )
	\ "skip a newline"
	swap dup c@ #nl = if
		swap 1 (advance)
	else
		swap
	then
	;
	 
: prints-advance ( buf off -> buf+ off- "chars")
	0 do print-advance loop
	;

: prints 0 do print-advance loop 2drop ;





























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
				nip nip r> + true
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

: print-advance~ \ ( buf off n -> buf+ off- "string" )
	over 0 <> if  						\ ( buf off n  -- )
		>r 2dup r@
 		cr .cy .s .!
		printables 

		dup 0 > if
			3dup nip type (advance) r> drop
		else 
			drop
			1 (advance) 
			r> drop 
\ 			cr .g .s .!
			recurse
		then
	else 
		 drop
	then
	;

: print-advance~ \ ( buf off n -> buf+ off- -- "string" )
	\ "prints n printables, advancing the buffer."
	\ "safe to call: skips front newlines"
	\ "returns original buffer and offset for off = 0"
	over 0 <> if
\ 		cr .g ." buffer"
		3dup printables 
		dup 0 <> if
\ 			cr .m ." no leading newline"
			nip 3dup 
			nip type
			(advance) 
		else
\ 			cr .bo .y ." leading newline"
			drop >r
			1 (advance)
			r>
			cr .s
			recurse
		then
	else
\ 		cr .r ." no buffer"
		drop
	then
\ 	.!
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