
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

include ~+/test/see-and-say.fs


' .hexframe is [default-display]

status colrow.frame status xy.frame 13 + frame: stack-fr

: stack-handler \ ( buf off -> nil -- "str")
	stack-fr .printframe
	;

: .stack
	$s $@ stack-handler ;

.windowclear
status dup .frame .clearframe 
stack-fr dup .frame .clearframe 
0 0 .xy

: loupe see-and-say .! chill ;
