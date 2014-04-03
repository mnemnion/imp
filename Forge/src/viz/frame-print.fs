
: .frame \ ( frame -> nil "frame" )
	\ prints a frame, without changing the pane. 
	.save
	dup
		xy.frame .xy
	colrow.frame .|box  \ ( nil -- "frame" )
	.restore
	;

: .title \ ( buf off frame -> nil -- "frame" )
	\ "titles a frame"
	.save
	dup .frame
	dup xy.frame .xy
	colrow.frame drop \ buff off cols
	over - 2 / 
	.fwd bl emit
	type
	.restore
	;

: .|wipe \ ( cols rows -- "pane" )
    swap |innerbox                   \ ( row col --  )
    0 do                        \ ( row     --  )
        dup 0 do bl emit loop     \ ( row -- "*+" )
        dup .back 1 .down       \ ( row     --  )
    loop
    drop 
    ;

: .clearframe \ ( frame -> nil "pane" )
	\ clears the (screen) pane of a given frame
	.save
	dup
		xy.into-frame
	colrow.frame
	.|wipe 
	.restore
	;

\ : print-advance  ." *.*" ;  \ dongle


: .|print \ ( [c-str] row col -- "pane" )
	\ (prints c-str into row col )
	swap |innerbox \ ([c-str] x y -- )
	0 do \ ( [c-str] x --        )
	\ 	.! cr .m .s  
		dup >r prints-advance r> 
		last-xy 2@ 1 + .xy
	loop
	2drop drop
	;

: .printframe \ ( [c-str] frame -> nil "pane" )
	\ "prints the contents of c-str into frame,"
	\ "from top left."
	 dup .clearframe
	.save
	dup 
		xy.into-frame
	colrow.frame 
	.|print 
	.restore
	;

: .hexframe \ ( frame -> nil "pane" )
	\ "fills a pane with hexidecimals"
	.save 
	dup
		xy.into-frame
	colrow.frame
		.|hex 
	.restore
	;

: .windowclear \ ( nil -- "clear screen" )
	1 1 .xy cols 0 do 
		 rows 0 do 
			 bl emit
		 loop 
	loop 
	1 1 .xy
	; 