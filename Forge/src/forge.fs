include ansi.fs
include toolbelt.fs
include stack-util.fs
include keyword.fs

\ inner loop
variable eval-pad 128 allot 

\ we need some way to catch all exceptions here. 
: innerloop  \ no more ." ok" cr
	eval-pad cols accept 
	eval-pad swap evaluate 
	recurse ; 

\ Frames

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

: makepane 
	create 				\ (:= "pane" frame -> nil  )
		rowcol.frame 		\ ( rows cols --    )
		|innerbox *			\ ( r*c       --    )
		dup ,               \ ( r*c   -- count! )
		8 / 1 +  			\ ( c-buf --        )
							 dup cr ." alloted " . bl ." cells"
		allot  				\ ( nil  -- c-buf! )
	does> 				\ ( pane -> [c-str] )
		dup 				\ ( pane pane  -- )
		cell + swap         \ ( c-buf pane -- )
		@					\ ( [c-str]    -- )
		;

: .frame \ ( frame -> nil "frame" )
	\ prints a frame, without changing the pane. 
	.save
	dup
	xy.frame     .xy
	rowcol.frame .|box  \ ( nil -- "frame" )
	.restore
	;

: .clearpane \ ( frame -> nil "pane" )
	\ clears the pane of a given frame
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

: n-printables 
( 	this word has to take a counted string with
	a maximum width of printable characters. 
	It returns the number of bytes to read,
	in order to print up to that many characters.
	If there is a newline, it returns the count up
	to, but not including, the newline.

	this requires us to parse ANSI control sequences,
	which is great fun, since they can be of arbitrary
	length. Best of all, there's a 'concealed' text
	attribute! which we will abuse the hell out of, no 
	doubt. in any case, we intend to support ANSI correctly.

	To *really* do this right, we'd need to handle Unicode. 

	Gforth provides an xchar library for dealing with local encoding,
	actually. GNU, sometimes you gotta love em. "include kitchensink.fth"

	The algorithm: look for a newline. If the literal width is small enough,
	we don't care about printable characters.

	If it isn't, we regex for escape sequences, and subtract their size from the line.

	If it's still not small enough, we truncate.

	We also return a flag, true if we sent a whole line, false if not.

	If true, there is a newline directly after our string. If false, we're still on a logical
	line. 

)
	;
	
( 
\ this does something cool:
page 
cols 2/ 
dup 0 at-xy \ jump halfway on screen
2 - rows 2/ 2 - swap .|box \ draw box 1/4 of screen
cols 2/ 2 + 1 at-xy        \ jump back into box
innerloop                  \ start i/oing.
)
anew wipeout