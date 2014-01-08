# Card

```
: card& here 128 allot 128 + ; -- card <-
```

1024 bit block.

64 header bits: 22 `face` bits and 42 `skull` bits.

## Structure

This guide treats a card as big-Endian. Architecture may vary, but cards should be stored and must be transferred as big-endian 
and twos-complement. 

A card is 1024 bits in size, divided into 16 cells of 8. The top cell is the head, the bottom cell is the crease. The cells
between are the ranks, numbered A for Ace, 2-9-0, r, j, q, k. r for "rogue" because 'page' is overloaded and 'knave' starts with a k.
Note that the character 0 means the 10th rank.

Those could be hex values, except the head and crease are not ranks, do not have rank, and may not be referred to by rank. Also,
these are card ranks, the Ace is special being the top rank, and so on.

The first 22 bits of the head are the "face" of the card. For a standard card, the first two bit are `00`. 

The first zero means the card is not small, the second one means that it's standard. `10` indicates we're running in an environment with cells smaller than 64.  `01` means the card isn't standard, `00` means it conforms to these conventions. 

`11` is currently undefined, because we don't have 32 or 16 bit cards. If we need them, we make them. 

The third bit is the change bit, set to 0 on load. If a card is modified, the change bit is set. A change bit on a stored card means
the card hasn't been arcived. 

The next five bits, bringing us to 8, are reserved. Sometimes a clean card must have a blank top byte, sometimes it must not.

The following 8 are the 'persona', a shorthand for the card's use. The next six bits are also reserved, bringing us to 22.

The last 42 are allocated, 3 each, as headers for the 14 slots in the suite. They are called the skull, also, the Answer. To
determine the personality of the card, look at the face and perform phrenology. 

The crease is always either zero or a reference to another card. In the latter case, the cards are formally a single card. Any number of
such cards are said to be "folded", and comprise a fold. 

A single card, considered by its ranks, is considered a 'hand'. All hands are straights, clearly. 

### Head Expansion

The head may be expanded by removing the latter bank of pad bytes and transposing the 3 bits into a byte. This gives a width of 
128, with the skull aligned on bit 16, a convenient form for reading without masking, though with consequences. Cards we intend
to treat this way will benefit from a logically empty Ace: cards always stay on 1k block lines. 

The expansion is unwritten, and might be somewhat intricate. I want to hold four of the bits after the persona for extending the
persona, and if so, we'll move those four bits into the four bits **before** the persona during an expansion. Those four, as
indicated, must have the option of being blank. 

The prior two paragraphs are tentative. The point would be to expose more categories for a card, but that metadata would have to be
either cached or discarded when decking the card, and it's not at all clear that this is a good idea. The size of the card doesn't
grow, and the header now looks 'weird' so we could just say that a card with that weird looking header has a large head.  

## Purpose

Nouns are the correct **interface** for any data or code structure. They are often not the efficient implementation.

A card maps to a noun in some easy to translate ways. The slots are 0 to 15 in decimal, the ranks, 1 to 14. An Ax call would refer to 
the slots as 16-32, in any of the `br` calls we use. An Ax `if` would provide, on a microcode level, 14-30. Similarly, a BigNum is
written literally onto a card with a face and skull that show it to be such. 

The face is comfortably roomy. We intend to use it as a shorthand for the sort of card we're dealing with, the persona being
reserved for this purpose. 

We have three bits to describe a slot, meaning a single slot may have eight meanings. We use one of them to say "this slot is
two cells deep", so we can describe six two cells as well. 

In large systems, space that isn't allocated in 1KB blocks is effectively wasted. That's eight cards or 64 nouns. My least
favorite kind of waste is the kind that adds up.

## Suits

The eight possible headers for a rank are referred to as the 'suit'. It's like a type, but is purely formal, telling the stack
machine a) if it's a reference and b) how to unpack it, if it is. The eight suits are the major suits. There are six minor suits,
due to `111`.


### 000 111

`000` is literal; the bits in that slot are interpreted as values rather than addresses. `111` means the slot is 'full' and 
meaning of the data is in the next not-full slot. The other six major suits are references to data; we say this data is 'on' 
the card. When we deck a card for transport or arciving, everything that's on the card comes with. 

### The Compound Gua

`card`, `book`, `noun`, `string`, `link`, `buffer`. Those are the atomic reference suits: 64 bit numbers which  
refer to some other structure. Formally, that structure is on the card: a card is the card itself **and** all its referents. 

A suit relates entirely to the form of the number. If it's not literal, it's an address: the suits tell us how to proceed
when we resolve that address.

For example, the address might be another card, in which case, the suit is `001`, because cards add up, 1024 at a time. 

Only the cards that are on the card are on the card. The cards that are on the card that are on the card, are on the card
that's on the card. Dragon flagon, pestle vessel.

A book: if a card is executing a noun, any cell or unrecognized opcode is compiled against the book, which may contain
words that "jet" a function. I'm starting to think we don't "jet" in Ax so much as we carl host systems to our own running process. 
as I build out Handle, the structure of words will change slightly but ultimately Ax runs on a handle even if it's a hardware 
handle, and we're standardizing on the stack machine for all the known good reasons. `010` because a book is a key-value, null
terminated linked list. A Forth dictionary is the archtypal book, but dictionary is semantic while book is formal. 

Again, the card suit tells you, only and entirely, how to read in the address on the card. A suit is not a type, one of
the good reasons we don't refer to it that way. 
 
A noun, of course, is our canonical container. Cards are just nouns dressed up to look pretty and support compact representation;
a `011` is an Ax noun with Ax semantics. 3 for the Ax operator. Formally, a noun is purely a cons cell; Ax or Nock nouns are
a type, not a suit. The only use of a suit is correct data retrieval, which means the semantics of retrieval will be bit-level 
specified. At some point: for now let's just premise that we'll use the Ax conventions, which are firming up. 

If there is anything subtle in this entire file, it is the distinction between a book and a noun. The 2/3 kline is terrifying stuff, no mames. Fundamentally, the difference is that Ax values are upside down, while books are null terminated.  

A string,  `100`, is a null-terminated byte array as usual. Again, the purpose of the suits is so that we can pack and unpack cards in a 
consistent way. As the name suggests, they are adaptive and serve whatever function we put them to. 'string' may contain characters,
but Forge will store most familiar strings in buffers, which I'm about to get to. Strings might also, eg, contain Base64 encoded 
data to be turned into something else on load; if so, there will be a dictionary on some card to provide the words. 4 because our even
numbered types end in zero and are null terminated: literal data is present on a null-terminated card, a book is a 
null-terminated linked list, and buffers are also null terms. We could call this behavior 'read until true'. 

We actually use a small refinement for in-memory strings. After the null terminus (which gets padded out to the slot boundary)
there is either a slot-width -1 or an address, in which case, the string continues at that address. This is true of our buffers
also.

a link, `101`, is what it sounds like: a reference to another card. It looks the same as a card would, the non-literals are all
addresses, but this one points, in principle, not to a card but to a word that can find the card. We like to resolve at load time when 
we can. The difference is clearest when cards are being serialized in some fashion: cards that are 1 get added to the deck, cards
that are 5 get their ref stored in some fashion, probably as a word in the earl card's book. 

A buffer, `110`, is a null-terminated string with additional structure. The first byte of the buffer address indicates the offset
at which the buffer proper begins. This front area is called the 'mode' because of its usual semantics, though from a card's 
perspective, it is formal like everything else. 

The offset may be zero but must be even, making the byte after the mode an odd byte.

The first byte of the buffer proper indicates the logical slot width. If it is zero, we have a string with a mode. If it is one,
then even bytes, and the next byte is even, are metadata, and odd bytes are paradata. Any other number indicates a logical slot
width, the typical numbers being `cell`, 8, and 16 for noun buffers. The 'even' slots are always meta and the 'odd' slots para; 
the first datum after the slot width byte is always a metadatum.

Buffers are doubly null-terminated, requiring 0 in contiguous meta and para slots, followed by -1 or continuation address, as
with strings. 

To sum: a front area, the mode, followed by alternating slots of meta and paradata of identical width, double null + continuation.

Buffers aren't entirely obvious, I suppose. There are a few patterns which justify them. I put them in because the easiest 
way to make living code that's still text is to interleave characters and information in a single memory structure. 
Easy to copy and paste, fast to access, trivial to strip out: even possible, with some care, to copy metadata from one 
string 'over' another with useful results.  

I intend to use the slot-aligned form of buffer much like an a-list from Lisp land. The difference between a buffer and a book
is formal, not semantic: a book is a linked list of key-value pairs while a buffer is a null-terminated interleaved collection
of meta-para pairs. The former is sparse and may share forward structure, the latter is dense and may not, in that we may
transform dictionaries with shared structures into links but never do so to buffers which share structure, they are always 
carded in such a way as to create copies.

Some of the semantics of suits are for writing, others are for decking. The continuations in strings and buffers are intended for
dynamically growing the contents, whereas it is completely normal and expected for most books that are dictionaries to point
eventually to the Earl dictionary: it's up to the decker to decide how to store books, but it's encouraged to make one copy of
any given folio per deck. I don't intend to share structure in buffers and strings, only to specify that if it happens, the decker
**must** make independent copies of that shared structure. 

The first rule of card club is: it doesn't matter who put it on the card: Holler can shuffle and deck it. 

Those are the major suits.

### Continuations

`111` says the the contents of that slot in the rank is part of the content described by the next rank 'up'. Which is actually down,
on the stack; lengthy Hermetic digression removed.

`111000` is therefore easy to read, as is the entire inductive set: we have a single piece of literal data being stored directly on cards with 1/8 overhead that reads the same all the way to the bottom card, which has a null suit in one of its ranks. That's an in-memory (your memory, cousin) representation of a big ol' chunk of number. Cards can hold anything, or they wouldn't be particularly useful.

The rest refer to useful 128 bit numbers. Again, we have six suits. These are the minor suits; you just met the major suits. There are 
eight majors and six minors, because of our consistent treatment of Heaven and Earth. 

Please note that the low slot of the 128 bit number has a **higher** rank than the high slot, and comes after. This is big endian and
opposite of what Forth wants, so we `swap` when we read a 2slot. It means that the skull will read `111` for the high bits and 
`000 > 111` for the low bits, which is also nice.  

A very common condition will have `111` in the Ace slot and a minor suit in the deuce. We call such a hand "acey deucey" of course,
and in general the 128 bit number so identified is information about the card. 

The most important 2slots are names, stamps, and ids. That leaves three. I'm not in a hurry to fill the teacup to the brim.

Names are UUIDs. They give the card a referent which is Urbit unique: there is exactly enough room for a submarine here. You may of course have a card named Zod, in fact, you'll need one, won't you? Curtis has the real Zod card. 

Stamps as in time. Though, 128 is quite a bit of width for a timestamp. Call it an n stamp, where n is up to 64 bits wide. 

Ids are conceptually simple. Ids are very, very hard. An Id is just a hash of a card. But we often want any card that has the same
literal contents in the same ranks to have the same hash. That means normalizing all references and being entirely anal about how 
we deck the card in order to hash it. That's what we need to do to send a card also, so we'll cross this bridge later. 

Formally, these are all just literal numbers. Sometimes an Id can be just a simple in-memory checksum of a card's contents, purely
by reference. Deckers don't care about the contents of minor suits when decking a card, though they might add a few. 

An Id can't be the hash of the card it's on, for obvious reasons. I'm told this is possible but I don't believe it.  

Minor suits are only used for data which a card absofuckinglutely must be able to interpret autocephalously. Anything we can do with a book or noun, we do. 

That's why one of our three remaining slots is for addresses. If you write an address on a card, you can send it to the name in question. 
That leaves 5 and 6. 5 means you just received some Red bits, complete with all elaborate protocol involved. 6? Dunno. It's nice to quit before the meniscus in one's teacup inverts. You'll notice there's lots of room in the face. Faces should have personality.

## Arrays

If an even numbered suit is preceded by a literally suited rank, the value of which is not zero, then the literal holds the offset 
in bytes to the formal null terminus of that data. 

That is to say, if the cell **before** eg a string is literal, then the string will be treated as an array with the offset given in the literal cell. For books, this would mean that the book in question is dense in memory; we will use this more
often for our strings and buffers. 

Cards, unlike nouns, are 'head up'; the in-memory address for the card begins at the crease, so add+ load will put the crease at the bottom of the stack and the head at the top. Ax numbers are all negative in-memory, so the arithmetic adds up: 0 is the head, -1 the ace, and -F the crease. Therefore, we want the offset to land on top of the address. Therefore, the offset precedes the array. 

You'll notice that all the weird literal stuff makes it annoying to write a plain ol' literal into the middle of a card. 
If you absolutely need to write an unrelated literal before a null-terminated string, you can afford a blank rank between them. 
This rule can burn you with unexpected behavior; life at the low level. Cards are designed to hold *compound* data, and have 
personas so you can keep usage straight. We're at the bottom level here: if you just want to store a value, store a value. 

Note: if we have an array string or buffer, it may contain null bytes or cells respectively. 

Forth being what it is, this is the prefereable in-memory form of strings and buffers. C world will appreciate our null terms more,
because in C, you don't use an abacus. You grit your teeth. 

## Personas

The eight-bit persona is for the environment's use. A persona could be masked out without affecting the load or read semantics of the 
card in question, and it's a bad idea to make the card useless without special knowledge of the persona.

Personas are there so I can pull the third byte and determine what my card is for: no muss, fuss, header decoding, or anything else.
Such a card is called a Trump card, obviously. So if I have a card in Forge that represents an edit frame, it'll have a persona that
tells me so, while the rank and file of the card will contain the books, modes, buffers and other arcana. The croupier just decks it
and shuffles it like any other card. 

### Summary

That strikes me as the level of formality needed to implement. Let's review:

Card  = 16 'slots' of 64 bits = 1024.

0 slot is head, F slot is crease. Those are Ax numbers, hence, upside-down.

The head has a 22 bit face with an 8 bit persona, and a 42 bit skull.

1 through E are the ranks, numbered A, 2-10, R, J, Q, K. F is the crease.

The suits:

Major

* `000`  Literal
* `001`  Card
* `010`  Book
* `011`  Noun
* `100`  String
* `101`  Link
* `110`  Buffer
* `111`  Contd. 

Minor - tentative

* `001`  Name
* `010`  Address
* `011`  Id
* `100`  Stamp
* `101`  Red
* `110`  ?


###Forge

Yeah so, I was going to write a 'text editor' and needed a data structure. I'm gonna roll with it, once the hangover settles and I get the last bit of my JobJob written out. 

I have a bit of dictionary writing ahead of me, and maybe some C patching to properly capture keys. 


###Reading a Card

Pull the head

Bitmask for the suit of the rank you want

Pull the rank you want with a suitable handler.


###Cards in memory

Though I brushed over this, cards are stored face-down. As you would expect! The crease is aligned with the block, and Kings 
are high. The first slot of each block tells you if the block is part of a fold or the end of one. Bulk modification of 
cards is generally concerned with masking the refs found at the crease, if we want to move say a 16 MiB piece of disk around
we're going to have to mask at least some of the crease references. 

I can only offer a brief, weak apology for my habit of referring to the 0 block of memory as the 'top' or 'up' and whatever's 
at the bottom as the bottom. For now let's just say my internal compass is wack. It has the dubious advantage that you have 
to read me carefully to know what I'm saying...

It does happen that stacks are routinely put in high memory and grow down towards the heap in low, see this Arduino diagram:

http://learn.adafruit.com/memories-of-an-arduino/arduino-memories

Which lets the heap protect the core routines, or at least if we segfault we do it after plowing through our own software. 

Scalable card handling, well, it's not premature optimization to be thinking about it. The card format will be painful to 
change by the time I'm concerned with scaling up or out. 

It's worth pointing out that buffers, for example, might actually be held on cards in memory. If we're working with text
it's easier to chunk it, because we routinely add to it in an arbitary order. Keeping those creases around is likely to
come in handy. We can store 96 bytes on a standard card, which is 48 columns, which is about right. 

We use frame cards to organize buffers, so things like the offsets into the buffers will be stored on a frame card. Frames
can be preallocated with say 2 x 256 cards, so 256 lines or so of text when we create a frame. 











