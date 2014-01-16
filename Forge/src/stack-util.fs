( stack utilities for Forge )

: .xt-name \ xt -- <'name'|"not found!">
	>name ?dup if 
		name>string type
	else
		." not found!"
	then ;

: .. clearstack ;

: .sp 
	depth dup 0 <> if 
		[char] < emit
		0 <# #s #> type 
		[char] > emit bl emit 
		depth 0 do 
			depth i 1 + - 
			pick . 
		loop
	else 
		[char] > [char] 0 [char] < 
		emit emit emit
	then ;