#Core Loop

Primitives end in .next, which in jonesforth is a macro that takes two instructions. EXIT is a single-instruction jump to the inner interpreter, found at the end of defined words. 

I need to know what AmForth .next looks like. BRB.

Well, here's the AmForth core loop:

```asm
; the inner interpreter.

DO_COLON:
    push XH
    push XL          ; PUSH IP
    movw XL, wl
    adiw xl, 1
DO_NEXT:
    brts DO_INTERRUPT
    movw zl, XL        ; READ IP
    readflashcell wl, wh
    adiw XL, 1        ; INC IP

DO_EXECUTE:
    movw zl, wl
    readflashcell temp0,temp1
    movw zl, temp0
    ijmp

DO_INTERRUPT:
    ; here we deal with interrupts the forth way
    clt
    ldi wl, LOW(XT_ISREXEC)
    ldi wh, HIGH(XT_ISREXEC)
    rjmp DO_EXECUTE

 ```

 OKay, so they're all subroutines. I wonder if we can eliminate the rjmp at the end of DO_EXECUTE just by putting DO_EXECUTE underneath it. There has to be a good reason this doesn't happen.

 That's sixteen instructions, snoop. I like this AmForth business. 


