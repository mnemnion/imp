#Terminal Handling

Forge is an interactive text environment. We're making certain choices that I feel 2014 supports.

One is that we're xterm-native. No attempt will be made to support any other weird terminal that used to exist, because the 'terminal' is a program like the 'browser'.

That's actually a good way to think about it. Abstractions over all possible terminals are like JQuery supporting IE6. We don't need to support banks or libraries, and those are the last refuge of the actual terminal.

Our Chrome and Firefox are Mac Terminal and Xterm itself, respectively. I'm developing for Terminal. Right now it doesn't work in iTerm2, and I don't care, because I wrote it against the Xterm spec and don't care to figure out who is wrong. 

Chances are, it's broken under multiple values of actual Xterm as well. We need to support two keyboard paradigms anyway, but ultimately I don't care much and will support mostly the Mac.

Why? Because within two seconds of going public, someone will take offense and make it work for all values of X-term. As long as they don't twerk my core word contracts, I'm good with that. 

Oh, and we're a UTF-8 shop, because we aren't stupid. Eventually, this means an enormous lookup table to correlate widths with font with code point. For the forseeable future, that means that double-width characters will badly twerk everything. Shikata ga nai.

##Philosophy 

An expert tool will require learning. We intend to build this in, since it's one big ball of introspection here. 

There will be little effort made to support old, frail, or weird devices. Nor do we harbor resentment agianst the mouse or arrow keys. On my keyboard, the arrow keys are precisely where space cadet keys are found, just on the right. There is no ergonomic challenge in use. 

Practice has converged around '7-bit' terminal handling, because it's UTF-8 safe. Nor will Forge be any exception: 8 bit term will break it quite convincingly and rapidly. The command syntax of Forge is somewhat dictated by this constraint.

In particular, the `alt` key generates escape and the alphanumeric that follows. Everything in Forge can be done with the usual keys (including arrows) or the use of `alt`. If you want `hjkl` navigation you may certainly implement it, but compatibility is a two-edged sword. There's a large, uncomfortable uncanny valley which is felt particularly with command syntax: it's better to learn something fresh, if it isn't weird.

##Handling

The equivalent of `key` in Forge is simply `event`. Event returns a type of event with the value beneath it. Currently events are of variable length, and a handler must know how to consume them. I intend to make this regular, so that mouse
events return three values and all other events return two. The mouse must be handled user-instantly, and there's no percentage in indirecting it. 

`event` is written around Terminal's implementation of Xterm. Any standardization happens here, by writing entirely separate `event` words chosen on load. 

The master loop produces events, giving them to the selected i-handle. This triggers continuations that result in a display refresh, handing back to the master loop. These continuations may change the selected i-handle, or indeed any part of the chain may be modified by an action. Ideally, all exceptions will be caught by the master loop: file handling is an example of something traditionally handled locally. The master loop may taste the stack and handle mouse effects itself.

An i-handle is capable of parsing all possible events and consuming them, or it is badly written. They will inherit from a base handler that does the right thing, namely, nothing further. 

There are only three types of handler, and only i-handles are called. t-handles and o-handles, which deal with transformation and display of subjects, are attached to i-handles and called as a consequence of invoking the handle. 

My next step is to actually write the fundamental handlers and get them working. My current thinking is that they are not interchangeable, that t-handles cannot change the type of the stack state by definition, and therefore i-handles may call o-handles directly. 

One important point: handles are immutable references to **handlers**. A handle calls two handlers in sequence, meaning any one handle may chain arbitrary handlers. The subjects stay on the stack, as does the active frame, and we sand rough edges with carefully chosen global state variables. t-handler words in particular have no idea what the subject is and can be applied safely by any number of handles. 

That done, I can pound off a quick, large rolling allocator that complains when it's full, and start writing some window sequences. This is when we refine the command vocabulary: at first, we just parse Forth, then we condense it. 

Then it's time to hack the inner loop, so we parse, pad, and execute 'by hand' as it were. I need to clean up event handling so that handles UTF-8 correctly..

###Mouse Events

`event` breaks the input stream up into events. Mouse events are always handled by the window/master loop, anything from the keyboard drops to the frame's i-handle. Mouse events may certainly trigger i-handles, or other actions.

We'll support two types of clicks and the scroll wheel/two finger up/down. 3 buttons is uncommon, 2 is universal.

###Arrow keys

Due to the peculiarities of terminal, we may detect the following: up/down/left/right, the alt equivlants, ctrl left-right, and shift left-right. There's more horizontal than vertical there: the missing verticals express as ordinary up and down. 12 degrees of freedom is decent; I can happily use as many as I can produce. There are five buckys to the left of the space, and four arrows to the right. That should translate to 20 degrees of freedom without combinatorics.

oh here's another weird one: command-alt left and right. 14 degrees of freedom, 4 vertical, 8 horizontal including a convenient double-press. Really it's 2 vertical dimensions and 4 horizontal ones.

We'll probably parse these into individual events before the i-handles see them. They go to the active frame directly.

###Control Sequences

These are the non-printable ascii characters. There's some weird stuff in here I don't really understand, and we'll use these sparingly. Terminals reliably intercept some and we won't tamper with that. Some are your basic keys like `tab` and `return`; there's the occasional CSI, notably `backspace`, that we have to turn into something sensible.

In general I'd rather give handles minimally filtered access to the input stream, and make them born to destroy anything they don't understand (and optionally make a note of it somewhere). 

###Esc and Command

We loop things up so that `alt-key` and `esc key` have the same event semantics. Multiple escapes toggle. The way many i-handles work (the consistent UI) is that there's a base register and a command register. When you enter the command register you stay in it until it drops out, and you're back at the base. 

That's not Vim and it isn't Emacs. It's more Forthy than anything. Language shapes thought, go figure. 

I haven't quite settled on how flexible this all is. Certainly everything may be rebound, it's just a Forth wordlist with delusions of grandeur, stuffed into a couple of offset tables.

That's an important semantic: i-handles simply take a key-event, calculate a literal offset into a handler block, and perform the execution token stored there. So `self-insert-q` is stored at `handle-block char q +`. Fast and easy to understand, and dense: we're chewing up bytes, there's no reason for paths that can potentially lead nowhere. It's just not a good fit for arbitrary depth: if you want a lot of handlers, make a lot of blocks. You can do it lazily with masks if you want, concatenative languages make this kind of approach simple. 

`alt-;` is an easy type and is intended to 'lock' the command register: all print keys are interpreted as commands, and alts are ignored. I like idempotent commands, and idempotence in general. `;` is the logical choice to end a command sequence and return to the base register. 

####A Vim aside

Vim is written backwards from my perspective. `insert` is the special mode and `command` is what you go back to. As a descendant of ed, this makes sense, but you wouldn't do it now. At least I won't. 

Emacs is another case of language shaping thought. The fact that every keystroke calls an elisp function is perfect. The sequence of events in which those keystrokes are decoded... well, it does mean that Emacs can emulate Vim excellently, or do anything else. It's totally nuts though, like, crunchy nuts. Calvinball. `alt-/` is a good candidate for dropping into the Forge repl line. That's a separate vocabulary from a Forth program you might be working on, and we've got a long way to go before we do much besides developing Forge and Fabri in Forge. 

###Keymaps and Masks

A keymap must process all key type events. It's a dense array that covers all supported events, with guaranteed coverage.

A mask is like a minor mode, and does what it sounds like: masks over a keymap, producing in effect a new keymap. We could implement this as a fallthrough case statement, and make them chainable, but I hate that crap. It's just a token table, they're cheap. 

i-handles all have the default keymap attached as their fallthrough for key events they don't recognize. 


