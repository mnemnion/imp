include ansi.fs
include toolbelt.fs
include stack-util.fs
include keyword.fs

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