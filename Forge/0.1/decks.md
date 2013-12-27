#Decking and Shuffling

These cards we're working with are a modern refinement of the oldest data structure in existence.

This data structure is also called a card. I don't usually make up terms when there's a better option. Basing the
rest of the language on Tarot is, well, that's how I roll. 

Sumerians didn't call it a card, they called it a me, pronounced may. Yes, they had rows and columns, typically
12 of the former and 60 of the latter. Our format is not sexigesimal, and is portrait not landscape. 

Well before computers, the mechanical ancestors thereof read information off stacks of cards. Our cards are virtual,
but they aren't actually different in the slightest. They have absolutely fixed sizes and bit-level meanings. 

Cards are convenient in-memory, but a lot of our data will be off card while we're using it. Memory is expansive but not
infinite; imposing a 1/8 overhead on data adds up for big sets while imposing extra constant time on algorithms. There's 
a reason we have suits other than `card`. 

The address is "on card", and sometimes we conflate this with the data, for good reason. Any data which is addressed on a card 
is logically on that card, and links are dereferenced: a link is a word which can find a card, not an address. 

Cards in their in-memory state are said to be shuffled. Cards in storage are said to be decked. 

Cards, in or out of memory, are 1k blocks. If they are smaller, they've been compressed in some fashion, and will have
a non-standard header. They are trivial to align and move around. 

Again, a single logical card might be folded from several literal cards. This is different from a deck: a deck is just
a type of card, with a distinct face, in which all ranks are cards. 

Lastly, we have stacks. Stacks are logically contiguous, literal cards: always contigous in storage, often contigous 
in memory. Decking a card involves putting all data *logically* onto that card onto *literal* cards, then stacking
the resulting cards in a consistent way that allows them to be shuffled into memory.

In Forth, we also have stacks. No big deal; when cards are on the stack, it's the reference that's on the stack,
so a stack of cards is a stack of cards. A card which has all its slots including the head and crease on the stack,
with the head on top, is said to be 'loaded' on the stack; if the reference is on the stack, well, it's stacked. 

We deck and shuffle cards. Sometimes those cards are deck cards.

Good? Good. 

##Decking

Decking a card for private use is very much system-specific. You're putting your own cards away, there's always a more
or less efficient way to do it. Do you have a database? File system? Both? Which kind? Neither? 

Decking a card if the change bit is zero is easy; forget it ever existed, because it's still on disk. We have no garbage
and will not deign to collect it; we rubbish cards on purpose, not because we're short on space. That would be weird.
It's good practice to flip the change bit back to 0 if a card of memory isn't going to be needed later. 

Decking a card for transportation must be canonical, clearly. I won't specify how, I'm sure Urbit has important things
to contribute here. I do know that you guys have your own 'card' that is assuredly different right now. 
I hope this won't cause massive indigestion: passing 1k UDP packets around is gentle work, and I do hope that Hoonian 
translations to the card format will occur to the Hoon proficient. If not, I've missed something. 

Basically, I'm assuming that a card can just be a hoon. If not, shame on Hoon! If so, well, I need them for my text editor,
you need a text editor, let's continue and see where it takes us. 

I also figure that the Urbit card can be a card with a distinct face saying that the ranks contain the necessary information
to calculate the Urbit function for that card. This is an eminently sensible supposition from my perspective: if you want to
go in the opposite direction I'm going to need a rapid and massive download on how Urbit cards work, and why they should be 
the superclass. Card faces have a nice convenient 16 bit field with correct offsets, to read types off of, starting at bit
16. 

Decking a card on a Forth / Labrys / Handle environment will generally be as stupid as possible while still being smart about it.
Presuming a file system, or even using one that we have, would add unnecessary complexity; decks will just be binary .dk files in
file systems, and aligned blocks in freer environments. Putting them into SQL databases is also stupid, since SQL is descended 
from the behemoths of yore which had real cards brah. I'm not going to be tampering with the schema down the line.

A noun is two slots wide per unit, a card 16. Cards are correspondingly more complex, there are in practice an infinite number
of ways to use that much space, which is all the more reason to have a really tight, canonical pattern. 

I have been manfully restraining myself from annotating every decision I've made with references to the Nature of Order and other
Hermetic texts, but when I say Pattern, if you think of the Gang of Four rather than Christopher Alexander, we are having 
issues communicating. I've spent weeks on end with Alexander and the only effort I've put into Gang of Four is avoiding their toxic
influence. I have employed all 14 forms in the cards, and can explain how if anyone is curious. 

###Size Limitation

Since cards must be stored within a deck by direct offset reference to the literal card block, I'm afraid we have a 1024 Exabit
limit in size, per deck. Larger decks must be stacked in storage. We do what we must, and it might even suffice in practice.  

## Shuffling

Shuffling is also done as convenient. In Forth, pointer arithmetic is the painless and easy way to do everything, which is the
source of much of its strength. The Forge environment should shuffle suits into consistent locations to make life easy on everyone.
Putting nouns on top is good, so that Ax may run rapidly by checking the high bit for atoms and then flooring the value to
check for true nouns. Anything in between is some other kind of card. Better, the Ax machine will be its own complete Forth
environment with its own set of offsets: it can have a meg or so under the nouns to hold cards, and the rest of Forge can 
be fast with cards since it doesn't calculate its own Ax. 

If it does need to walk a noun, it must do so carefully. A value in this case is either an atom or 'something else', many of which
are large atoms, all of which are addresses. Again, it's important to shuffle data into consistent locations; pausing for reallocation
from time to time is OK. 

## Tight Spaces

Even when you only have 16 cards of memory, cards are still a pretty good way to load it in. You get a free card just by
making it one fold and having no crease, and another by losing your headers and hard-coding the suits. We will never,
ever, have less than 16 cards of memory. Normally we have enormously more than that, and disks that allow 1k blocks 
aren't even big disks. Mostly we fit 4 or 8 cards on a disk block. 

Arduinos have 64 cards of memory. I'm trying to fit a Handle on 8 cards, FlashForth takes 4. We can use bytes 
for addressing and high bits for atoms, as usual: the compiler will keep track of atoms greater than 127. 
These small cards, if they're being stored somewhere roomy, have 'short' headers with a `1` for the zero bit. 
Eight to a straight card? No, seven; six is a less confusing number, slightly, but seven fit. An Arduino can hold
a K of such short cards, and do correspondingly less with them.  

On larger systems, compressing cards or using short cards is generally a losing proposition. It is far more common 
to need to make room for content later than to run out of space; partially blank cards are great, and the crease 
is right there if we need to fold a new card on. If two in-memory cards are a fold, and we allocated them together,
we make the crease -1: it is "false" that we are done reading the card out of memory. 

Even very large sets of data frequently benefit from being written out with headers intact and crease space reserved by
checksums. It stays block aligned, for one thing, and if we need to compress it, surely this isn't the sole redundancy.
The checksums won't be redundant in the Shannon sense, nor will they be needed, so compression time is the right moment
to remove them. Everything is well aligned for rapid movement.

This is hardly the first time some bright fellow has allocated a block 16 * n bits wide with a 0 header and an F reference.
Nor is Nock the first instance of a cons cell. These are good traits to have in a data structure: if you're the first person
to need it, the chances are that you're weird, not that it's useful. 

It's enormously freeing, and key to composability. Objects suck mostly because there's no clear place to put more object,
making them compact is just putting spaghetti in a bowl. If you want to define a data structure, or a mixed data-code 
structure, we can put that on a card for you, and give you a nice noun that can use it. Or, depending on the structure,
we can put it in a noun, and write it out on cards. 

Bright ten year olds at the right summer camp could be taught to read cards, in binary, in a pleasant afternoon. XML it
is not. The mnemonics help as always. Can you have a straight flush of strings? Indeed you can. 

Cards replace a lot of the need for jet assistance of many common noun cases. They're already specified in an efficient,
native quality way, which may be spoken to in AxHoon without slowing anything down: it amounts to a 'cheat' operator 
that replaces `br` when we're selecting off a card.

Reading cards into some benighted ancient environment is a matter of writing a library, a week's work including unit tests.
A week only because you need an Ax machine including Forth interpreter, because cards can have instructions and often do. 

It's been said there are more Forth interpreters than there are significant applications written in Forth itself. That's 
unfair, but implementing Forth is so easy that dozens of people have done it just to learn Forth. Hundreds more have done it
in anger, and I haven't found an environment that doesn't have one.

## Middle Ground

For systems larger than Arduinos and smaller than our 1024 Eb limit, a file system is an encumbrance. We may pull any block
directly off our storage, put it into memory with a consistent mask, keep the mask, and put it back if we've changed it.
Garbage collection, this is not. Periodic defragmentation is harder, since the block references are direct and stored on the 
cards. In principle, you'd have to read references off the skull, in practice they'll be easier to find than that. A reference
to a card means that card is on the earl card, and we'll keep such cards together when we can; links are for cards that are
likely to slide around each other. This could take the form of a UUID for the deck and an offset to the card in question.

Remember, our links are a function, not a data format: a single word, the exact kind that can be stored in a book, which can
execute and find the referenced card. 

Defrag is also no big deal, we'll have useful metadata on larger systems. It can be done on an ad-hoc basis when the chip
is drawing power and bored. It's not like we have to do a lot of swap: a card was either pulled from store or generated 
into memory, and in the latter case, is presumed important. We toss anything we're not using and read it from where we found
it when we want it again. Nor will we ever have two copies of data, ever, unless we mean to mutate one; that would be weird. 

Cards aren't intended to be immutable. Nouns are a better choice for that. Cards are structured data, as ever and always. It's
just that they're so structured that there's no sense in making a bunch of duplicates until you need to change something. I'm 
counting on Urbit to provide the necessary accounting: a stupid file system can have oodles of copies of redundant data, so it's
good to put a smart operating function on top of it. 

Ideally, systems with multiple stores just stack the address space on top when new store arrives and pull it off when it's gone.
Subtracting to zero at each boundary should be a firmware-level concern; the board should tell the store handler "your offset is now
n" and it should behave accordingly. That's not an unreasonable demand to make of a USB port; they're really stupid but they 
can be compelled to speak a base, Orcish sort of Forth. 

You probably want to store links rather than refs, if the store is removable. We do have em.

Collections of cards with complex mutually interdependent relationships sounds like a relational database. Given the absolute
nature of our schema, that's just grand: we use one when we need it. 





















