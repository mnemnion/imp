( # String Library )

( We define a string, unusually, as an area of memory, fronted with a count. )

( Being Forth, strings have to live somewhere, typically inside a subject.   )

variable s"pad 255 cells allot \ 4k pad

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

: ""  \ like s"   
	34 parse s"pad "! s"pad ;

: "..   \ like type
	"@ type ;