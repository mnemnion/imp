include ansi.fs
include toolbelt.fs
include stack-util.fs

anew wipeout

\ inner loop

variable eval-pad here 128 allot 

defer innerloop

:noname 
	eval-pad 128 accept eval-pad swap evaluate innerloop ;
	is innerloop 
