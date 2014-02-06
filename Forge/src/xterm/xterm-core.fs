( # Xterm core library )

require ~+/core/core.fs

( ## Control code words )

\  ansi-color 	\ ( byte -> str)
\  takes a byte, composes the appropriate single ANSI command (fg, bg etc.)

\  xterm-fg 	\ ( byte -> str)
\  takes a byte, composes the appropriate xterm-color foreground command. 

\  xterm-bg 	\ ( byte -> str)
\  takes a byte, composes the appropriate xterm-color background command. 


27 charpad str $esc  \ there we go

$esc char [ charpad $cat str $^

27 constant #esc

10 constant #nl

: .^ 27 emit [char] [ emit ;

: xterm-fg ( byte -> str )
	\ "pads a string that changes the foreground color"
	\ "to byte"
	$^  ;" 38;5;" $cat 
	swap #->$  $cat  
    ;" m" $cat 
	;

: xterm-bg ( byte -> str )
	\ "pads a string that changes the background color"
	$^  ;" 48;5;" $cat 
	swap #->$  $cat  
    ;" m" $cat 
	;

: ansi-color ( byte -> str )
	\ "pads a string that changes the background color"
	$^ swap #->$ $cat  
    ;" m" $cat 
	;


