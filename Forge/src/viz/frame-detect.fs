( frame detection )

-1 constant {in}   \ ( := hit-flag )
0  constant {on}   \ ( := hit-flag )
2  constant {out}  \ ( := hit-flag )

: in-between ( in? low high -> hit-flag )
	3dup
		rot tuck = >r = r> or if 
\ 			cr .m ." on" .!
			2drop drop {on}
		else
	1 + rot within if
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

: in-on-out? 
	y-detect >r 
	x-detect r>
	+	
	dup 0 > if
		drop {out}
		else dup -2 = if 
			drop {in}
		else 
			drop {on}
	then then
	.!
	;

