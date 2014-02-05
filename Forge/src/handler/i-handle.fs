( Input Handlers )


: (i-handle-default) \ ( frame -> frame false )
	cr .b ." no attached i-handle" .!
	false
	;

: (i-handle-next)  \ ( frame subject -> nil )
	cr .r ." add a next" .!
	\ 2drop 
	;

: ooblick ." oo-blick! " ;

: (i-handle), \ ( nil -> ,i-handle )
	['] (i-handle-default) , \ ( ,xt )
	['] (i-handle-next)    , \ ( ,xt )
	;

: i-handle, \ \ does next -> := i-handle )
	here >r
	swap , , r> @ ;

: i-handle \ ( i-handle -> nil )
	\ "calls an i-handle"
	dup >r perform 
	r> cell + perform
	;

: i-handle: 
	create (i-handle),
	does> i-handle
	 ;