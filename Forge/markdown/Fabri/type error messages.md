# Type error messages

The Fabri type system is intended, like any good type system, to help the user detect and correct errors of intent.

This is just a quick note to suggest a way to generate good error messages. 

If a word has the form ( foo bar -- baz ), and the top of the stack reads 'bar foo', Fabri might suggest a 'swap'. By following that analysis forward, Fabri may in some cases be able to detect that an 'over' is more useful. 

Basically, we try fundamental stack manipulations in between our last valid word and our first invalid word, then continue to type check. Whichever manipulation gives the best result (if any) is suggested. 