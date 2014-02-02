
-127 constant {ascii}

-1024 constant {csi}

: mouse-parse
	drop
	key \ type
	key 32 - \ x
	key	32 - \ y
    rot    \ ( rows cols type -- )
	;

: csi-other-handle
	cr .m ." unusual CSI sequence"
	;

: csi-parse 
	drop
	key? if
		key
		dup [char] M = if 
\ 	    cr .m ." mouse event"
	   		mouse-parse
		else
			csi-other-handle \ silently dispose of esc-[; blocks it as command.
		then
	else 
		cr .y ." donk" .s
		[char] [ #esc 
	then 
	;

\ we may want access to esc-[-notM for consistency. If so we need a special
\ return type, that says : there was a CSI before this next event. 
\ this might be a good idea since sending CSIs is the kinda thing that happens.

: command-parse   
	#esc \ put esc back 
\ 	 cr .m ." command "
\ 	 .s 
	 ;

: escape-parse 
\ 	cr .cy ." escape sequence"
	drop
	key dup [char] [ = if
\ 	    cr .r ." CSI"
		csi-parse
	else 
		command-parse
	then 
	;

: (event) ( nil - mu event-flag )
	\ like "key" but refreshing and different 
	dup 27 = if
		escape-parse 
	else dup printable? if
\ 		cr 56 xterm-fg .$ ." printable"
		{ascii}
		else 
\ 			cr .r ." other"
			0
		then
	then 
	.!
	;

: event 
	key (event) ;

: events \ "event, plural"
	0 do event loop ;