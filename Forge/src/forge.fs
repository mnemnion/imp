: POSSIBLY  ( "name" -- )  BL WORD FIND  ?dup AND IF  EXECUTE  THEN ;
: ANEW  ( "name" -- )( Run: -- )  >IN @ POSSIBLY  >IN ! MARKER ;
\ include core/string.fs
\ include core/roll-allot.fs
include ansi/ansi.fs
require util/rollhex.fs
\ include toolbelt.fs
require stack-util.fs
require keyword.fs
\ require xterm/xterm.fs
\ ui
include frames.fs

\ inner loop
variable eval-pad 128 allot 

\ we need some way to catch all exceptions here. 
: innerloop  \ no more ." ok" cr
	eval-pad cols accept 
	eval-pad swap evaluate 
	recurse ; 
	
anew wipeout

include testrig.fs

\ this dumps us to the command line when we fail.
\ eventually, we want this; innerloop takes over 
\ everything it can handle. 

\ ." Welcome to the Machine." cr innerloop