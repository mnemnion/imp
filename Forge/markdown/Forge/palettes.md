#Palettes

Now that I know i have 256 colors each of foreground and background, a lot of things change. 

I was going to have palettes anyway, because not everyone likes color or can even see it. Though about the latter: anyone who can read fine-point text can distinguish at least 8 shades, and the 'ansi' colors are abstracted by any decent client today. In fact, ansi color favors the visually disabled, because an aware terminal could read the colors or even indicate them with punctuating sounds. Applying CSS over HMTL is much harder to parse out. Of course it's also arbitrarily flexible. 

But not in ways we want. As data wranglers, anything that doesn't fit into rows and columns can't be easily worked with further. We're using the terminal for a lot of reasons; one of them is that rows and columns are well-defined for any mapped space, always. 

In fact, we have four dimensions per location: x, y, value, and color. 

[http://en.wikipedia.org/wiki/Block_Elements](Block+_Elements) allow us to do some fairly fine-grained mapping of data, along with the basic line and box drawing facilities.

But palettes. We want them to be fairly swift; we'll be doing an outrageous amount of tagging things, and even precomposed it's part of some longish inner loops eventually, esp. when we get to data analysis. 

##The Palette

There will be a 'the' palette, what's charmingly called a Singleton, or a source of truth.

Colors are supposed to *mean* something. You may choose the color. I choose the meaning. Cute though it might be to support Palette-by-frame, the result could only be color vomit. Many less-attuned ravers^H^H^H^H^H^H users will find my use of color dizzying already. Not everyone can handle as much data as the pipe can hold. 

As a (slightly confusing) result, some of the palette colors will be named after colors. You may, of course, shade them however, but a colon word, for example, will be highlighted as `p#red`. Ultimately this will let people design a color scheme for arbitrary syntax, one that will translate in a consistent way across platforms. The Sublime Text / TextMate / everyone thing of trying to specify the semantics of the syntactic group is hopelessly naive. I constanly end up saying "sure, okay, hashes can be a control word, as long as it makes them pink". Worse, it's up to the palette which semantics to support. ff'n mess. 

Other palette colors will have system names like `p#f-command` to highlight a frame that's receiving commands. The palette is semantic, not chromatic. `.f-command` will: compile the palette address with offset added, then `type` the string placed on the stack by the palette. 

Corrolary: there needs to be room in each palette for as much command as we can send. `e[38;5;220m` is a perfectly reasonable thing to say, and that's 11 bytes right there. 4 cells is not unreasonable, with 8 gb on a typical machine, and probably less than 64 shades in play. I'm coding, not interior decorating. Even 512*4 = 2048 cells, a whole 16k just for the palette, is a reasonable sacrifice. It's ludicrous; we won't do that. But the offsets will be consistent and easy to calculate. 32 bytes is enough room to turn on a background and foreground: `e[38;5;220;48;5;128m` is 20 characters, and we read to the first 'm' when we return the count. Plenty of room to dim things or whatever; we won't have many lengthy palette commands, it's rare to set both foreground and background at the same time. 

