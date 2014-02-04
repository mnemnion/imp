#Handlers

Handlers do all the heavy lifting. Frames know where they are on the screen, where the handler is, and that's it. Frames tell handlers "here are some instructions" and they do it; the frame is thereby updated. The frame is just a frame, it doesn't know anything, really. 

Subjects are also fairly inert in general. Input into a handler is required to change them, and access requires making requests of the handler also. It's actually two words in one, and handlers can be anonymously composed from i-handles and o-handles. The user input is an important subject; sometimes a frame will switch i-handles without necessarily switching o-handles. 

A subject always has both, and there are separate defaults which apply. Each may be independently switched out, but you always have a pair.

The simplest handler is a display handler. It receives formatted AnSCII and politely gives the frame the amount requested. A format handler takes arbitrary data and formats it for display. A transformer takes the output of one kind of subject and produces a new subject for the next handler. There is also a flag, zero for failure and any other number indicating the type of the subject. This goes on the top of the stack, under the new subject. 

##Types of Handler.

An input handler, i-hand, takes a subject and applies it to the attached subject, returning the entire subject and a return flag. If there's an 'on input' handler attached to the subject, the xt is pushed and called.

An output handler, o-hand, takes a simple request for a piece of the subject and produces it. They distinguish between passing by reference and passing by value: in the former case, the system must never mutate the subject, nor count on its existence. 

A transform handler, t-hand, is given read privileges on a subject and write privileges on another subject, and applies them accordingly, returning the written subject. 


Handlers, being but a word, may be either reserved in a dictionary or ephemerally created on the stack. There's certainly no need to manage their allocation, the words they refer to will be either global-static or modular when we get there. Subjects are heavyweight and get allocated and freed by a subject pool: there are limited handlers in play at any given time, and subjects will tend to either disappear or stick around. 

Handlers let us rope together multiple effects and consequences. Frames and subjects attach atomically, handlers may trigger another handler on input. For a trivial example, an editing handler will update the display handler after every action, which will refresh the frame. A char is inserted, a char appears. 

A less trivial example: a particular input command triggers a handler to return compact arrays, column-long, from CSV data in the subject. This output is attached to the input of a different display handler, which displays it in another frame. Flow control may be set in either direction, or it may be a one-time copy, and one or more transforms may live in between. 

This is potentially confusing: I'm talking about literal handlers, not conceptual ones. You may and routinely will have dozens of the same handler, working on different subjects. But two i-handlers for one subject, or indeed anything but one i and one o, never happens.

Something else that's confusing: handlers receive a subject as **input** and similarly output one. Neither of these should be confused for the subject of the **handler**, though they might be, in whole or in part. Other times, the input is a command, and the output is an entire new subject. Never a handler, though again, handlers may call other handlers as their last action.

A consequence is that a handler can be written by calling a bunch of other handlers directly, as ordinary words, then attached to a *handle* and will behave identically to that handle, chained to other handles, that call all the handler words in sequence. 

Confusing? It shouldn't be in practice. Handles are execution-driven linked lists that call handlers. A bit of Lisp, right? Of course right. With stack-driven continuation passing and just a taste of global state. 

Flow control is accomplished by handlers that combine and pipe inputs and fork outputs, either dynamically in response to other handlers being called, or statically as a single command sequence. `><` probably means 'fork this output, once, into the next two handlers' while `}<` would do the same for an active pipe. Merging the output of two handlers is a rarer case, I think. We're getting a little Choosaphonic here: the main goal is observing data, not munging it. This isn't quite yet another language; we call Forth if we need heavy lifting in a command sequence: `` `forth here goes` `` or `:word`.

We will rapidly need the full generality of these tools when we start taking over the inner loop. It's MVC, more or less.


###Flow Details

Well, I'm about to write the handle architecture. Let's see if any of this holds water.

####i-handle

i-handles respond to `events`, which have variable stack effect. Predictable, in that the flag planted at the top implies an exact number of cells below. The `event->i-handle` contract has a large surface area in the interest of speed. A default event always has one cell below it, and it is always a 'string' type subject. 

i-handles are called by frames, but have only an `event` on the stack. `this-frame` is set by the window-handler before calling i-handles. we'll probably have a special kind of handler for files, sockets, ports etc. since the events are fundamentally of a different kind. 

i-handles leave a subject on the stack, having the total effect \ ( event -> buff off ). 

####t-handle

A t-handle leaves a new subject and calls the next handle.

####o-handle

an o-handle must know the output frame. There's no sense in having more than one of those at a time in a single-threaded loop, so o-handles will have `that-frame`, which is global. If and when we add concurrency, `that-frame` may be redefined along with o-handles to return a reference to their frame. 

The benefit to this approach is that i/o chains are logically detached from frames. You can stretch the same transformation between frame #1 and #4 or #5 and #2, by setting `1 #frame i-frame! 4 #frame o-frame!`

We're aiming for a short form command sequence. It's totally up in the air, but something like `$$}:compendium >$4` meaning "take the subject of the current frame, attach the 'compendium' transform to its input handler, and attach that transform to the display handler for frame 4." `:` takes one full-length Forth word, and is space-terminated. 

This would then be executed once, because the command itself qualifies as input to the frames i-handle. Every following change to whatever frame was `$$` will be transformed accordingly. 

There is only one o-handler that consumes its subject: `handle-ender`, which is the default next for an o-handle. An **o-handle** will consume its subject unless it is chained to another handle, which is plausible: we might want to return focus to a new frame, for example, or redraw some other frame. 

t- and o- handles have interchangeable stack effects except for terminal o's. An i-t sequence that terminates without output is plausible, or even an i only sequence: `handle-ender` has consistent effects if put in the `next` cell of a handle. `handle-ender` may have an additional responsibility, leaving a unique stack token that demonstrates that a handle chain was terminated correctly. 

The loop is very simple: we get events, dispatch mouse clicks, and pass the rest to the current i-handle. We check that the return is correct, attempt to recover if not, and otherwise take another event. 