: POSSIBLY  ( "name" -- )  BL WORD FIND  ?dup AND IF  EXECUTE  THEN ;
: ANEW  ( "name" -- )( Run: -- )  >IN @ POSSIBLY  >IN ! MARKER ;
require core/core.fs
require ansi/ansi.fs
require util/util.fs
require viz/viz.fs
require xterm/xterm.fs

\ inner loop
variable eval-pad 128 allot 

\ we need some way to catch all exceptions here. 
: innerloop  \ no more ." ok" cr
	eval-pad cols accept 
	eval-pad swap evaluate 
	recurse ; 

\ this dumps us to the command line when we fail.
\ eventually, we want this; innerloop takes over 
\ everything it can handle. 
	
anew wipeout

: .. clearstack ;

include testrig.fs

..

:noname cr ." Welcome to the Machine" ; is bootmessage


