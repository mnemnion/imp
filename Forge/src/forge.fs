include ansi.fs
include toolbelt.fs
include stack-util.fs

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
	create \ ( rows cols x0 y0 -> )
		, , , , ;

: xy.frame \ ( frame -> x0 y0)
	2@ ;

: rowcol.frame \ ( frame -> rows cols )
	2 cells + 2@ ;

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

	To *really* do this right, we'd need to handle Unicode. Hah.
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