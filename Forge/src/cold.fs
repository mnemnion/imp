\ include ./compat/all.fs

[ifundef] possibly

: possibly  ( "name" -- )  
	bl word find  
	?dup and if  
		execute then ;
		
: anew ( "name" -- )( run: -- )  
	>in @ possibly  
	>in ! marker ;

: (colder)
	s" cold.fs" included ;

defer [wump]

: chill
	[wump]
 	(colder) 
 	bootmessage
	;

[then]

anew (smasher)

' (smasher) is [wump]

include warm.fs