( stack utilities for Forge )

: .xt-name \ xt -- <'name'|"not found!">
	>name ?dup if 
		name>string type
	else
		." not found!"
	then ;