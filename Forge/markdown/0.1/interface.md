#Talking to Forge

Don't want to get ahead of myself, I still have most of Holler to write, and probably even need to dig into Pforth and fix the 
definition of KEY, but I'm mulling over how the Forge interface will work. Specifically the input interface. 

Someone else can do the mouse part. ^_^

Firstly, I am totally enamored of the Space Cadet approach to navigation. Navigation isn't a mode for me, it's got to always be
there underneath the arrow keys. That won't be the only way to get around: I'm starting to see the advantages of navigating from
command mode. 

Which should really be called the command register. We use 'mode' in a more Emacsian than Vimish sense: buffers have modes. 
The metaphor for input is registers: the space cadet keys push the keyboard into the appropriate register.

Our interface will discourage glomming, which is how you talk to Emacs: `Ctrl-Shift-C Alt-c-k` is a glom, and your wrists will 
suffer. Sometimes it's convenient to press `Alt-key` rather than `Alt key`, but they won't have different meanings. 

So yes, `Shift s` will emit S, just like `Shift-s`. `Shift Shift` is the shift lock, which we end with a `Shift Return`. I presume
you're using Caps lock for something useful. 

You can shift-lock any register. Without a shift-lock, the register listens until it has a command, executes it, keeps listening
if that's what the command says to do, otherwise, drops down into the previous register. The bottom register is self-insert.

Shift-locks stack, so you can go from insert register to command register and shift lock: `Shift Command` or both together. Type
from the command register, `Shift Alt` to shift lock the alt register, type some alts, then `Shift Return` to drop down into Command mode.

`Shift Return` again gets you insert mode, or just a second `Return` while holding the Shift key. `Shift Return Return`, without holds, would be `Return` from the Command register, what you'd get with `Command Return` from anywhere. 

Keystrokes concatenate, of course; they're nothing but compressed, mode-specific Forth. The mode of the buffer containing 
the cursor handles turning keystrokes into Forth words against the  mode dictionary and executing them. If Vim was in Forth,
I don't think Emacs would have survived the late 90s. 

This should give the right flavor. You just type: out of register commands are just "Register-key" and the hold down is optional. 
Shift-locking provides a consistent way to stay in-register, those folks accustomed to staying in the command register (which is going
to feel a lot like Vim under the fingers) will hit Command-shift and shift-return, or remap them, which is of course easy for us to do.
The registers will be hard-coded in, though; we don't play Calvinball. The goal is infinite flexibility, not a side order of infinite
flexibility to go with your Choosaphone. How you get around them is up to you; they're named after the keys that produce them in
standard Forge. 

##Copying and Pasting

The system versions of these are going to work differently, because typically we'll copy and paste both the meta and paradata along with
a reference to the originating mode. That means we'll write a fresh card with the information and move it over.

At some point I'll provide "clone", which copies by reference. Any changes to either copy will be reflected. A one-way version of that 
relationship would be represented with a link.