( # #Frames

	Frames describe physical space on the screen.

	The interior of a frame is its 'pane'. 

	Frames have six elements, in order: 
	column row
	x0 y0
	input display
	
	input and display are both handlers. 
	input will begin as a global default, and will be added later.

	display will get complex, but to begin it will simply take:
	{ offset frame } and display accordingly. 

	the display handler, not the frame, holds a pointer to the subject,
	which is the data under display. To change frames, move the handler. 
	
)

require ~+/util/util.fs

: [default-input] 
	cr .r ." not yet implemented"
	;

: [default-display]
	cr .r ." no display attached"
	;

: makeframe  
	create \ ( := 'frame' cols rows x0 y0 -> nil )
		swap , , 
		swap , , 
		['] [default-input] ,
		['] [default-display] ,
	;

: xy.frame \ ( frame -> x0 y0)
	2@ ;

: set-xy.frame \ ( x0 y0 frame -> nil -- !x0 !yo )
	>r swap r> 2! ;

: colrow.frame \ ( frame -> cols rows )
	2 cells + 2@ ;

: set-colrow.frame \ ( co)
 	2 cells +
 	>r swap r> 2!
 	;
 	
: xy.into-frame
	xy.frame 
	swap 1 + swap 1 +
	.xy ;
