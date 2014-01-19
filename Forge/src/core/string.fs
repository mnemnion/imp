( # String Library )

( We define a string, unusually, as an area of memory, fronted with a count. )

( Being Forth, strings have to live somewhere, typically inside a subject.   )

variable ""pad 255 cells allot \ 4k pad

: "@  \ ( str -> buf off )
	\ "takes a string, returning a (byte) counted offset buffer"
	dup cell + swap @
	;

: "!  \ ( buf off str -> nil -- := str! )
	\ "stores a counted offset into a string"
	2dup ! \ ( buf off str -- off! )
	cell + 
	swap cmove 
	;

: "!+ 	\ ( buf off str -> nil -- str! )
	\ "concatenates a string to the end of a counted offset"
	dup @ >r
	2dup
	+!
	cell +
	r> chars +
	swap cmove
	;

: "c+  \ char str -> nil -- char! )
	\ "appends a single char to the end of string"
	dup >R 
	dup @ +
	cell +
	c!
	1 r> +!
	;

\ : ahuh [ 34 parse until" ] ;

: "..   \ like type
	"@ type 
	;

: "pad! "!
	 ;    \ add a proper rolling allocator

: string
	create
	"@ dup allot here "!
	does>
	;

\ `` -- state-smart word.

:noname 34 parse ""pad "pad! ""pad
    ;     
:noname
  34 parse 
  POSTPONE SLiteral 
  POSTPONE ""pad 
  POSTPONE "pad! 
  POSTPONE ""pad ;    interpret/compile: ``

: ->" \ convert one cell to a counted string
	0 <# #s #> ""pad "pad! ""pad ;




