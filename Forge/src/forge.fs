include ansi.fs
include toolbelt.fs
include stack-util.fs
include keyword.fs

\ ui
include frames.fs

\ inner loop
variable eval-pad 128 allot 

\ we need some way to catch all exceptions here. 
: innerloop  \ no more ." ok" cr
	eval-pad cols accept 
	eval-pad swap evaluate 
	recurse ; 



: n-printables 
( 	this word has to take a counted string with
	a maximum width of printable characters. 
	It returns the number of bytes to read,
	in order to print up to that many characters.
	If there is a newline, it returns the count up
	to, but not including, the newline.

	this requires us to parse ANSI control sequences,
	which is great fun, since they can be of arbitrary
	length. Best of all, there's a 'concealed' text
	attribute! which we will abuse the hell out of, no 
	doubt. in any case, we intend to support ANSI correctly.

	To *really* do this right, we'd need to handle Unicode. 

	Gforth provides an xchar library for dealing with local encoding,
	actually. GNU, sometimes you gotta love em. "include kitchensink.fth"

	The algorithm: look for a newline. If the literal width is small enough,
	we don't care about printable characters.

	If it isn't, we regex for escape sequences, and subtract their size from the line.

	If it's still not small enough, we truncate.

	We also return a flag, true if we sent a whole line, false if not.

	If true, there is a newline directly after our string. If false, we're still on a logical
	line. 

)
	;
	
anew wipeout