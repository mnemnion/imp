( stack utilities for Forge )

: $xt? \ ( ?xt -> str flag )
	>name ?dup if 
		name>string $pad true
	else
		s" not found!" $pad false
	then ;

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

variable stackpad

: (dumb-print) \ ( literal stack print )
	0 do
			depth i 1 + - pick \ retrieve a stack value
\ 			dup cr .r . .!
			#->$ #nl $c+       \ Add string pad newline
			stackpad @ $cat    \ pad onto stack
			stackpad !         \ store
		loop
	;

: (smart-print) \ (figurative stack print)
	0 do
		depth i 1 + - pick \ retrieve a stack value
		dup $xt? if
			cr .m ." xt found"
			dup .$ 
			#nl $c+
			stackpad @ $cat
			stackpad !
		else 
			drop
			cr .b ." number"
			dup .
			#->$ #nl $c+
			stackpad @ $cat
			stackpad !
		then
		cr .w .s
	loop .!
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


