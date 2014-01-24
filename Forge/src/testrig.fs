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

page .save
20 6 60 10 makeframe tframe   
20 6 60 17 makeframe bframe
tframe .frame  tframe .clearframe 
 bframe .frame .#teststr bframe .printframe .restore

: .xy* .save .xy .g ." *" .! .restore ;

10 60 .xy* 17 60 .xy*
10 6 + 60 20 + .xy*
17 6 + 60 20 + .xy*

.#hex 16 print-n cr .#hex drop 16 n-printables .bo . .! 

variable foo 100 cells allot

;" String Bar" string bar
;" String Foo " `@ foo `!

.s ..