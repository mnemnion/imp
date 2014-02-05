36 12 cols 36 - 1 frame: status

status colrow.frame status xy.frame 13 + frame: stack-frame

' stack-frame is stack-fr



: in-status? \ ( x y -> hit-flag )
	status in-on-out?
	;

: click-respond
		cr .cy .s 
	in-status?
	dup 0= if
		drop .b status .frame
	else 0 < if
		.g status .frame
	else
		.r status .frame
	then then 
	false
	;

: release-respond
	2drop false
	;

: rclick-respond
	.xy* false
	;

: ascii-respond
	emit
	false
	;

: (command-default)
	printable? if
	
		cr .g ." command " emit 
		false false \ default case eats from the wrong place! damn
\ 		cr .s
	else 
		." unprintable :-\ " 
		. false false 
	then 
	;

: command-respond 
	dup 
	case 
		[char] q of cr .b ." bye!" .! 
			drop true endof
		(command-default)
		\ default
	endcase	

	;

: csi-respond
	cr .m ." csi: "
	.$
	false
	;

: esc-csi-respond
	cr 223 xterm-fg .$ ." esc-csi: "
	csi-respond
	;


: control-respond
	cr 177 xterm-fg .$ ." control: "
	. false
	;

: event-respond
	case 

		-127  	  of 	ascii-respond          endof
		32    	  of 	click-respond          endof
		{cmd} 	  of 	command-respond        endof
		35    	  of 	release-respond        endof
		96        of 	." ⇓" 2drop false 	   endof
		97        of 	." ⇑" 2drop false 	   endof
		34     	  of	rclick-respond         endof
		{csi} 	  of    csi-respond            endof
		{esc-csi} of    esc-csi-respond 	   endof
		{ctrl}    of    control-respond		   endof

	cr .r ." respond: other: "  .s 
	true true
	endcase 
    ;

: see-and-say
	begin 
		event
		event-respond
		1 .left?
		.!
	until
	;