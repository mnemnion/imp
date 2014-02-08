	\ colors

	: .!  .^ ." 0m"      ;
	: .r  .^ ." 31m"     ;
	: .g  .^ ." 32m"     ;
	: .y  .^ ." 33m"     ;   
	: .b  .^ ." 34m"     ;
	: .m  .^ ." 35m"     ;
	: .cy .^ ." 36m"     ;
	: .w  .^ ." 37m"     ;
	: .bo .! .^ ." 1m"   ;
	: .di .! .^ ." 2m"   ;
	: .un .! .^ ." 4m"   ;
	: .in .! .^ ." 7m"   ;
	: .bu .! .^ ." 1;4m" ;
	: .ib .! .^ ." 1;7m" ; 
	: .fu .! .^ ." 5;7m" ;


: .!"   s\" \e[0m"        ;
: .r"   s\" \e[31m"       ;
: .g"   s\" \e[32m"       ;
: .y"   s\" \e[33m"       ;
: .b"   s\" \e[34m"       ;
: .m"   s\" \e[35m"       ;
: .cy"   s\" \e[36m"      ;
: .w"   s\" \e[37m"       ;
: .bo"  s\" \e[0m\e[1m"   ;
: .di"  s\" \e[0m\e[2m"   ;
: .un"  s\" \e[0m\e[4m"   ;
: .in"  s\" \e[0m\e[7m"   ;
: .bu"  s\" \e[0m\e[1;4m" ;
: .ib"  s\" \e[0m\e[1;7m" ;
: .fu"  s\" \e[0m\e[5;1m" ;

.!" $pad str: $.!

\ control holds for number printing
: .#[e  [char] [ hold 27 hold ;
: .#_  32 hold ;
:  #ℳ  [char] m hold  ;
: .#! #ℳ [char] 0 hold .#[e ;
: .#r #ℳ [char] 1 hold [char] 3 hold .#[e ;
: .#g #ℳ [char] 2 hold [char] 3 hold .#[e ;
: .#y #ℳ [char] 3 hold [char] 3 hold .#[e ;
: .#b #ℳ [char] 4 hold [char] 3 hold .#[e ;
: .#m #ℳ [char] 5 hold [char] 3 hold .#[e ;
: .#c #ℳ [char] 6 hold [char] 3 hold .#[e ;
: .#w #ℳ [char] 7 hold [char] 3 hold .#[e ;
: .#bo #ℳ [char] 1 hold .#[e ;
: .#di #ℳ [char] 2 hold .#[e ;
: .#un #ℳ [char] 4 hold .#[e ;
: .#in #ℳ [char] 7 hold .#[e ;
: .#bu #ℳ [char] 4 hold [char] ; hold [char] 1 hold .#[e ; \ oh syntax highlighting
: .#ib #ℳ [char] 7 hold [char] ; hold [char] 1 hold .#[e ;