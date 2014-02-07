
: ` comp' postpone, ; immediate

: .. clearstack ;

: keys 0 do key loop ;

: emits 0 do emit loop ;

: drops 0 do drop loop ;

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

: csi-end?
	64 127 within ;


: alpha?
	dup 
		65 91 within >r
		97 123 within r>
	or
	;

: utf-lead? \ ( utf-lead? -> flag )
	194 245 within ;

: utf-bytes? \ ( utf-lead -> flag )
	dup 194 224 within if
		drop 2
	else dup
		224 240 within if
			drop 3
		else
			drop 4
	then then
	;

\ default case words

: otherwise ` [ ` ' ` drop ` ] ` literal ; 
: endother false ;

\ handy printers

: .bin ['] . 2 base-execute ;