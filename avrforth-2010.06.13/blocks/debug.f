( debugging utilities )

: dump1 ( a -- a' ) ] c@+ ] b. ] ;
: dump10 ( a -- a' ) ] cr ] dup ] h. [ $ 10 l ] for ] dump1 ] next ] ;
: dump ( a n -- ) [ $ f l ] + [ $ 4 l ] rshift ] for ] dump10 ] next ] drop ] ;

: idump1 ( ia -- ia' ) ] dup ] i@ ] h. ] 1+ ] ;
: idump8 ( ia -- ia' ) ] cr ] dup ] h. [ $ 8 l ] for ] idump1 ] next ] ;
: idump ( ia n -- ) [ $ 7 l ] + [ $ 3 l ] rshift ] for ] idump8 ] next ] drop ] ;
  
