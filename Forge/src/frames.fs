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
	create \ ( := "frame" rows cols x0 y0 -> nil )
		, , , , ;

: xy.frame \ ( frame -> x0 y0)
	2@ ;

: rowcol.frame \ ( frame -> rows cols )
	2 cells + 2@ ;

: .frame \ ( frame -> nil "frame" )
	\ prints a frame, without changing the pane. 
	.save
	dup
		xy.frame .xy
	rowcol.frame .|box  \ ( nil -- "frame" )
	.restore
	;

: .clearpane \ ( frame -> nil "pane" )
	\ clears the (screen) pane of a given frame
	.save
	dup
		xy.frame 
		swap 1 + swap 1 +
		.xy
	rowcol.frame
    1 - swap 1 -
    .di 
	.|wipe 
	.!
	.restore
	;

( Panes 
	
	Panes are a moderately complex data structure. 
	This complexity is reflected in their use words,
	not the layout, which is a simple string combining
	printable ASCII and escape codes. 

)

: makepane 
	create 				\ (:= "pane" frame -> nil  )
		rowcol.frame 		\ ( rows cols --    )
		|innerbox *			\ ( r*c       --    )
		dup ,               \ ( r*c   -- count! )
		8 / 2 * 			\ ( c-buf --        )
		\   			    dup cr ." alloted " . bl ." cells"
		allot  				\ ( nil  -- c-buf! )
	does> 				\ ( pane -> [c-str] )
		dup 				\ ( pane pane  -- )
		cell + swap         \ ( c-buf pane -- )
		@					\ ( [c-str]    -- )
		;

: .#teststr s" this is a rather long test string. there are no newlines in it. it should fill a small buffer completely to capacity. I'm adding a great deal of extra test to this of the lorem ipsum etce because running out of test string is annoying at best." ;

: .#tnl s\" this \n string \n has \n newlines." ; 

: .#rstr s\" \e[34mthis \e[31mstring is red\e[0m" ;

: .fillpane \ ( [c-str] frame -> nil "pane" )
	\ "writes c-str to the pane. naive."
	dup xy.frame 
		.save 
		1 + swap 1 + swap .xy  \ inside box
	rowcol.frame 	\ ( c-adr count rows cols   -- )
	|innerbox 		\ box small 
	rot drop        \ drop count -- will want it later
	swap     \ ( c-adr rows cols --  )
	0 do
		dup -rot 	\ (rows c-adr rows -- count ) \ cr .b ." s:" .! .s 
		type-n
		swap dup
			.back 1 .down
	loop
	2drop
	.restore
	;