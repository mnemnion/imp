include ansi.fs
include toolbelt.fs
include stack-util.fs

\ inner loop

variable eval-pad 128 allot 

defer innerloop

\ we need some way to catch all exceptions here. 
:noname  \ no more ." ok" cr
	eval-pad 128 accept 
	eval-pad swap evaluate 
	innerloop ; is innerloop 

: buf-size \ ( rows cols -- rows cols bufsize )
   dup 2 - 
   rot dup 2 - rot *
   ;

: makebox \ ( create := rows cols x0 y0 -> nil does> nil -> rows cols x0 y0  [c-buffer] )
	create \ ( rows cols x0 y0 -> )
		>r >r 
		buf-size
		rot
		r> r> 
		, , , , 
		allot
	does>
		dup                       \ ( box box  -- )
		2@ rot dup                \ (x0 y0 box box -- )
		2 cells + 2@              \ (x0 y0 box rows cols -- )
		buf-size				  \ (x0 y0 box rows cols bufsize -- )
		>r rot r> 
		swap 4 cells + swap ;

: d-box-l-buffer 
	>r >r ;

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