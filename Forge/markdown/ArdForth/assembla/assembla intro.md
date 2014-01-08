#Assembla

I'm going to write a really wordy assembler, because I don't like cryptic code and convention isn't really my thing.

I think it was Kernigan whom, when asked what he would change about Unix, said "I'd probably put an e on creat". I like to use my vocabulary to elucidate problem domains, rather than tnk wrd voc dom. 

So lets look at the opcodes of AVR.

## Some guy named Krue's assembler

http://krue.net/avr/

##AVR ASM

There is of course, a standard set of names. There are more than a hundred instructions; don't even know how to count them, because of LD (load-indirect). I have... much to learn. 

\ ( upper := <R16 .. R31> ) probably we need to explicitly list each reg.

\ ( lower := <R0 .. R15> )

LDI load-immediate upper , byte \ ( R16 .. R31 )

we could make this something like `upper byte load-byte`, because that's what it does, right?
