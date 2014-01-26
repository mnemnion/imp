( basic terminal handling )

require xterm-core.fs	

\ cursor control \ x -- ""



: .fwd  .^  dec. ." C" ;
: .up   .^  dec. ." A" ;
: .down .^  dec. ." B" ;
: .back .^  dec. ." D" ; 
: .save 27 emit ." 7" ;
: .restore 27 emit ." 8" ;
\ screen clear
: .page .^ ." 2J" ;

variable last-xy 1 cells allot 
1 1 last-xy 2!

: .xy   
	2dup last-xy 2!
	.^ swap dec. [char] ; emit dec. ." f" ; 