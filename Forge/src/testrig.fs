
\ test includes

include ~+/test/vocab.fs
include ~+/simulorc/simulorc.fs

\ end includes

: bye .! bye ;

: .#teststr s" this is a rather long test string. there are no newlines in it. it should fill a small buffer completely to capacity. I'm adding a great deal of extra test to this of the lorem ipsum etce because running out of test string is annoying at best." ;

: .#tnl s\" this \nstring \nhas \nnewlines." ;

: .#longnl s\" this \e[34mstring\e[0m has newlines. \nthey are sufficiently \nspaced for my nefarious purposes. \nthat should do." ;

: .#rstr s\" prefix: \e[34mthis \e[31mstring is red\e[0m" ;

: .#hex s\" 0123\e[31m4567\e[0m\e[2m89AB\e[0mCDEF" ;

: hex-n \ "print the hex, over n over"
 hex 0 do
 	i 16 mod  
 	dup 15 <> if
 		0 <# # #> type
 	else \ Red F, for ez countz
 		0 <# .#! # .#r #> type 
 	then
 loop decimal
 ;

36 12 cols 36 - 1 frame: status

: in-status? \ ( x y -> hit-flag )
	status in-on-out?
	;

: click-respond
		cr .cy .s 
	in-status?
	dup 0= if
		drop .b status .frame
	else 0 < if
		.g status .frame
	else
		.r status .frame
	then then 
	false
	;

: release-respond
	2drop false
	;

: rclick-respond
	.xy* false
	;

: ascii-respond
	dup 27 = if 
	
	else
		emit \ something more interesting
	then	
	false
	;

: command-respond 
	dup 
	[char] q = if
		cr .b ." bye" 
		drop true
	else
		cr .g ." command " 
		emit false
	then
	;

: event-respond
	case 

		-127  of ascii-respond          endof
		32    of click-respond          endof
		#esc  of command-respond        endof
		35    of release-respond        endof \ right click drop
		96    of ." ⇓" 2drop false 	    endof
		97    of ." ⇑" 2drop false 		endof
		34    of rclick-respond         endof 

	cr .r ." respond: other" .! cr .s 
	true
	endcase 
    ;

: clickloop
	begin 
		event
		event-respond
	until
	;


' .hexframe is [default-display]

.windowclear
status dup .frame .clearframe 0 0 .xy


