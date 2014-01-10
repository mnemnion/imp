( Master Loop 

	We shall have a line-based command loop.

	When we aren't in the status line, we'll need a key-based, flexible event handler.

	This accepts a key or event, flags it, finds the handler context, and handles the event. 

	At first, when this loop breaks, we'll drop out of it. Later, it will catch and handle all events.

	Won't that be fun! Implementation specific, too: we use as little event handling as possible. 

)