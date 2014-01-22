\ Da Tawka

( # Da Tawka 

this is how native orcs speak Orcish. 

Orcish is moderately complex. So is AVR Assembler.

We should have a solid idea of how the former works before trying to squeeze it down.

Also if some of the ideas are looking expensive, we can drop them here. 

      )

: vocab-table ( -- )
    table create ,
does> ( -- )
    \ replaces the wordlist on the top of the search list with the
    \ vocabulary's wordlist
    @ >r
    get-order dup 0= -50 and throw \ search-order underflow
    nip r> swap
    set-order ;

get-current

vocab-table Orcish

also Orcish definitions

: WAGI! ." Orc! Orc! Orc!" ;

set-current previous


( ## Da Tawka, Propa 
	
	Our Orc is a simple creature. Most of his brain is dedicated to understanding a rough and ready sort of speech,
	and responding intelligibly. 

	The first thing an Orc does with a byte is `heer` it. This simply involves ignoring the byte if it happens to be out of the ASCII
	printing range. Orcs do not typically respond when an error happens; this is silent behavior. 

	Orcs heer two bytes at a time, meaning the second one is on the stack. The first test: is the first byte a space? If so, it
	is nipped, and we pull another byte. This will get us word-aligned, consuming all extraneous spacemarks. 

	Note: Orcs have never heard of a tab, let alone a vertical tab. 

	The second test: is the second byte a space? If so, we have either a liva word or a numba. We flip the 'liva' flag and parse the
	first byte accordingly.

	If the second byte is not a space, we swap and test the first byte for numba status. If it's a numba, we try to make both bytes into
	a numba and immediately try to pull another key if successful. If we keep getting numbas, we make numbas, 16 bits at a time, and push
	to stack. If we get a space, we succeed: if we get a letta, we fucked up. Scrub everything and glare silently. 

	If the first byte received is a letta, we look it up in the bakpak, because it might itself consume keys. If we find it, we echo 
	` \ hex \ `, where hex is the hexidecimal value of the two bytes. \ is our ignore word, and lasts until the next \. This means no 
	additional logic is needed to make a dumb Orc ignore replies from another dumb Orc. 

)

: byte>cha \ ( byte -> < := cha true | false > -- := ?cha )
	\ "filters out non-printable ascii"
	\ 'grunt'
	dup 32 126 within if dup else drop 0 then ;

: half-heer 
	\ 'macro'
	key byte>cha if else recurse then ;

: heer 
	\ "get a word from input.""
	key byte>cha if 
	    key byte>cha if else 
			half-heer
		then
	else recurse then ;

: unspaz
	\ "advance past spacemarks"
	\ 'macro' ( 'grunt' ) ?
	over 				\ ( a b a --       )
	32 = if         \ first byte was a space
		cr ." space"
		nip         \ drop it
		half-heer	\ get another 
		recurse	    \ grok
	else then
	;



: grok \ "comprehend a werd"
	heer unspaz
	cr .cy .s .!
	cr swap emit emit ;







