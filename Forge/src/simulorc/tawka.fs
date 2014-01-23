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

: unspaz
	\ "advance past spacemarks"
	\ 'macro' ( 'grunt' ) ?
	over 				\ ( a b a --       )
	32 = if         \ first byte was a space
\ 		cr ." space"
		nip         \ drop it
		half-heer	\ get another 
		recurse	    \ grok
	else then ;

: heer 
	\ "get a word from input.""
	key byte>cha if 
	    key byte>cha if else 
			half-heer
		then
	else recurse then ;

: numba? \ ( cha -> flag )
	\ " is the cha a numba?"
	dup 97 103 within >r \ a-f
	    48 58  within r> \ 0-9
	\ cr .cy .s .!
	or ;

: numba-ta-byte \ ( numba -> byte )
	\ "change one numba to a byte"
	dup 97 103 within if \ convert alphas
		87 - 
	else
		48 -
	then ;

: numba-one \ ( numba cha -> < byte true > | < 0 false > )
	\ "pushes one numba onto the stack, if"
	\ "the top byte is also a numba"
	dup 32 = if 
		swap numba-ta-byte swap true
	else dup numba? if 
\ 			cr .y ." good numba!"
			numba-ta-byte
			swap
			numba-ta-byte
			16 * + true
		else
			cr .r ." bad numba >.<"
			2drop false    \ Orcs ignore bullshit of all sorts
		then
	then ;

: numbaz 
	\ "tries to make up to once cell from up to 4 chaz"
	numba-one if
		dup 32 = if \ 1 sig fig
			drop
		else
			half-heer
	\ 		cr .g .s
			dup 32 = if \ 2 sig fig
				drop
			else
				half-heer
				over numba? if 
					numba-one if
						dup 32 = if \ 3 sig fig
							drop 
							swap 16 * +
						else            \ 4 sig fig
							swap 256 * +
						then
					then
				else
					cr .r ." bad numba2 >.<"
				 	2drop
				then
			then
		then
	then
	;
 
variable (liver) 126 cells allot
\ holds the execution tokens for the liva
\ and some padding because we're simulating
\ normally the liva table resides in the spleen
\ because it's byte-addressable

: nope-nope-nope
	cr .di .w ." nope" .! ;

:noname (liver) 127 0 do
		dup i cells + 
		['] nope-nope-nope swap ! loop ; execute  \ stuff the liver with no-ops

: liva \ ( letta -> `effect` )
	\ "process a liva word"
	dup
		(liver) swap cells + perform
		\ report success
	;

: bakpak \ "process a bakpak word" 
	;

: lettaz \ "parse words"
	dup 32 = if \ liva word
		cr .cy ." liva!"
		liva
	else
		cr .m ." bakpak!"
		bakpak
	then ;


: grk \ "comprehend a werd"
	
	over numba? if
		numbaz
	else
		cr .g ." letta!"
		lettaz
	then
 	cr .cy .s .!

	;

: grok 
	heer unspaz grk .! ;





