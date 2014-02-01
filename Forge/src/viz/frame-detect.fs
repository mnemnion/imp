( frame detection )

-1 constant {in}   \ ( := hit-flag )
0  constant {on}   \ ( := hit-flag )
1  constant {out}  \ ( := hit-flag )

: in-on-out? \ ( x y frame -> hit-flag )
	dup dup .frame .clearframe
	-rot 2dup .xy* rot
	;