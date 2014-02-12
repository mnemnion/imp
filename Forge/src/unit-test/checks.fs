( Core Testing Suite )

require ~+/xterm/xterm.fs

\ aka man I need some unit tests or I'm never getting the better tools written

variable checkpad

variable passpad

variable failpad

null-str passpad !
null-str failpad !

: fail!
	#nl charpad swap $cat
	failpad @ $cat
	$ham failpad !
	;

: pass!
	#nl charpad swap $cat
	passpad @ $cat
	$ham passpad !
	;

: check!
	$ham checkpad ! 
	;

: (cat-and-perform)
	dup $xt? if
		check! 
		checkpad @ bl $c+
		check!
		execute
	else 
		cr .r ." bad xt!"
		2drop
	then
	;

.g" $pad s" ✓" $pad $cat $.! $cat str: green-check

.r" $pad s" ✗" $pad $cat $.! $cat str: red-check

: ($check)
 	if
 		checkpad @ 
		green-check $cat
		check! true >r
	else
		checkpad @ 
		red-check $cat
		check! false >r
	then
	checkpad @ r>
	;

: $check? \ ( xt tos? -> < yes-str | no-str > )
	>r (cat-and-perform) r> = 
	($check) 
	;

: 2$check? 
	2>r (cat-and-perform) 2r>
	rot = ($check) nip >r
	= ($check) r> and
	;

: test  \ ( mu xt tos? -> nil -- < !pass | !fail > )
	$check? if
		pass!
	else
		fail!
	then
	;

: 2test \ ( mu xt nos? tos? -> nil < !pass | !fail > )
	2$check? if
		pass!
	else
		fail!
	then
	;

: passing passpad @ ; 
: failing failpad @ ;
: .stats passing .$ failing .$ ;


