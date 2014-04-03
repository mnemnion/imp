#ANSIMark

Short version: a markup language that uses ANSI escape sequences in a more-or-less compatible way with the intended use. 

It combines visible markup for things you'd like the raw text to contain, with escaped markup for render-specific stuff.

To cite a stupid, but viable example: To create a link, you turn on a shade of blue and the underlining capability. It's followed by "invisible text", which we use for escaping textual info, containing the actual link, then "reset color" as a terminator. 

Feed it into xterm, get a blue link. Feed it into something that reads ANSIMark, get a blue, clickable link. Strip the ANSI, you get [link](http://example.com) turning into link http://example.com, just like that. 

Yet another incompatible way to make a webpage! Break out the champagne!

Not the intended use case, which is semantic highlighting of code.

##ANSIMark in action

The reason this is cool: The escape character isn't used in source code. Period. Meanwhile, the basic VT100 codes are almost as old as ASCII. 

Properly integrated with the editor, this allows for copy-pastable, local, marked-up information about the metadata of the code. Things like the offset at which a function is defined, the type represented by the token, all the usual things we represent with color or somewhere off to the side. 

Copying and pasting shouldn't change the code, but will routinely change the markup. The code travels with its own context, which makes reconciling it with the new context a lighter task. Think of the boon for integrated version control, if every copy and paste was marked up with the originating file and offset. 

The link example was called stupid. In many contexts it would be. The rule of thumb with ANSIMark is that it is neither entered by the user nor consumed by the target, in the general case. ANSIMark covers metadata: a compiler should work from a stripped source file containing no escape sequences, nor should the user need to add escaping manually.

Syntax/semantic highlighting, context preservation, decoration containing information derived from the representative data structure (think range and type information attached to the nodes of a tree), these are the sort of thing that ANSIMark excels in. Having a consistent schema for rendering the data into the usual intermediate forms, notably the witch's brew of HTML/CSS, is another useful one. Here, maybe the link thing is not so stupid, and there are direct maps between things like `<em>` and the bold command in ANSI. 

In Markdown, we try to make the ASCII look like an email. In ANSIMark, we try to make the ANSI look like an xterm page rendering the content. 

