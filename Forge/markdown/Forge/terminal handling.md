#Terminal Handling

Of all the things I can be working on, I'm starting with a terminal library.

Gforth provides one, which I don't care for. Also, it's GPL, and I'm making effort to rely on as few non-ANS words as possible. Eventually we need to be on pforth, which is AFAIK the only 64 bit native public-domain Forth. *Much* later we want a metacompiler that targets ARMs, and Intels if someone wants to bother. 

I'm a visual coder, and I grew up in the BBS era. What I want is a colorful, interactive console that can be used for exploratory systems programming and microcontrol. I want that more than I want a Forth type system.

Also I'm a babe in the way of the Forth. I'm learning the Sudoku of the stack juggle. It's refreshingly similar to forms juggling in Lisp. All it needs is annotative inferential stacking to have a truly playful and user-empowering kind of feel. 

I want a typed stack print. First, I want a simple stack print, that grows from the bottom, and lives in its own window. The kind of thing you could add to the interpreter loop and get an off-band update every time. 

For that, I need a frame to display buffer contents, and some intelligent words to calculate the actual character width of a given sequence. This is non-trivial, since I like color and want to recognize ANSI escape sequences as zero width. I also like Unicode, though it's a bottomless pit. If someone wants to write a proper UTF-8 recognizer that can return all the state values of the Unicode state machine, I'm eager to see that happen.

Halfwidth, Fullwidth, Bidi, composing characters, zero-print characters, backspace and print over characters, yeah. Someone else can do that. I'll buy you a case of your favorite. 


##Basics

I'm going to start by defining simple formats and move onto more complex arrangements when we start actually loading code. Current plan is to build the code editor around OrcForth, rather than ANS. We may well do this with Fabri also, which is (probably) easier to build alongside a new Forth than over an existing one. Pop the hood even slightly on gforth and there's a morass of implementation specific words. 

###Output

The simplest possible thing is to output to the terminal. I've made progress with this. 

#### : 1-printable ( c-adr count -- := 1-char flag )

Not yet written. Should have started here. ^_^ . 

This word takes a counted string and returns a '1-char', which is a counted string that will produce one terminal character if typed. 

The flag is `0` for 'no printable characters left', `-1` for printable character, and '10' for a newline. 

My newline policy: all printers skip them if they are found at the head of a string. Everything which returns a line, returns it up to, but not including, the newline. This preserves the semantics of a newline without ever inserting one into the terminal, provided the frame handler does the correct thing.

#### : n-printables \ ( [c-str] n -- count )

I wrote this first. It was therefore hard, and is not easy to understand. Refactoring it with 1-printable will be clearer.

In any case, it consumes the address, returning only the count needed to print n characters across the screen.

#### : print-advance \ ( [c-str] n -> c-adr+ count- "string" )

This takes n and prints it, advancing the c-str accordingly. This is buffer safe, and will not exceed a string's remaining offset if properly used. It returns up to n characters, or up to a newline, and when printing, skips the newline. 
