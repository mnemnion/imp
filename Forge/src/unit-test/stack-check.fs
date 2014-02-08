( Stack checks )

var: checkpad
null-str checkpad !


: stackcheck!
	$ham checkpad !
	;

: (stack-pad-check)
	dup $xt? if
		stackcheck! 
		checkpad @ bl $c+
		stackcheck!
		execute
	else 
		cr .r ." bad xt!"
		2drop
	then
	;

: $stackcheck?
	depth 2 - \ variant and xt
	+ >r 
	(stack-pad-check)
	depth r> -
	dup 0 = if
		checkpad @
		green-check $cat
		stackcheck!
		true
	else
		checkpad @
		red-check $cat
		bl $c+ swap
		signed>$ $cat
		stackcheck!
		false
	then
	checkpad @
	swap
	;

: stack-test \ ( mu xt difference -> mu < !pass | !fail > )
	$stackcheck?
	if
		172 xterm-fg 
		s" stack: " $pad $cat $.! $cat
		swap $cat pass!
	else 
		179 xterm-fg
		s" stack: " $pad $cat $.! $cat
		swap $cat fail!
	then
	;


