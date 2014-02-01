( # Frame Handlers )

( 	All frame activity is delegated to handlers. A frame has an input and output handler: the window manager calls these for 
	visible frames. We may optimize this so that it only calls frames which need refreshing, but only if lag annoys. 


	Handlers are conceptually simple: They get called with their frame on the stack, and put their own data token down, 
	like your basic create / does> word.  Handlers have an action, a subject, and a `next`. 

	As a result, handlers do not consume their frame token. The default 'next' action is simply 'drop'. 

	There are three types of handles. input handles, i-handles, deal with events: keypress, mouse click, and anything else that 
	becomes relevant. They are *read*only* with respect to the subject, and leave a subject token on top of their frame token. 

	For a single letter insert, the i-handle will: handle the command 'a' as 'self-insert-a', pad a copy of the line with the 'a'
	at the appropriate place, and leave this padded copy on the stack, on top of the frame. 

	The next type of handler is a t-handle, which expects a frame and a subject { frame subject -- }. A t-handle might have a 
	name like `merge-to-current-line`. How would that work? That depends greatly on the data structure of the subject. 

	The t-handle might call another t-handle called 'mark-current-frame-changed', and then `next` on out of there, clearing the stack.

	t-handles always leave a frame and a subject, so `t-next-default` is a 2drop.

	It is up to the window handler to then call the frame display handler. Now the user sees the letter 'a'. o-handles are simply 
	called via `frame display.frame`, which has the effect { frame -> nil -- "frame" }.

	Or the t-handle could update the frame directly by calling the o-handler. We might call this t-handle 'refresh-frame-and-exit'. 
	
	o-handles just exit, which falls through to the master loop. Which merely calls the input handle for the current frame. 

	note that this is fairly general and that 'text editing' is not a primary purpose of Forge. Forth code editing and interaction,
	sure. Perhaps more general purpose data wrangling, in time. 

	Some notes:

	The frame subject is always held by the display handle, within the frame. 

	The frame token is always at the bottom of the stack. It may only be dup'd.

	Note that *as*called* all handlers consume the frame token and have a nil effect. Generic handlers are chained,
	so *as*written* they do not consume the frame. 

	Handlers are born with a 'next' token. They *do*not* call this themselves: a handle call dups the address, pushes it on
	the return stack, calls the handler, retrieves the xt, offsets to 'next' and calls it. 

	I don't think this is more complex than it needs to be. 

	)

include ~+/handler/i-handle.fs





