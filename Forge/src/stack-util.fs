( stack utilities for Forge )

: .xt-name \ xt -- <'name'|"not found!">
	>name ?dup if 
		name>string type
	else
		." not found!"
	then ;

: .. clearstack ;

: .sp [char] < emit depth bl . [char] > emit  depth 0 do depth i - dup pick . loop ;