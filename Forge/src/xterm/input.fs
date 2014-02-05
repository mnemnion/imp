
-27 constant {cmd}

-127 constant {ascii}

-512 constant {ctrl}

-1024 constant {csi}

-2048 constant {esc-csi}

-4096 constant {subj}

variable inpad 

: mouse-parse
	drop
	key \ type
	key 32 - \ x
	key	32 - \ y
    rot    \ ( rows cols type -- )
	;

: csi-end?
	64 127 within ;

: (inpad-on)
	inpad @ swap
		 		$c+ 
		 		inpad ! ;

: csi-other-parse
	[char] [ 

	charpad inpad !  
	dup (inpad-on)
	csi-end? if
	else 
		begin
			key 
		    dup
		 		(inpad-on)
			csi-end?
		until
	then
	inpad @
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
		[char] [ {cmd}
	then 
	;

: command-parse   
	{cmd} \ put esc back 
\ 	 cr .m ." command "
\ 	 .s 
	 ;

: escape-parse 
\ 	cr .cy ." escape sequence"
	drop
	key? if
		key dup [char] [ = if
	\ 	    cr .r ." CSI"
			csi-parse
		else dup #esc = if
		    key
		    dup [char] [ = if
		        2drop
		        key
		    	csi-other-parse
		    	drop {esc-csi}
		    else 
		    	nip command-parse 
		    then
		else
			command-parse
		then then
	else 
		#esc {ctrl}
	then
	;

: event ( nil - mu event-flag )
	\ like "key" but refreshing and different 
	key
	dup 27 = if
		escape-parse 
	else dup printable? if
\ 		cr 56 xterm-fg .$ ." printable"
		{ascii}
		else 
\ 			cr .r ." other"
			{ctrl}
		then
	then 
	.!
	;


: events \ "event, plural"
	0 do event loop ;