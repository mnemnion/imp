( # Xterm core library )

require ~+/core/core.fs

( ## Control code words )

\  ansi-color 	\ ( byte -> str)
\  takes a byte, composes the appropriate single ANSI command (fg, bg etc.)

\  xterm-fg 	\ ( byte -> str)
\  takes a byte, composes the appropriate xterm-color foreground command. 

\  xterm-bg 	\ ( byte -> str)
\  takes a byte, composes the appropriate xterm-color background command. 


variable "esc cell allot \ lovingly hand-constructed non-printable string.
1 "esc ! 27 "esc cell+ ! 

: xterm-fg ( byte -> str )
	\ "pads a string that changes the foreground color"
	\ "to byte"
	"esc ;" [38;5;" "cat
	swap #->`  "cat 
    ;" m" "cat
	;

: xterm-bg ( byte -> str )
	\ "pads a string that changes the background color"
	"esc ;" [48;5;" "cat
	swap #->`  "cat 
    ;" m" "cat
	;

: ansi-color ( byte -> str )
	\ "pads a string that changes the background color"
	"esc ;" [" "cat
	swap #->`  "cat 
    ;" m" "cat
	;


