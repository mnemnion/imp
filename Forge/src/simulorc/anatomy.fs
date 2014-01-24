( Orcish Anatomy )


variable (liver) 126 cells allot
\ holds the execution tokens for the liva
\ and some padding because we're simulating
\ normally the liva table resides in the spleen
\ because it's byte-addressable

: nope-nope-nope
	cr .m ." nope " .! ;

:noname (liver) 127 0 do
		dup i cells + 
		['] nope-nope-nope swap ! loop ; execute  \ stuff the liver with no-ops