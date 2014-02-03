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

The equivalent of `key` in Forge is simply `event`. Event returns a type of event with the value beneath it. Currently events are of variable length, and a handler must know how to consume them.

`event` is written around Terminal's implementation of Xterm. Any standardization happens here, by writing entirely separate `event` words chosen on load. 

The master loop produces events, giving them to the selected i-handle. This triggers continuations that result in a display refresh, handing back to the master loop. These continuations may change the selected i-handle, or indeed any part of the chain may be modified by an action. Ideally, all exceptions will be caught by the master loop: file handling is an example of something traditionally handled locally. The master loop may taste the stack and handle mouse effects itself.

An i-handle is capable of parsing all possible events and consuming them, or it is badly written. They will inherit from a base handler that does the right thing, namely, nothing further. 

There are only three types of handler, and only i-handles are called. t-handles and o-handles, which deal with transformation and display of subjects, are attached to i-handles and called as a consequence of invoking the handle. 

My next step is to actually write the fundamental handlers and get them working. My current thinking is that they are not interchangeable, that t-handles cannot change the type of the stack state by definition, and therefore i-handles may call o-handles directly. 

One important point: handles are immutable references to **handlers**. A handle calls two handlers in sequence, meaning any one handle may chain arbitrary handlers. The subjects stay on the stack, as does the active frame, and we sand rough edges with carefully chosen global state variables. 

That done, I can pound off a quick, large rolling allocator that complains when it's full, and start writing some window sequences. This is when we refine the command vocabulary: at first, we just parse Forth, then we condense it. 

Then it's time to hack the inner loop, so we parse, pad, and execute 'by hand' as it were. I need to clean up event handling so that handles UTF-8 correctly..