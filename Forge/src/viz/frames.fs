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

	the input handler processes events. Typically it will receive one event,
	handle it, and turn over control to the window loop to refresh the display.
	
)

require ~+/util/util.fs

defer [default-input] 
:noname	cr .r ." no input attached" .! drop
	; is [default-input]

defer [default-display]
:noname	cr .r ." no display attached" .! drop
	; is [default-display]

: frame, \ ( cols rows x0 y0 -> nil -- := ,frame )
		, ,                   		\ ( cols rows -- ,y0 ,x0 )
	    , , 				     	\ ( nil -- ,cols ,rows )
		['] [default-input] ,       \ ( nil -- ,i-handle )
		['] [default-display] ,     \ ( nil -- ,o-handle )
		;

: frame:  \ ( C: cols rows x0 y0 -> nil := 'frame'
             \   D: nil -> frame )
	create frame, ;  


: xy.frame \ ( frame -> x0 y0)
	2@ ;

 	 	
: xy.into-frame
	xy.frame 
	swap 1 + swap 1 +
	.xy ;

: set-xy.frame \ ( x0 y0 frame -> nil -- !x0 !y0 )
	 2! ;

: colrow.frame \ ( frame -> cols rows )
	2 cells + 2@ ;

: set-colrow.frame \ ( cols rows -> nil -- !cols !rows )
 	2 cells + 2!
 	;

 : input.frame \ ( frame -> nil -- `i-handle` )
 	dup 
 	4 cells + perform ;

: set-input.frame \ ( i-handle frame -> nil -- !i-handle )
	 4 cells +  ! ; 

 : .display \ ( frame -> nil -- "display" )
 	dup
 	5 cells + perform ;

: set-display.frame \ ( o-handle frame -> nil -- !o-handle )
	5 cells + ! ;




