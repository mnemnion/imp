#Command Syntax

Forge won't really have a vi syntax, because it's designed for console piloting, not text editing per se. The expectation is that many frames will be open and connected to each other in programmatic ways. 

Key handling itself will be very flexible and general: keys fall into a key handler provided by the window manager, often based on the selected frame. In general, Esc will put you in command mode, while Alt-key will produce the same as esc-plus key. normally, insert and command are the only modes in the vi sense of the term.

`alt-key` equaling `esc key` is mindlessly simple to provide, since that is precisely the two byte sequence send by the alt key on vt-100 terminals. I expect my convention will be that single-alphabetic keys such as `alt-a` will take no arguments, dropping back down to the insert mode, whereas a key like `alt-;` will start a command sequence. 

I don't really know what the command sequence will look like, other thant `.` will start anything relating to movement. It's a point, like the cursor, and `alt-.` is an easy type, for those who don't like arrows. Me, I like arrows, and mine will work just exactly like Mac arrows in every respect I can make them. 

We'll constantly be adding 'either alt-letter or esc letter' to documents, so our shorthand will be `→`, which you can't type easily without a macro. That way we can say `^-` or other plausible shorthands in our command syntax. This is better: we've reached, finally, the age of copy-paste Unicode. Mostly; I'm looking at you, V8.

I have some reading up to do on vim, and should probably start using it to edit Forth code. I'm also interested in J; it would be swank to introduce the idea of operators that work on various types, lines, numeric data, JSON or edn, etc. 

The syntax will be forward only, as usual. 

#Structure

Fundamentally, I'll be using the Forth parser and stack to execute commands. The command syntax will be a compressed Forth where space is not a privileged character. Much like Orcish except for the latter element; so even more brutish, in a sense. 

As I work on this, I'll be slowly designing the long form and using it. Short form is a matter of translation, and we can show the long form in parallel if the user is in the learning phase. Tools should boot assuming ignorance on the part of the user, with a 'pop to intermediate mode' option. There is no 'expert mode' in a true tool, just a config file.  

The most important point is that it will be a forth: initials specify a parser, numbers will prefix, alphabetics will act on either default values or the stack contents if such were pushed. 

Every command sequence will start by putting zero, then an esc on the stack, so single-stroke commands that expect a prefixed number will see an esc. If you want to pass them 27, there won't be a zero under it, there will be an esc. Easy; we may actually use extra cell width for this, we really have a lot of space on a 64 bit stack. 

##Initials

Initial characters start a type of command. Currently we have `.`, I like `;` rather than `:` for `command` because frequent keys should be unshifted. Similarly, `,` and `/` make good candidates for initials. `\` is okay, perhaps a signifier instead.


##Segregators

Segregators are characters that turn on and off a mode of interpretation. One segregator I will likely use is `` ` `` meaning "interpret these words in Forth". Sort of our `Meta-x`. 

`"`, `'` are the other ticks that we automatically read as segregators. `'` should probably be a literal string. no escaping, `''` as a double segregator if you need to catch `'`, `'''` for `''` and so on. It's a rude, functional pattern that works great. Markdown has some genius ideas. Empty string is simply the absence of an optional string. Passing an empty string to a mandatory string word is balls.


 `"` I'm reserving for now. We shall see.

##Pairs

`[` and `]`. `{` and `}`. `(` and `)`, and `<`, and `>`. Every programmer loves these keys: we have exactly four paired single strokes, and they are the basis of much of 'syntax', as in, what trolls tend to argue over. 

The pointy ones `{, }, <, >` we reserve for flow control, the others `( [ ] )` for grouping. As usual, `[ ]` will be for arrays/ranges/vectors/what have you, and `( )` will provide some kind of list or set. 

`\` and `/` could also be used this way, but are more common as segregators in common use, or signifiers, sometimes.  

They are less obviously useful in forward-only contexts. If I incorporated all of these into a syntax for pushing a cursor around, you would justifiably hate me. But wait: frames have panes for display, but what they display is abstracted. 

Some of our pairs can be useful to act, not on the display data, but on the underlying data. That's where J is interesting to me: "show the result of selecting all `foo` in the JSON of frame 1 and frame 2 in a new buffer attached to frame 3" or "perform this word matrix-wize on these two arrays, display the output in the current buffer". Those are *commands* right there.  

##Signifiers

The rest of the glyphs are signifiers. These are 'lowercase' signifiers: `-` and `=`. On my keyboard, that's what's left. 

That leaves `?|~_+!@#$%^&*`, unless I missed something. `@`, `$`, `#` and `&` have a good reputation as 'typecast the thing to follow'. 

This text is mostly something for me to refer back to later, when I start actually doing things which require a command vocabulary. 

I'm thinking `$` means 'frame' and `@` means 'subject', with a consistent follow syntax: `$` means 'current frame', `@4` means 'subject attached to frame 4', etc. `&` for h`and`ler is the handler attached to the subject in question, which may be a frame (`&` or `&n`), a buffer `&'buffer'`, or a passed anonymous subject. 

The actual command language should favor alphabetics, unless it shouldn't. J again.

##Conjunctions

We need to be able to pipe a subject through multiple handlers, so we can have several views of the data. A handler simply takes words off the stack and uses them to prepare a new anonymous subject. Sometimes this is a display buffer for text printing: in the degenerate case, a handler reaches directly into a bare string and returns a certain amount of it to the frame. 

The crucial thing to understand is that handlers always take and return subjects, which are counted areas of memory in a literal sense. When a subject is attached to a frame, the frame handler provides a window into it. 

A subject is **not** restricted to strings, in the slightest. On a low level, searching for the first occurance of a regular expression is a handler, which returns an anonymous subject, in this case an offset into the original subject. 

A handler is clearly nothing but a particular type of Forth word, one which returns a subject. They live in their own special namespace. Buffers are a named handler. 

The way it works is frames are always attached to a handler which is always attached to a subject. There is a default handler and an empty subject, perhaps `-&` and `-@`. If you move a handler without first detaching the subject, the subject comes with. Same for moving frames, which we routinely do: the handler and the subject travel with. 

I think you 'detach' by attaching to the null subject or handler. There's no normal need to detach whatever handler a frame has and pass it over to something else. 

Frames, being the only places that can accept input, also have input handlers. `` $` `` is a good name for 'this input handler', with `` $`4 `` etc in the best forward style. The subject of an input handler is the input stream. 

We'll be using a weird blend of prefix and infix syntax by introducing conjunctions. I don't rightly care, I want to be able to type sensible yet powerful things. Words that consume exactly one more word and then do something are looked at askance but by no means forbidden, even in Forth.

### ...conjunctions

Right. `>` and `<` are attachment. `&foo > $bar` == `$bar < &foo`: handler foo is now attached to frame bar. 

The crazy is: handlers attach to arbitary numbers of other handlers. Each time a handler returns, it can trigger an arbitrary number of handlers to do things with the returned data. `&foo } &bar > $baz` will mean that any call to &foo will trigger &bar, which is now attached to the `$baz` frame. Presumably `baz` is a display handler and `bar` does some type of transformation. We fold from left to right, because Forth, not right to left like cray cray APL. We're not doing math usually, that's a handler's job, this is plumbing. This requires remembering the previous token, which is suitably forward only: we probably automatically `dup` each word and destroy it when the value isn't called for. 

`&foo { &bar > $baz` means what? bar is now attached to frame baz, and any (presumably display) calls to $bar are also handled by &foo, producing whatever, since we don't know what stuff &foo is attached to. Great power, great responsibility.

Can you imagine a physical patch board so you don't have to do this shit with your fingers? Yes you can. It would be a true professional's tool. All it would have to do is send ASCII. My grandmother, bless her soul, used to send ASCII. 

A lot of commands string conjunctions: a simple 'search for all `foo`' type command will make an anonymous subject consisting of those offsets and hand them off to a handler that arranges for appropriate syntax highlighting. Transfering the intermediate subject will put all those `foo` in an attached frame, while piping it will dynamically update the `foo` count any time the first subject is handled: pretty, potentially quite expensive. 



###Things to say

and imaginary translations!

"take my current buffer, split it into words, count all occurences of the words in buffer 2, and display the output interactively in buffer 3."

I really have to read up on array languages. they're key here. Play with J labs a bit. "highlight using" is a good word, it would take the first argument and apply the second argument as a word that returns a highlighted form. We'll have a lot of words that return a color, or something which can be translated into a color, as a flag. 