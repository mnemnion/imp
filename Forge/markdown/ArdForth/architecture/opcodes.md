#OrcForth opcodes

The phonemes should be quite carefully chosen. They're hard to change later. 

Front/unrounded vowels `a, i` for capitals, `o, u` for miniscules. Shwa reserved for pronouncing punctuations, also, emphasis on the first of the two syllables. `Zapə`, emphatic on the za. We'll use the Curtis form unless there's good reason not to, but it will of necessity sound weirder than that. 

Numbers are a problem. I don't care greatly about speaking the Low Tongue. Yet.  

Really, I'm inventing yet another bytecode virtual machine. But it's Forthian. That still matters.

0-10 a-f hex numbers

! @ + * / % : ; \ `\n` as usual (?) 

`~!@#$%^&*()_-+={[}]|\:;"'<,>/?

# Count

Space separates, tab is reserved, newline triggers interpretation. 

Other control characters and escape sequences may be recognized by the core OS. Probably not; we may as well use TAB switching for commands, it's a large key that doesn't see much good use, given that it emits a perfectly sensible 9. Tab could pop out and speak directly to the protocol server, if there is one, and there generally is. 

This would create a Pythonic love-hate relationship with the interface. It's worth having extra code that lints anything that might have come from a text editor. Dont Tab Forth, and DO NOT Tab Orcish. I might even make OrcOS WAAAGH!! at you.

31 glyphs
16 hexidecimals
47 single letter codes
            
            	A
            	B
        	    C   create
            	D   dup
           		E   else
           		F   if
g				G  
h 				H
i   input?		I
j				J 	
k				K
l	does> :λ	L
m				M
n 	nip			N
o   output		O  	Over
p  				P 	dumP 
q				Q
r   rot			R   dRop
s   swap		S   
t				T 	then
u				U
v   r>			V   >r
w				W
x				X   do :eXec 
y               Y
z  	sleep	    Z 	freeZe
~   mark        `   forget
?               /	div
.	emit        >	les 
,               <   grt  
'               "
;	enddef      :  	def
{               }
[               ]
\               |
!               @  adr
#               $
%               ^
&               * 	mul 
(               )
- 	min         _
+ 	add         =   eq

