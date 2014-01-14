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

The flag is `0` for 'no printable characters left', `-1` for printable character, and `10` for a newline. `2` means a double width character. 

My newline policy: all printers skip them if they are found at the head of a string. Everything which returns a line, returns it up to, but not including, the newline. This preserves the semantics of a newline without ever inserting one into the terminal, provided the frame handler does the correct thing.

1-printable becomes horribly complex as we start to support Unicode. The basic "get a glyph's worth of information" is fairly painless in UTF-8, which is all we'll ever support. Figuring out the width of the resulting glyph is a righteous pain. We'll want a hash to which we add glyphs as the system needs them: I have no interest in recognizing a character I don't personally use. If something screws up my own window, well, I'll add it, and I'll be generous with pull requests to `1-printable` that work. 

#### : n-printables \ ( [c-str] n -- count )

I wrote this first. It was therefore hard, and is not easy to understand. Refactoring it with `1-printable` will be clearer.

In any case, it consumes the address, returning only the count needed to print n characters across the screen.

#### : print-advance \ ( [c-str] n -> c-adr+ count- "string" )

This takes n and prints it, advancing the c-str accordingly. This is buffer safe, and will not exceed a string's remaining offset if properly used. It returns up to n characters, or up to a newline, and when printing, skips the newline. 

## Frames

I wrote a little box drawing library and am going to jack it up with simple line drawing and the like.

It uses the Unicode drawing set, rather than switching to graphics mode. This would be a relatively minor rewrite, if anyone out there needs it: all the drawing literals are abstracted behind words, as is the Forth way. 

A frame, proper, is simply a row col x y array. There are access words; a frame gets its coordinates when created, and getters and setters are defined. The 'pane' of a frame is the part of that frame which is writable. In Fabri, these will be types. 

## Printing

Printing happens into a frame, and expects the frame on top of the counted buffer. 

Right now, it's top-down, left-right. We can add right and center versions of that as we go: stacks should be right justified and grow from the bottom, for example. 

We'll eventually add fancy compound words like `size-frame-to-longest-line`, which will take a frame and a counted buffer, return the screen count of the longest line, and resize the cols of the frame accordingly. The window manager decides what to do after any frame gets sized. We'll also have `size-frame-to-buffer`, which does a rowcol resize in the same fashion. 

We have no window manager yet, but we're almost ready to write one! Forge will have multiple windows in a single terminal, so we can pop between entire views. Useful when developing another interactive terminal program, among other purposes. Forge proper will use one of them; we'll keep the hooks to a minimum. Chuck always says you don't need them, I'm starting to get why. 

## Buffer

I keep talking about a buffer. It's specifically a composed view, just a chunk of string that may be fairly rapidly painted into a frame. It's not line based, but line movement words will be provided. It's not for editing text, it's for displaying composed text. 

We'll be adding input handlers after this. That's a separate page.  

