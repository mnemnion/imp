
include ansi.fs

: card& here 128 allot 128 + ; \ -- card <- 

\ head: face skull

\ face: 00!?-???? tttt-tttt ????-?? |

\ skull: AAA 222 333 444 555 666 777 888 999 000 rrr jjj qqq kkk

\  | AA A222-3334 4455-5666 7778-8899 9000-rrrj jjqq-qkkk

\ crease: card@

\ constants

\ ranks

1   constant ¢A
2   constant ¢2
3   constant ¢3
4   constant ¢4
5   constant ¢5
6   constant ¢6
7   constant ¢7
8   constant ¢8
9   constant ¢9
10  constant ¢0
11  constant ¢r
12  constant ¢j
13  constant ¢q
14  constant ¢k

\ major suits

0 constant §lit
1 constant §card
2 constant §book
3 constant §noun
4 constant §str
5 constant §link
6 constant §buff
7 constant §full

: .lit  s" ☷" type ;   
: .card s" ☳" type ;   
: .book s" ☵" type ;   
: .noun s" ☱" type ;   
: .str  s" ☶" type ;   
: .link s" ☲" type ;   
: .buff s" ☴" type ;   
: .full s" ☰" type ;   


: binary  2 base ! ;

: octal   8 base ! ;

\ bytemasks :: not currently used
binary
: ¢Am- 10000000 00000011 ; \ stack order: 
: ¢2m  01110000 ;
: ¢3m  00001110 ;
: ¢4m- 11000000 00000001 ; \ 2-byte masks are low-high
: ¢5m  00111000 ;
: ¢6m  00000111 ;
: ¢7m  11100000 ;
: ¢8m  00011100 ;
: ¢9m- 10000000 00000011 ;
: ¢0m  01110000 ;
: ¢rm  00001110 ;
: ¢jm- 11000000 00000001 ;
: ¢qm  00111000 ;
: ¢km  00000111 ;
decimal

\ bit offsets into head
here 15 cells allot constant ranksets 
-1 ranksets ! \ wasted cell instead of off-by-one index
: ¢k* ;             ' ¢k* ranksets 14 cells + ! 
: ¢q* 3  lshift ;   ' ¢q* ranksets 13 cells + !
: ¢j* 6  lshift ;   ' ¢j* ranksets 12 cells + !
: ¢r* 9  lshift ;   ' ¢r* ranksets 11 cells + !
: ¢0* 12 lshift ;   ' ¢0* ranksets 10 cells + !
: ¢9* 15 lshift ;   ' ¢9* ranksets 9 cells  + ! 
: ¢8* 18 lshift ;   ' ¢8* ranksets 8 cells  + !
: ¢7* 21 lshift ;   ' ¢7* ranksets 7 cells  + !
: ¢6* 24 lshift ;   ' ¢6* ranksets 6 cells  + !
: ¢5* 27 lshift ;   ' ¢5* ranksets 5 cells  + !
: ¢4* 30 lshift ;   ' ¢4* ranksets 4 cells  + !
: ¢3* 33 lshift ;   ' ¢3* ranksets 3 cells  + !
: ¢2* 36 lshift ;   ' ¢2* ranksets 2 cells  + !
: ¢A* 39 lshift ;   ' ¢A* ranksets 1 cells  + !

here 15 cells allot constant rankgets
-1 rankgets
\ head -- suit
: ¢k/ ;                    ' ¢k/ rankgets 14 cells + ! 
: ¢q/ 3  rshift 8 mod ;    ' ¢q/ rankgets 13 cells + !
: ¢j/ 6  rshift 8 mod ;    ' ¢j/ rankgets 12 cells + !
: ¢r/ 9  rshift 8 mod ;    ' ¢r/ rankgets 11 cells + !
: ¢0/ 12 rshift 8 mod ;    ' ¢0/ rankgets 10 cells + !
: ¢9/ 15 rshift 8 mod ;    ' ¢9/ rankgets 9 cells  + ! 
: ¢8/ 18 rshift 8 mod ;    ' ¢8/ rankgets 8 cells  + !
: ¢7/ 21 rshift 8 mod ;    ' ¢7/ rankgets 7 cells  + !
: ¢6/ 24 rshift 8 mod ;    ' ¢6/ rankgets 6 cells  + !
: ¢5/ 27 rshift 8 mod ;    ' ¢5/ rankgets 5 cells  + !
: ¢4/ 30 rshift 8 mod ;    ' ¢4/ rankgets 4 cells  + !
: ¢3/ 33 rshift 8 mod ;    ' ¢3/ rankgets 3 cells  + !
: ¢2/ 36 rshift 8 mod ;    ' ¢2/ rankgets 2 cells  + !
: ¢A/ 39 rshift 8 mod ;    ' ¢A/ rankgets 1 cells  + !



: hd-§-¢--!                   \ head suit rank -- head!
	ranksets swap cells + @   \ head suit verb --
	dup >r 7 swap             \ head suit 7 verb -- verb
	execute invert            \ head suit mask -- verb 
    rot and swap r>           \ head! suit verb -- \ head is zero at offset
	execute or                \ head! -- \ head now has the correct value.
    ;

\ skulpr needs to read each .# off a custom offset and then *! it.
\ that way, the colors will correspond to the suit, not the rank.
\ clearly we need a function to populate such an offset first. 

\ we'll reuse the metadata syntax highligher format for this, so 
\ it won't be on this card. 

: skulpr binary dup 0 
      <# # # # .#g 	     \ king
		 # # # .#c       \ queen
		 # # # .#r       \ jack
		 # # # .#m       \ rogue
		 # # # .#g       \ (1) 0
		 # # # .#c       \ 9
		 # # # .#r       \ 8
		 # # # .#m       \ 7
		 # # # .#g       \ 6
		 # # # .#c       \ 5
		 # # # .#r       \ 4
		 # # # .#m       \ 3
		 # # # .#g       \ 2
		 # # # .#b       \ 1
		 \ end of skull
		 #> decimal type .!
		 ; 

: facepr binary dup 0 
      <# # # # .#g 	     \ king
		 # # # .#c       \ queen
		 # # # .#r       \ jack
		 # # # .#m       \ rogue
		 # # # .#g       \ (1) 0
		 # # # .#c       \ 9
		 # # # .#r       \ 8
		 # # # .#m       \ 7
		 # # # .#g       \ 6
		 # # # .#c       \ 5
		 # # # .#r       \ 4
		 # # # .#m       \ 3
		 # # # .#g       \ 2
		 # # # .#b       \ 1
		 \ end of skull
		 .#!
		 [char] - hold
		 # # #
		 # # # .#di     \ padding

		 #> decimal type .!
		 ; 

\ s" /hold" environment? 130 characters. Fix.	

: ocpr octal dup 0 
      <#  .#! # .#g 	     \ king
			  # .#c        \ queen
			  # .#r        \ jack
			  # .#m        \ rogue
			  # .#g        \ (1) 0
			  # .#c        \ 9
			  # .#r        \ 8
			  # .#m        \ 7
			  # .#g        \ 6
			  # .#c        \ 5
			  # .#r        \ 4
			  # .#m        \ 3
			  # .#g        \ 2
			  # .#b        \ 1
							  \ end of skull
			 [char] - hold .#!
		  binary		  \ back pad 
			 # # #
			 # # # .#di 
	      hex
	      	 [char] - hold .#!
	         # #   .#w          \ persona
	         [char] - hold .#!
	      binary
	         # # # # # .#di .#! \ front pad
	         #   .#r            \ change bit
	         # # .#w .#bo       \ 00 for standard card

		 #> decimal type 
		 ; 

