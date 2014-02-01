( frame detection )

-1 constant {in}   \ ( := hit-flag )
0  constant {on}   \ ( := hit-flag )
1  constant {out}  \ ( := hit-flag )


: in-between ( in? low high -> hit-flag )
	3dup
		rot tuck = >r = r> or if 
\ 			cr .m ." on" .!
			2drop drop {on}
		else
	rot within if
\ 		cr .b ." in" .! 
		{in}
	else
\ 		cr .r ." out" .!
		{out}
	then
	then

	;

: y-detect ( x y frame -> x frame hit-flag )
	tuck dup 
		xy.frame nip swap
	colrow.frame nip \ ( x frame y y.frame rows.frame )
	over +
\ 	cr .w .s
	in-between 
	;

: x-detect ( x frame -> hit-flag )
	dup 
		xy.frame drop swap
	colrow.frame drop 
	over + 
\ 	cr .y .s
	in-between
	;

: in-on-out? \ ( x y frame -> hit-flag )	
\   cr .cy ." y-detect" cr .s .!
	y-detect 
	dup 0 < if
\ 		cr .b ." x-detect" cr .s .!
		drop x-detect
	else -rot 2drop
	then
	;