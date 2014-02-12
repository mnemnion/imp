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
	\ 		r@ cr .r ." reached" .s .! drop
			3drop r>  true 
	\ 		cr .cy .s .!
				endof
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

: prints-advance ( buf off count-> buf+ off- "chars")
	>r cr-skip r> 0 do print-advance loop
	;

: prints 0 do print-advance loop 2drop ;


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