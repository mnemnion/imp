
include vt100.fs

decimal

\ control code

: .^ 27 emit ; \ cursor control

\ environment
: @xy? \ -- pair!
       here form               \  adr x y -- 
       here 2 cells allot 2!   \  adr! --
       ;

\ colors
: .!  .^ ." [0m"      ;
: .r  .^ ." [31m"     ;
: .g  .^ ." [32m"     ;
: .y  .^ ." [33m"     ;   
: .b  .^ ." [34m"     ;
: .m  .^ ." [35m"     ;
: .c  .^ ." [36m"     ;
: .w  .^ ." [37m"     ;
: .bo .! .^ ." [1m"   ;
: .di .! .^ ." [2m"   ;
: .un .! .^ ." [4m"   ;
: .in .! .^ ." [7m"   ;
: .bu .! .^ ." [1;4m"    ;
: .ib .! .^ ." [1;7m" ; 
: .fu .! .^ ." [5;7m"    ;

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
: .fwd  .^ ." [" . ." C" ;
: .up   .^ ." [" . ." A" ;
: .down .^ ." [" . ." B" ;
: .back .^ ." [" . ." D" ; 

\ boxes
: .|ul ." ┏" ; : .|ut ." ┳" ; : .|ur ." ┓" ;
: .|ml ." ┣" ; : .|mt ." ╋" ; : .|mr ." ┫" ;
: .|ll ." ┗" ; : .|lt ." ┻" ; : .|lr ." ┛" ;
: .|uli ." ┌" ; : .|uti ." ┬" ; : .|uri ." ┐" ;
: .|mli ." ├" ; : .|mti ." ┼" ; : .|mri ." ┤" ;
: .|lli ." └" ; : .|lti ." ┴" ; : .|lri ." ┘" ;
: .|uld ." ╔" ; : .|utd ." ╦" ; : .|urd ." ╗" ;
: .|mld ." ╠" ; : .|mtd ." ╬" ; : .|mrd ." ╣" ;
: .|lld ." ╚" ; : .|ltd ." ╩" ; : .|lrd ." ╝" ;
: .|| ." ┃"  ; : .||i ." │" ; : .||d ." ║"  ;
: .|- ." ━"  ; : .|-i ." ─" ; : .|-d ." ═" ;

: .|box \  y x --  \ do in-place eventually { no cr }
        dup 
        .|ul 0 do .|- loop .|ur 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .|| .fwd .|| 
        loop dup 2 + .back 1 .down
        .|ll 0 do .|- loop .|lr  
        ;
: .|boxi
        dup 
        .|uli 0 do .|-i loop .|uri 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||i .fwd  .||i 
        loop dup 2 + .back 1 .down
        .|lli 0 do .|-i loop .|lri  
        ;
: .|boxd
        dup 
        .|uld  0 do .|-d  loop .|urd  
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||d  .fwd  .||d  
        loop dup 2 + .back 1 .down
        .|lld  0 do .|-d  loop .|lrd   
        ;

: .\n cr cr 1 .up ;

: .tall \ str -- "str"  \ needs .\n from the repl
  2dup .^ ." #3" type 
  dup .back 1 .down
  .^ ." #4" type .! cr
 ; 
