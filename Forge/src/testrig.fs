
\ test includes

include ~+/test/vocab.fs
include ~+/simulorc/simulorc.fs

\ end includes

: bye .! bye ;

: .#teststr s" this is a rather long test string. there are no newlines in it. it should fill a small buffer completely to capacity. I'm adding a great deal of extra test to this of the lorem ipsum etce because running out of test string is annoying at best." ;

: .#tnl s\" this\n string\n has\n newlines." ;

: .#longnl s\" this \e[34mstring\e[0m has newlines. \nthey are sufficiently spaced for my nefarious purposes. \nthat should do." ;

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

 : .xy* .save .xy .g ." *" .! .restore ;

 : clickloop	
 	begin
	 	event
	 	dup 32 = if       \ mousedown
	 		drop .xy* 0   \ make a star
	 	else dup 35 = if  \ release
	 		drop 2drop 0  \ dispose of
	 	else dup 34 =     \ 'right'-click
	 then then
	 until                \ exits loop
	 	drop 2drop
	 	event drop 2drop \ clear mouse release
	 ; 

page
.windowclear