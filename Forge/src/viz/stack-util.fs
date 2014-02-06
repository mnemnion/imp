( stack utilities for Forge )

: $xt? \ ( ?xt -> str flag )
	>name ?dup if 
		name>string $pad true
	else
		s" not found!" $pad false
	then ;

: $var?
	2 cells - >name ?dup if
		name>string $pad true
	else 
	  s" nottavar!" $pad false
	then
	;

: $var
	$var? drop ;

: $xt $xt? drop ;

: .xt $xt .$ ;

: .sp 
	depth dup 0 <> if 
		[char] < emit
		0 <# #s #> type 
		[char] > emit bl emit 
		depth 0 do 
			depth i 1 + - 
			pick . 
		loop
	else 
		[char] > [char] 0 [char] < 
		3 emits drop
	then ;

variable stackpad 128 cells allot

: (dumb-print) \ ( literal stack print )
	0 do
			depth i 1 + - pick \ retrieve a stack value
\ 			dup cr .r . .!
			#->$ #nl $c+       \ Add string pad newline
			stackpad @ $cat    \ pad onto stack
			stackpad !         \ store
		loop
	;

: (pad-stack)
	#nl $c+
	stackpad @ $cat
	stackpad !
	drop
	;

: (smart-print) \ (figurative stack print)
	0 do
		depth i 1 + - pick \ retrieve a stack value
		dup $xt? if
\ 			cr .m ." xt found"
\ 			dup .$ 
			(pad-stack)
		else 
			drop
			dup $var? if
				(pad-stack)
			else
				drop
				#->$ #nl $c+
				stackpad @ $cat
				stackpad !
		then then
\ 		cr .w .s
	loop 
	;

: $s \ "turn stack into (literal), \n separated string"
	#nl charpad stackpad !
	depth dup 
	0 <> if
		(dumb-print)
	else
		drop
		s" zero stack" $pad stackpad !
	then
	stackpad @
	;

: $smart \ "turn stack into (literal), \n separated string"
	#nl charpad stackpad !
	depth dup 
	0 <> if
		(smart-print)
	else
		drop
		s" zero stack" $pad stackpad !
	then
	stackpad @
	;

defer stack-fr

: stack-handler \ ( buf off -> nil -- "str")
	stack-fr .printframe
	;

: .stack

	$smart $@ stack-handler ;

: (stack-title)
	depth >r \ before we stuff the stack...
	.g
	s" Stack: " $pad 
	r> #->$ $cat bl $c+
	$@ stack-fr .title .!
	;

: .ss 
	(stack-title)
	.stack
	;

: .left? ( count -- nil )
	1 + depth <> if
		cr .r 
		s" Leftovers: "
		stack-fr .title
	    .stack .!
	    .s
	then ;


