( # String Library )

require roll-allot.fs

( We define a string, unusually, as an area of memory, fronted with a count. )

( Being Forth, strings have to live somewhere, typically inside a subject.   )

256 cells roll-allocator roll-pad


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


: "pad ( buf off -> str ) 
	\ "stores a counted offset into the roll pad"
	dup cell + roll-pad if
		dup >r "! r>	
	else
		cr .r ." string exceeds pad" .!
	then
	;   \ add a proper rolling allocator

: string \ creates a named string.
	create \ ( str -> ,str := 'str' )
	here >r
	"@ dup 2 cells + allot
	r> "!
	does>
	;

\ `` -- state-smart word.

( 

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

 )


