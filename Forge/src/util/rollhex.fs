require ~+/xterm/xterm.fs
( Rollhex 
	a simple closure. 
)

: offset-hexpr ( offset n -- new-offset )

 tuck                            \ ( n offset n -- )
 hex 0 do                        \ ( n -- `hex`    )
 	dup i + 16 mod               \ ( n n+i%16 --   )
 	dup 15 <> if                 \ ( n n2     --   )
 		0 <# # #> type           \ ( n -- "n2"     )
 	else \ red F
 		0 <# .#! # .#r #> type 	 \ ( n -- "n2"     )
 	then
 loop decimal					 \ ( n -- `decimal` ) 
 + 16 mod						 \ ( new-offset --  )
 ;

 : hexer  \ ( C: nil -> nil := !hexer D: nil -> nil "hex" )
 	create \ ( nil -> nil )
 		0 ,
 	does>  \ ( nil -> nil )
 	dup >r @ swap offset-hexpr r> ! ;

 hexer rollhex 

 : .|hex \ ( cols rows -> nil "pane" )
 	\ "fills a frame with hex"
	swap |innerbox                   \ (  row col --    )
    0 do                        \ (  row     --    )
    	dup				        \ (  row row  -- ) 
    	rollhex					\ (  row     --    )
        dup .back 1 .down       \ (  row     --    )
    loop
    drop 
    0  ['] rollhex  >body !
    ;