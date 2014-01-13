\ adder.fs

\ adds those dadgum arrays

\ lovingly handcrafted arrays

variable foo[] 2 cells allot
variable bar[] 2 cells allot
variable baz[] 2 cells allot

99 99 99 foo[] ! foo[] cell + ! foo[] 2 cells + !
2 4 8    bar[] ! bar[] cell + ! bar[] 2 cells + !
16 32 64 baz[] ! baz[] cell + ! baz[] 2 cells + !

: add-advance  \ ( a1 a2 a3 -- a1+ a2+ a3+ )
	\ "add a2 and a3, store value in a1. Advance all three addresses."
	
	dup >r -rot dup >r -rot dup >r -rot \ ( a1 a2 a3 -|- a1 a2 a3 )
	@ swap @ +                          \ ( a1 a2+a3 -|- a1 a2 a3 )
	swap !                              \ ( nil -|- a1 a2 a3      )
	r> r> r>					        \ ( a1 a2 a3    --        )
	cell + rot cell + rot cell + rot    \ ( a1+ a2+ a3+ --        )
	; 

: n-adder \ ( create: n -> nil does>: `add-advance` )
	create , 
	does>
	@ 0 do add-advance loop
	drop 2drop ;

3 n-adder 3-adder

\ foo[] bar[] baz[] 3-adder  ok
\ foo[] @ . 72  ok         
\ foo[] 1 cells + @ .  36  ok
\ foo[] 2 cells + @ .  18  ok 
