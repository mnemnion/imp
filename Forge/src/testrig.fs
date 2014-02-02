
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

: click-handle
		cr .cy .s 
	dup 0= if
		drop .b status .frame
	else 0 < if
		.g status .frame
	else
		.r status .frame
	then then ;


 : clickloop

 	begin

	 	event
	 	dup 32 = if       \ mousedown
	 		drop  
	 			in-status?
	 			click-handle 0   \ make a star
	 	else dup 35 = if  \ release
	 		drop 2drop 0  \ dispose of
	 	else dup 34 =     \ 'right'-click
	 then then
	 until                \ exits loop
	 	drop 2drop
	 	event drop 2drop \ clear mouse release
	 ; 

' .hexframe is [default-display]

.windowclear
status dup .frame .clearframe 0 0 .xy


