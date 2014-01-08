#Vocabulary Structure

Orcish vocabulary is fairly rigid stuff. We save a lot of words this way, at the expense of a certain flexibility. 

Single word commands are all precisely defined and well understood. Double word commands are parsed differently depending on the contents.

Orcs think a letta is anything printable that isn't a space, newline, or numba. Lettas that are alphabetic we call phonemes, just to keep it straight. 

The structure of the two letta command language is a fallthrough: anything that isn't otherwise defined is a DOCOL, unless one of the other words has been activated explicitly. 

Because 16 numbaz are reserved for the high byte, there are 78 * 94 => 7,332 words possible in extended Orcish. 

## Constant, Value, Variable

Variables are memory offsets, constants live in the pak, and values are in the spleen. `#` is the command to compile a constant word, and any cell word that has `#` in it is a constant. `,` in a word means it's a variable, `%` means it lives in the spleen. There are 155 of each reserved, which is a lot in practice. all-phoneme cell words may be compiled explicitly as anything you want by prepending an explicit compiling word: the tawka is smart enough to interpret certain byte words specially, if they follow immediately. 

In general, any word containing a glyph is reserved for the extended Orcish operating system. That leaves 48 * (48 - 16) => 1,536 compound words, because numbaz can't lead a word. That's a lot of words for a microcontroller. It leaves 5,000 or so for extended Orcish, which is quite the library, and should be doled out somewhat carefully so that Orcs are fearsomely capable in combination. 

See, Orcs can teach each other things, on a short and long term basis. As long as we have some community consistency about what glyph words mean, this is awesome. 

###Corrolary

This would mean that `WAGI` and `ORC!` aren't such great words. `W@GI` would be fine, and `O^k!` looks... Ok.

I'm hoping I can still make Orcish slightly whimsical. I'd just like that. 

## Extended Orcish

Batteries included.

Some people like APL derivates and enjoy memorizing long semi-mnemonic lists of commands.

For everyone else, there's MiddleOrc, which we won't make any less ANS compliant than we have to. 

