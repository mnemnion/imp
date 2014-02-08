( Ansi Incompatibility Layer )

: ` comp' postpone, ; immediate

: .. clearstack ;

: keys 0 do key loop ;

: emits 0 do emit loop ;

: drops 0 do drop loop ;

: 3dup \ chuck forgive me
	2 pick 2 pick 2 pick ;

: 3drop 2drop drop ;

\ default case words

: otherwise ` [ ` ' ` drop ` ] ` literal ; 
: endother false ;

\ consistent compiling words

: var: create 0 , ;
: const: (constant) , ;
: val: (value) , ;

