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



	decimal

	\ control code

	27 constant #esc
	10 constant #nl 

	: .^ 27 emit [char] [ emit ; 

	: hide-cursor 
	    .^ ." ?25l" ;

	: show-cursor
	    .^ ." ?25h" ;

	\ colors
	: .!  .^ ." 0m"      ;
	: .r  .^ ." 31m"     ;
	: .g  .^ ." 32m"     ;
	: .y  .^ ." 33m"     ;   
	: .b  .^ ." 34m"     ;
	: .m  .^ ." 35m"     ;
	: .cy .^ ." 36m"     ;
	: .w  .^ ." 37m"     ;
	: .bo .! .^ ." 1m"   ;
	: .di .! .^ ." 2m"   ;
	: .un .! .^ ." 4m"   ;
	: .in .! .^ ." 7m"   ;
	: .bu .! .^ ." 1;4m" ;
	: .ib .! .^ ." 1;7m" ; 
	: .fu .! .^ ." 5;7m" ;

\ color strings

variable colors" 14 cells allot

: .!"   s\" \e[0m"        ;  ' .!"  colors" !
: .r"   s\" \e[31m"       ;  ' .r"  colors" 1  cells + !
: .g"   s\" \e[32m"       ;  ' .g"  colors" 2  cells + !
: .y"   s\" \e[33m"       ;  ' .y"  colors" 3  cells + !
: .b"   s\" \e[34m"       ;  ' .b"  colors" 4  cells + !
: .m"   s\" \e[35m"       ;  ' .m"  colors" 5  cells + !
: .cy"   s\" \e[36m"      ;  ' .cy" colors" 6  cells + !
: .w"   s\" \e[37m"       ;  ' .w"  colors" 7  cells + !
: .bo"  s\" \e[0m\e[1m"   ;  ' .bo" colors" 8  cells + !
: .di"  s\" \e[0m\e[2m"   ;  ' .di" colors" 9  cells + !
: .un"  s\" \e[0m\e[4m"   ;  ' .un" colors" 10 cells + !
: .in"  s\" \e[0m\e[7m"   ;  ' .in" colors" 11 cells + !
: .bu"  s\" \e[0m\e[1;4m" ;  ' .bu" colors" 12 cells + !
: .ib"  s\" \e[0m\e[1;7m" ;  ' .ib" colors" 13 cells + !
: .fu"  s\" \e[0m\e[5;1m" ;  ' .fu" colors" 14 cells + !

\ control holds for number printing
: .#[e  [char] [ hold 27 hold ;
: .#_  32 hold ;
:  #ℳ  [char] m hold  ;
: .#! #ℳ [char] 0 hold .#[e ;
: .#r #ℳ [char] 1 hold [char] 3 hold .#[e ;
: .#g #ℳ [char] 2 hold [char] 3 hold .#[e ;
: .#y #ℳ [char] 3 hold [char] 3 hold .#[e ;
: .#b #ℳ [char] 4 hold [char] 3 hold .#[e ;
: .#m #ℳ [char] 5 hold [char] 3 hold .#[e ;
: .#c #ℳ [char] 6 hold [char] 3 hold .#[e ;
: .#w #ℳ [char] 7 hold [char] 3 hold .#[e ;
: .#bo #ℳ [char] 1 hold .#[e ;
: .#di #ℳ [char] 2 hold .#[e ;
: .#un #ℳ [char] 4 hold .#[e ;
: .#in #ℳ [char] 7 hold .#[e ;
: .#bu #ℳ [char] 4 hold [char] ; hold [char] 1 hold .#[e ; \ oh syntax highlighting
: .#ib #ℳ [char] 7 hold [char] ; hold [char] 1 hold .#[e ;

\ cursor control \ x -- ""
: .fwd  .^  dec. ." C" ;
: .up   .^  dec. ." A" ;
: .down .^  dec. ." B" ;
: .back .^  dec. ." D" ; 
: .save 27 emit ." 7" ;
: .restore 27 emit ." 8" ;
\ screen clear
: .page .^ ." 2J" ;

variable last-xy 1 cells allot 
1 1 last-xy 2!

: .xy   
	2dup last-xy 2!
	.^ swap dec. [char] ; emit dec. ." f" ;

