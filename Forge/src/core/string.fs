( # String Library )

require roll-allot.fs

( We define a string, unusually, as an area of memory, fronted with a count. )

( Being Forth, strings have to live somewhere, typically inside a subject.   )

2 KiB roll-allocator roll-pad
4 KiB roll-allocator cat-pad 

:  $@  \ ( str -> buf off )
\ "takes a string, returning a (byte) counted offset buffer"
	dup cell + swap @
	;

: $!  \ ( buf off str -> nil -- := str! )
	\ "stores a counted offset into a string"
	2dup ! \ ( buf off str -- off! )
	cell + 
	swap cmove 
	;

: $++ 	\ ( a.str b.str -> nil -- str! )
	\ "concatenates b.str to the end of a.str"
	\ to do this we must:
	\ add the a offset to the a buffer
	\ save the a+b offset to the a count
	\ use the a+ buffer offset to store the b.buf
	\ 
	over             				\ ( a b a   --   )
	$@ +             				\ ( a b a+  --   )
	-rot 	         				\ ( a+ a b  --   )
	2dup @ swap @ +  				\ ( a+ a b a+b   )
	\  store new offset into a
	rot  ! 					\ ( a+ b    --   ) 
 	$@ rot swap                     \ ( b.off b.count a+ -- )
 	cmove   
\ 	cr .cy .s .!
	;


\ the following is dubious.
: $c+  \ char str -> nil -- char! )
	\ "appends a single char to the end of string"
	tuck $@ + !   \ ( str -- !char )
	$@ 1 + swap cell - !  \ ( nil -- !offset+1 )
	;

: charpad \ pad a single character into a string
	2 cells roll-pad if
		1 over !
		tuck cell + !
	then
	;

: str,  \ allocate a string into the dictionary
	here >r
	$@ dup cell + allot
	r> $! align
	;

: .$   \ like type
	$@ type 
	;


: $pad  ( buf off -> str ) 
	\ "stores a counted offset into the roll pad"
	dup cell + roll-pad if
		dup >r $! r>	
	else
		cr ." string exceeds pad" 
	then
	;  



: $cat  ( a.str b.str -> a+b.str )
	\ "concatenates two strings into the cat pad"
	swap 2dup @ swap @ cell + + cat-pad if
		dup >r swap $@ rot $! r@       \ store foo
		swap $++ r>       
	else
		cr ." strings exceed cat pad"
	then
	;

: str \ creates a named string.
	create \ ( str -> ,str := 'str' )
	str,
	does>
	;

\ note: we need to natively compile this;
\ the compile time behavior is fundamentally broken. 

:noname 34 parse  $pad  
    ;     
:noname
  34 parse 
  POSTPONE SLiteral 
  POSTPONE $pad  
   ;    interpret/compile: ;"

: #->$ \ convert one cell to a counted string
	0 <# #s #> $pad  ;




