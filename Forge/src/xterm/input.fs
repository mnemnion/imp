
: mouse-parse
	drop
	key \ type
	key 32 - \ x
	key	32 - \ y
	swap rot    \ ( rows cols type -- )
	;

: csi-parse 
	drop
	key dup [char] M = if 
\ 	    cr .m ." mouse event"
	    mouse-parse
	then 
	;

: escape-parse 
\ 	cr .cy ." escape sequence"
	drop
	key dup [char] [ = if
\ 	    cr .r ." CSI"
		csi-parse
	then 
	;

: event ( nil - mu event-flag )
	\ like "key" but refreshing and different
	key 
	dup 27 = if
		escape-parse 
	else dup 32 127 within if
\ 		cr 56 xterm-fg .$ ." printable"
		-127
		else 
\ 			cr .r ." other"
			0
		then
	then .!
	;

: events \ "event, plural"
	0 do event loop ;