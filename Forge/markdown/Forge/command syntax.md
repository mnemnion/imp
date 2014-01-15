#Command Syntax

Forge won't really have a vi syntax, because it's designed for console piloting, not text editing per se. The expectation is that many frames will be open and connected to each other in programmatic ways. 

Key handling itself will be very flexible and general: keys fall into a key handler provided by the window manager, often based on the selected frame. In general, Esc will put you in command mode, while Alt-key will produce the same as esc-plus key. normally, insert and command are the only modes in the vi sense of the term.

`alt-key` equally `esc key` is mindlessly simple to provide, since that is precisely the two byte sequence send by the alt key on vt-100 terminals. I expect my convention will be that single-alphabetic keys such as `alt-a` will take no arguments, dropping back down to the insert mode, whereas a key like `alt-;` will start a command sequence. 

I don't really know what the command sequence will look like, other thant `.` will start anything relating to movement. It's a point, like the cursor, and `alt-.` is an easy type, for those who don't like arrows. Me, I like arrows, and mine will work just exactly like Mac arrows in every respect I can make them. 

I have some reading up to do on vim, and should probably start using it to edit Forth code. I'm also interested in J; it would be swank to introduce the idea of operators that work on various types, lines, numeric data, JSON or edn, etc. 

The syntax will be forward only, as usual. 

##Initials

Initial characters start a type of command. Currently we have `.`, I like `;` rather than `:` for `command` because frequent keys should be unshifted. Similarly, `,` and `/` make good candidates for initials. `\` is okay, perhaps a signifier instead. 

##Segregators

Segregators are characters that turn on and off a mode of interpretation. One segregator I will likely use is `` ` `` meaning "interpret these words in Forth". Sort of our `Meta-x`. 

`"`, `'` are the other ticks that we automatically read as segregators. `'` should probably be a literal string. no escaping, `''` as a double segregator if you need to catch `'`, `'''` for `''` and so on. It's a rude, functional pattern that works great. Markdown has some genius ideas. Empty string is simply the absence of an optional string. Passing an empty string to a mandatory string word is balls.


 `"` I'm reserving for now. We shall see.

##Pairs

`[` and `]`. `{` and `}`. `(` and `)`, and `<`, and `>`. Every programmer loves these keys: we have exactly four paired single strokes, and they are the basis of much of 'syntax', as in, what trolls tend to argue over. 

`\` and `/` could also be used this way, but are more common as segregators in common use, or signifiers, sometimes.  

They are less obviously useful in forward-only contexts. If I incorporated all of these into a syntax for pushing a cursor around, you would justifiably hate me. But wait: frames have panes for display, but what they display is abstracted. 

Some of our pairs can be useful to act, not on the display data, but on the underlying data. That's where J is interesting to me: "show the result of selecting all `foo` in the JSON of frame 1 and frame 2 in a new buffer attached to frame 3" or "perform this word matrix-wize on these two arrays, display the output in the current buffer". Those are *commands* right there.  

##Signifiers
The rest of the glyphs are signifiers. These are 'lowercase' signifiers: `-` and `=`. On my keyboard, that's what's left. 

That leaves `?|~_+!@#$%^&*`, unless I missed something. `@`, `$`, `#` and `&` have a good reputation as 'typecast the thing to follow'. 

This text is mostly something for me to refer back to later, when I start actually doing things which require a command vocabulary. 

The actual command language should favor alphabetics, unless it shouldn't. J again.

###Things nice to say

and imaginary translations!

"take my current buffer, split it into words, count all occurences of the words in buffer 2, and display the output interactively in buffer 3."

I really have to read up on array languages. they're key here. Play with J labs a bit. "highlight using" is a good word, it would take the first argument and apply the second argument as a word that returns a highlighted form. We'll have a lot of words that return a color, or something which can be translated into a color, as a flag. 