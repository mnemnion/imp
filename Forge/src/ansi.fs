
include vt100.fs

decimal

\ control code

: .^ 27 emit [char] [ emit ; 

: hide-cursor 
    .^ ." ?25l" ;

: show-cursor
    .^ ." ?25h" ;

\ colors
: .!  .^ ." 0m"      ;
: .r  .^ ." 31m"     ;
: .g  .^ ." 32m"     ;
: .y  .^ ." 33m"     ;   
: .b  .^ ." 34m"     ;
: .m  .^ ." 35m"     ;
: .c  .^ ." 36m"     ;
: .w  .^ ." 37m"     ;
: .bo .! .^ ." 1m"   ;
: .di .! .^ ." 2m"   ;
: .un .! .^ ." 4m"   ;
: .in .! .^ ." 7m"   ;
: .bu .! .^ ." 1;4m" ;
: .ib .! .^ ." 1;7m" ; 
: .fu .! .^ ." 5;7m" ;

\ color strings

: .!"   s\" \e[0m"        ;
: .r"   s\" \e[31m"       ;
: .g"   s\" \e[32m"       ;
: .y"   s\" \e[33m"       ;
: .b"   s\" \e[34m"       ;
: .m"   s\" \e[35m"       ;
: .c"   s\" \e[36m"       ;
: .w"   s\" \e[37m"       ;
: .bo"  s\" \e[0m\e[1m"   ;
: .di"  s\" \e[0m\e[2m"   ;
: .un"  s\" \e[0m\e[4m"   ;
: .in"  s\" \e[0m\e[7m"   ;
: .bu"  s\" \e[0m\e[1;4m" ;
: .ib"  s\" \e[0m\e[1;7m" ;
: .fu"  s\" \e[0m\e[5;1m" ;

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

\ cursor control \ x -- ""
: .fwd  .^  dec. ." C" ;
: .up   .^  dec. ." A" ;
: .down .^  dec. ." B" ;
: .back .^  dec. ." D" ; 
: .xy   .^ swap dec. [char] ; emit dec. ." f" ;
: .save 27 emit ." 7" ;
: .restore 27 emit ." 8" ;

\ Boxes

\ oldschool!

: .crunk 27 emit  ." )0" ;
: .sanity 27 emit ." )B" ;

\ unicode
: .|ul ." ┏" ; : .|ut ." ┳" ; : .|ur ." ┓" ;
: .|ml ." ┣" ; : .|mt ." ╋" ; : .|mr ." ┫" ;
: .|ll ." ┗" ; : .|lt ." ┻" ; : .|lr ." ┛" ;
: .|uli ." ┌" ; : .|uti ." ┬" ; : .|uri ." ┐" ;
: .|mli ." ├" ; : .|mti ." ┼" ; : .|mri ." ┤" ;
: .|lli ." └" ; : .|lti ." ┴" ; : .|lri ." ┘" ;
: .|ulr ." ╭" ; : .|urr ." ╮" ; 
: .|llr ." ╰" ; : .|lrr ." ╯" ;
: .|uld ." ╔" ; : .|utd ." ╦" ; : .|urd ." ╗" ;
: .|mld ." ╠" ; : .|mtd ." ╬" ; : .|mrd ." ╣" ;
: .|lld ." ╚" ; : .|ltd ." ╩" ; : .|lrd ." ╝" ;
: .|| ." ┃"  ; : .||i ." │" ; : .||d ." ║"  ;
: .|- ." ━"  ; : .|-i ." ─" ; : .|-d ." ═" ;
: .|... ." ┈" ; : .|::: ." ┊" ;

: |innerbox \ ( rows cols -- rows-1 cols -1 )
    dup 3 < if 
        drop 1
    else
        1 -
    then swap 
    dup 3 < if 
        drop 1
    else 
        1 -
    then  ;

: .|box \  ( cols rows -> "box" ) 
        swap |innerbox dup 
        .|ul 0 do .|- loop .|ur 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .|| .fwd .|| 
        loop dup 2 + .back 1 .down
        .|ll 0 do .|- loop .|lr  
        2drop
        ;
: .|boxi
        |innerbox dup 
        .|uli 0 do .|-i loop .|uri 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||i .fwd  .||i 
        loop dup 2 + .back 1 .down
        .|lli 0 do .|-i loop .|lri  
        ;
: .|boxd
        |innerbox dup 
        .|uld  0 do .|-d  loop .|urd  
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||d  .fwd  .||d  
        loop dup 2 + .back 1 .down
        .|lld  0 do .|-d  loop .|lrd   
        ;
: .|rbox
        |innerbox dup 
        .|ulr 0 do .|-i loop .|urr 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||i .fwd  .||i 
        loop dup 2 + .back 1 .down
        .|llr 0 do .|-i loop .|lrr  
        ;

: .|dashbox
        |innerbox dup 
        .|uli  0 do .|...  loop .|uri  
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .|:::  .fwd  .|::: 
        loop dup 2 + .back 1 .down
        .|lli  0 do .|...  loop .|lri   
        ;
: .|wipe \ ( rows cols -- "pane" )
        |innerbox dup             \ ( row cols cols -- )
        ." *" 0 do ." *" loop 
        0 do 
            dup dup
                1 + .back 1 .down 
            0 do 
                ." *" loop 
            ." *"
        loop drop
        ;

: .\n cr cr 1 .up ;

: .tall \ str -- "str"  \ needs .\n from the repl
  2dup .^ ." #3" type 
  dup .back 1 .down
  .^ ." #4" type .! cr
 ; 
