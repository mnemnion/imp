
-127 constant {ascii}

-1024 constant {csi}

-2048 constant {esc^2}

: mouse-parse
	drop
	key \ type
	key 32 - \ x
	key	32 - \ y
    rot    \ ( rows cols type -- )
	;

: csi-other-parse
	cr .cy ." unusual CSI sequence"
	\ must parse until alphabetic
	[char] [ swap
	dup alpha? if
	else 
		begin
			key 
			dup alpha?
		until
	then
    {csi}
	;

: csi-parse 
	drop
	key? if  \ humans are slow
		key
		dup [char] M = if 
\ 	    cr .m ." mouse event"
	   		mouse-parse
		else
			csi-other-parse 
		then
	else 
		[char] [ #esc 
	then 
	;

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
	else dup #esc = if
	    recurse 
	    drop {esc^2}
	else
		command-parse
	then then
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