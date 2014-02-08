orphan-words.fs


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