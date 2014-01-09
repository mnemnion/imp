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

: makebox 
	create 
		, , , , 
	does>
		dup 
		2 cells + 
		2@ 
		rot 
		2@ ;
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