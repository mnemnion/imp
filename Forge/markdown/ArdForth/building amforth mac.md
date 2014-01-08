#Building AmForth for the Mac

After many false starts, I realized that avra is a brew command. So:

```sh
brew install avra
```

Gives us a perfectly functional command line assembler. Now we need to figure out how to use it. We'll start by downloading Amforth.

turns out we use avrdude to load prebuilt hexes. so it goes.

http://www.ladyada.net/learn/avr/avrdude.html

Then I find AVRforth, which has some colorforthian ideas:

http://krue.net/avrforth/

It comes with its own assembler, written in Forth. Yeah, we use this one. Public domain too.

Just putting this here:

http://pygmy.utoh.org/riscy/manual.html

This is an AVR Forth that might be cool.
