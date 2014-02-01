( frame detection )

-1 constant {in}   \ ( := hit-flag )
0  constant {on}   \ ( := hit-flag )
1  constant {out}  \ ( := hit-flag )


: in-btw 
	2dup = if 
		cr .m ." on" 
		2drop {on}
	else > if
		cr .m ." less than" 
		{in}
	else
		cr .m ." grt than"
		2drop {out}
	then then
	;

: in-between ( in? low high -> hit-flag )
	rot tuck 
	cr .b .s
	in-btw
	dup 0 < if
		cr .g ." testing low"
		drop
		cr .s
		swap in-btw
	then
	 .!
	;

: y-detect ( x y frame -> x frame hit-flag )
	tuck dup 
		xy.frame nip swap
	colrow.frame nip \ ( x frame y y.frame rows.frame )
	over +
	cr .w .s
	in-between 
	;

: x-detect ( x frame -> hit-flag )
	dup 
		xy.frame drop swap
	colrow.frame drop 
	over + 
	cr .y .s
	in-between
	;

: in-on-out? \ ( x y frame -> hit-flag )
	dup dup .frame .clearframe
	-rot 2dup .xy* rot
	cr .cy .s .!
	y-detect 
	dup 0 < if
		drop x-detect
	else -rot 2drop
	then
	;