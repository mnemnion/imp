( # #Frames

	Frames describe physical space on the screen.

	Frames have at least one pane, and can switch between panes.

	Panes are specific to frames, and are not buffers; they are 
	composed areas, used for a combination of literal text and 
	syntax highlighting. 

)

include strutils.fs

: buf-size \ ( rows cols -> rows cols bufsize )
   over over 2 - >r 2 - r> *
   ;

: makeframe  
	create \ ( := "frame" cols rows x0 y0 -> nil )
		swap , , swap , , ; \ I am not a clever man.

: xy.frame \ ( frame -> x0 y0)
	2@ ;

: rowcol.frame \ ( frame -> cols rows )
	2 cells + 2@ ;

: .frame \ ( frame -> nil "frame" )
	\ prints a frame, without changing the pane. 
	.save
	dup
		xy.frame .xy
	rowcol.frame .|box  \ ( nil -- "frame" )
	.restore
	;

: .clearframe \ ( frame -> nil "pane" )
	\ clears the (screen) pane of a given frame
	.save
	dup
		xy.frame 
		swap 1 + swap 1 +
		.xy
	rowcol.frame
	.|wipe 
	.restore
	;

: .|print \ ( [c-str] row col -- "pane" )
	\ (prints c-str into row col )
	|innerbox \ ([c-str] x y -- )
	0 do \ ( [c-str] x --        )
	\ 	.! cr .m .s  
		dup >r print-advance r> 
		last-xy 2@ swap 1 + swap .xy
	loop
	;

: .printframe \ ( [c-str] frame -> nil "pane" )
	\ "prints the contents of c-str into frame,"
	\ "from top left."
	.save
	dup 
		xy.frame
		swap 1 + swap 1 +
		.xy
	rowcol.frame 
	.|print 
	.restore
	;

: .xy   .^ swap dec. [char] ; emit dec. ." f" ;