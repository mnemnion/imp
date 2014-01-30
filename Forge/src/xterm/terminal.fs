( basic terminal handling )

require xterm-core.fs	

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

: ascii-num? \ ( char ->  flag )
	\ "tests a byte for numeracy in ASCII terms"
	48 58 within ;

: ascii>num \ ( char -> byte )
	\ "convert a single ASCII char 0-9 to its value"
	48 - ;

: [;]? dup 59 = ;

: control-to-num \ ( mu -> num )
	\ "converts up to 4 chars to a number"
	\ "stops at, and consumes semicolons"
	\ first byte
	dup ascii-num? if
		ascii>num swap
		[;]? if 
			drop 
		else
			dup ascii-num? if
				ascii>num 10 * + swap
			else [;]? if drop then
			then
			dup ascii-num? if
				ascii>num 100 * + swap
			else [;]? if drop then
			then
			dup ascii-num? if
				ascii>num 1000 * + swap
			else [;]? if drop then
			then
		then
	then 
	[;]? if drop then
	 ;

: xterm-accept \ ( char -> mu ) unknown # of bytes
	>r
	\ "accept an escape sequence returned by terminal environment"
	\ "trims esc proper and the end of sequence char"
	key dup 27 = if \ esc
		drop 
		begin
			key dup r@ = 
		until
		r> 2drop
	else r> 2drop then
 ;

 : form .^ 18 dec. ." t" [char] t xterm-accept 
 	control-to-num >r
 	control-to-num nip nip r> 
 ;

 : whereami \ ( nil -> rows cols )
	\ "gets cursor position"
	.^ ." 6n" [char] R xterm-accept
	control-to-num >r
	control-to-num drop r>
	; 

: rows form drop ; : cols form nip ; 

: row? whereami drop ; : col? whereami nip ;

: mouse-on
	\ "turn on mouse handling"
	.^ ." ?1000h" ;

: mouse-off
	\ "turn off mouse handling"
	.^ ." ?1000l" ;


















