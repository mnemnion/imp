( Input Handlers )


: (i-handle-default) \ ( frame -> frame false )
	cr .b ." no attached i-handle" .!
	false
	;

: (i-handle-next)  \ ( frame subject -> nil )
	cr .r ." end of handle" .!
	2drop ;

: i-handle, \ ( nil -> ,i-handle )
	['] (i-handle-default) , \ ( ,xt )
	['] (i-handle-next)    , \ ( ,xt )
	;

: i-handle: 
	create i-handle,
	 ;

: i-handle \ ( i-handle -> nil )
	\ "calls an i-handle"
	dup >r perform 
	r> cell + perform
	;