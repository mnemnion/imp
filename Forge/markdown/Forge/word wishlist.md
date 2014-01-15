#Words I Wish Were in Forth

# Prior

The fact that a colon word doesn't immediately compile itself is a mistake Chuck has since rectified. 

It's a serious mistake. `: a-user-word create ... does>` puts one copy of the address on the stack. Want another? Juggle that puppy. ` ' a-user-word ` will not do the obvious. Forget recursion: this makes multi-state conditional execution of a `does>` clause pointlessley painful.

It's also dirt simple to have a word `prior` that lets the colon word call the previous version of the colon word. Which supports the only use case presented for this error, namely `: foo foo other-stuff ;` which would of course be `: foo prior other-stuff`. `'prior` would pollute the namespace less, perhaps. 