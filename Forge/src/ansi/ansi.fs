include core.fs

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
        swap |innerbox dup 
        .|uli 0 do .|-i loop .|uri 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||i .fwd  .||i 
        loop dup 2 + .back 1 .down
        .|lli 0 do .|-i loop .|lri  
        ;
: .|boxd
        swap |innerbox dup 
        .|uld  0 do .|-d  loop .|urd  
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||d  .fwd  .||d  
        loop dup 2 + .back 1 .down
        .|lld  0 do .|-d  loop .|lrd   
        ;
: .|rbox
        swap |innerbox dup 
        .|ulr 0 do .|-i loop .|urr 
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .||i .fwd  .||i 
        loop dup 2 + .back 1 .down
        .|llr 0 do .|-i loop .|lrr  
        ;

: .|dashbox
        swap |innerbox dup 
        .|uli  0 do .|...  loop .|uri  
        2dup swap \ y x x y --
        0 do dup dup
        2 + .back 1 .down .|:::  .fwd  .|::: 
        loop dup 2 + .back 1 .down
        .|lli  0 do .|...  loop .|lri   
        ;

( Test -- Remove )

: offset-hexpr ( offset n -- new-offset )

 tuck                            \ ( n offset n -- )
 hex 0 do                        \ ( n -- `hex`    )
    dup i + 16 mod               \ ( n n+i%16 --   )
    dup 15 <> if                 \ ( n n2     --   )
        0 <# # #> type           \ ( n -- "n2"     )
    else \ red F
        0 <# .#! # .#r #> type   \ ( n -- "n2"     )
    then
 loop decimal                    \ ( n -- `decimal` ) 
 + 16 mod                        \ ( new-offset --  )
 ;

 : hexer  \ ( C: nil -> nil D: nil -> nil "hex" )
    create \ ( nil -> nil )
        0 ,
    does>  \ ( nil -> nil )
    dup >r @ swap offset-hexpr r> ! ;

 hexer rollhex 

( / Test )

: .|wipe \ ( rows cols -- "pane" )
        |innerbox                   \ ( row col --  )
        .di .w                      \ ( "ansi"      )
        0 do                        \ ( row     --  )
            dup 0 do ." *" loop     \ ( row -- "*+" )
            dup .back 1 .down       \ ( row     --  )
        loop
        drop 
        .!
        ;

: .\n cr cr 1 .up ;

: .tall \ str -- "str"  \ needs .\n from the repl
  2dup .^ ." #3" type 
  dup .back 1 .down
  .^ ." #4" type .! cr
 ; 
