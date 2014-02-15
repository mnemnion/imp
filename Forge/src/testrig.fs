
\ test includes

\ include ~+/test/vocab.fs
\ include ~+/simulorc/simulorc.fs


include ~+/test/see-and-say.fs
\ end includes

: bye .! bye ;



' .hexframe is [default-display]
' .stack stack-frame set-display.frame 
.windowclear

\ printer tests

include ~+/test/printer-test.fs

\ status dup .frame .clearframe 
\ stack-frame dup .frame .clearframe 
 0 0 .xy

: loupe see-and-say .! chill ;
