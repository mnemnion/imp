
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

\ handy printers

: .bin ['] . 2 base-execute ;