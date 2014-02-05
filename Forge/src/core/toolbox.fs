
: ` comp' postpone, ; immediate

: .. clearstack ;

: keys 0 do key loop ;

: emits 0 do emit loop ;

: 3dup \ chuck forgive me
	2 pick 2 pick 2 pick ;

: 3drop 2drop drop ;

: 3dup-alt
	rot dup >r
	rot dup >r
	rot dup >r
	r> r> swap r> -rot
	;

: printable? 
	32 127 within ;

: alpha?
	dup 
		65 91 within >r
		97 123 within r>
	or
	;

: .left? ( count -- nil )
	1 + depth <> if
		cr .r ." leftovers: "
	    .s .!
	then ;