#ArdForth Register Doctrine

The intention is to study how the existing 'sketch' ecosystem generates code, and try to closely follow those conventions. 

Really we'd like C functions to end up being strange Forth words that can be called with minimal accomodation. This is the clever way to bulk up our libraries. It's almost like a foreign function interface, and besides, the compiler writers often know what they're doing.

http://jaxcoder.com/Projects.aspx?id=912541054#asmtoc

Basically, R25 is the top of the parameter stack, which is normally kept ihe registers 25-8. Ok. R25-24 is TOS and R23-22 is POS, maybe? an 8 bit stack seems awfully narrow, we use words as cells, I think... that gives 8 stack values and a continuation byte into SRAM. We use some lower byte to point to the actual TOS and round robin, maybe? 

It all reminds me of Towers of Hanoi. I'm sure there's a trick to it. 

##AmForth Registers

Here's how AmForth does it; hence, how we'll do it at first.

W: Working Register -> R22:R23

IP: Instruction Pointer -> XH:XL (R27:R26)

RSP: Return Stack Pointer -> SPH:SPL (R?:R?)

PSP: Parameter Stack Pointer -> YH:YL (R29:R28)

UP: User Pointer -> R4:R5

TOS: Top of Stac -> R24:R25

X: Temporary Register -> ZH:ZL(R31:R30)

Extended:

A: Index and Scratch Register -> R6:R7

B: Index and Scratch Register -> R8:R9

R0:R1 used internally to hold intermediates, R2:R3 are used for zeros, I guess? 

R14-R21 are temps 0-7, R10-13 are reserved. 

The only good reason I could think of to change this is to make it easier to call a C function, which we may end up wanting to do. It's not the first priority exactly. I think TOS is the same as the first parameter word and the return word in C, so that's a start. 