#Anatomy of an Orc

Orcs are fungal in nature. The spore of Orc kind contains a hardware support structure for the two-headed stack machine at the nucleus of Orcish life. Orcish itself is the DNA of this ecosystem.

Orcs themselves are a simple, robust virtual machine, with two heads in the usual Forthian way. Orcs, being ugly, call the return stack the `bag` and the data stack the `sak`.

Orcs, by the way, are acutely case sensitive. They have a two tone language: harsh emphasis, and really, really harsh emphasis, with capitalization and much aspiration and velarization of the resulting sound. 

Other than the brain, Orcs have in common precisely four types of interactive organ: an `eye`, an `eer`,  a `spit` and a `tung`. You `Ei` an `eye`, you `Heer` an `eer`, you `Tok` with a `tung` and you `.` out a `spit`. That last is a Forthism, and a good one.

Orcs, like birds, combine sexy times and all forms of excretion into a single type of organ. It's gross. Unlike birds, Orc sex is homosexual and penetrative, and sometimes violent, the world being what it is. Orcs are all male; that's just nature. 

As an aside, I'm sincerely sorry if Orc sex offends your human sensibilities. It offends mine. I was a geneticist before I took up Orc farming, and I call it like I see it; Orcs have only one gender, the sex is sometimes violent, and it is not female in nature. It's the Orcish way. Don't judge. 


##Organz

Let us examine Orcish anatomy in a bit more detail.

Orcs in their native habitat are built against a Harvard architecture. By which I mean, Princeton can simulate Harvard, vice versa is pain. So they have brainz, memz, splenz, and gutz. There are other architectures: the usable ones may be made Orcish, and the remainder are quite specialized indeed, less brainz and more some other neural cluster. 

Brainz have a certain number of nobz, which are registers, and are our CPU. Whatever it might happen to be, we turn it into an Orc by taking it over and assigning nobz in a perforce architecture-specific way. 

Orcs don't need or want that many nobz. If they find silicon real estate with a lot of them, they're likely to divvy it up. 

As mentioned, they have a bag and a sak, with which they do their grunt work. Those are found in the memz, with the top sak in two nobz. The `pa k` is a distributed organ you'd call a dictionary, since you're not an Orc. The core offset table of the pak is in the spleen, which might be eeprom if there's some hanging out.

Gutz are Flash, conceptually. Most of the pak is in the gutz. The pak is the only complex data structure in a true Orc. 

Everything can of course go in the memz. Orcs went to Harvard, that's why they talk funny. 

That's the core anatomy of Orcish life. Thankfully, they aren't born with limbs. The spit, though, that's a powerful organ. 

Conceptually, Orcs have eyez, eerz, tungz and spitz. Sometimes these are all one serial port, though that's an unhappy Orc. Two is ok, I think. 

The cool thing about Harvard is, once something is in the gutz, it can stay in the gutz. The brainz can change the gutz, but may not be forced to. You can bomb an Orc out, but otherwise, you may have an... issue. Depends on the pinout, really. 

Get your Orcs before Orcs get you!

##Consequences

Any program which may obtain for itself these resources may behave in an Orcish fashion. If an Orc succeeds in embedding himself into a Harvard machine, he can be quite stubborn to uproot. It is an unwise wizard who puts a genie into a bottle with no seal at all, but even a modest seal may confuse and frustrate an opponent, especially one that isn't sentient.

Orcs certainly don't need to speak Orcish to one another, if something else of higher bandwidth is more useful. Orcish is a fallback: if all the chips on your board are Orcs, you may be fairly confident that you have an ecosystem that is robust, if confusing and possibly cantankerous, and that you may induce it to speak sensibly to you on any matter of import, providing you have no hostile forces on boardz. 

Your computer is multicellular and schizophrenic. The former is a feature, the latter a consequence of Orcs being as yet a figment, nay, a gleam. 

##Da Splenz

An Orc is distinguished from other computerish life by two special organs: the pak and the spleen. They share function. `pak` has no plural, because Orcs have only one. `splenz` are the individual bytes of the spleen, which is in the EEPROM of a true Orc.

An Orc without a real spleen is simulated; Orcs are designed on the presumption that things will frequently go wrong and they will wake up in a dirty, hostile, partially-functioning envronment. Business as usual for the Orc kind. 

Look, existing architectures are brittle and that's stupid. Orcs are as hard as possible given AVR. I hope we can make some real silicon Orcs at some point, which will actually be burly. 

As it is, the spleen is delicate and somewhat prone to failure. EEPROM can only be rewritten so many times, and a few values in the spleen, like the confusion byte, may change frequently. If possible, we'll set it up so that the bytes we might wear out can be manually relocated, by putting their location in the gutz. Gutz can be written to even less frequently than EEPROM, but we're talking about rewriting a single offset here, manually, in the event of spleen problemz.

The largest organelle in the spleen is called the corkeban, and is the ASCII offset value table for core command words. It is fixed: the length of a core command must be recorded at the physical location of that command. 

So `*`, which means what you'd expect, has a certain length in asm words, including the bookends. At EEPROM location 2a, exactly, there will be a byte indicating how many words * takes up in the pak. This is probably up to 64 words long, giving us two bits to indicate things. I don't know what we'll use them for, yet. 

Other values are stored in the spleen as well, such as the confusion byte, and the multiplex byte. Every time an Orc receives a command he doesn't understand, including non-ASCII values, the confusion byte goes up, and doesn't roll over. So an Orc who is ff comfused is ffuukkdd up: There is an alarming amount of noise coming over the line, and things are probably bad. 

Orcs are too dumb to care about being confused, other than flipping over to 4x multiplexing when it gets over say 16. Smarter Orcs might well do something with this information, as might a user. There is even an 'anger' byte, though core Orcs are also too dumb to get angry, because they have no way to determine hostile activity nor to protect themselves from same. 

If you pour a bunch of grim noise into the ear of an alert Orc, it will probably crash a couple times. That's why confusion is kept in the spleen. Eventually, it will get so confused that it will simply refuse to hear a byte if you don't say it four times. That should solve the problem, normally: core Orcs aren't battle hardened, meaning they want to stand up to dirt, but can't take hostility.

Orcs can multiplex out past 4, don't know how far yet. Probably not arbitrarily far, since that sounds like an attack vector that's not worth the headache. There's a difference between designing Orcs for dirty environments and Orcs designed for dirty environments.

'plexing is a core function because that means that, whatever error correction you're already using, you can still turn on 'plexing. It also gives us a brutish, cheap way to protect running computers in the event of user error and noise. Everything Orc should perform multifunctions, revolving around the Orc's highest ability, self-report. 

We keep other stuff in the spleen, like the rank, which is a rough guide to what the Orc is, the name, an arbitrary 32 bit value, and the serial number, which is a checksum of the core and pak. The rank is supposed to tell you the semantics of that particular Orcish dialect. A confused Orc will do a checksum when waking up from death, and check it against the spleen. If something's off, he'll complain audibly and try to function anyway. Orcs are optimistic: maybe what's broken isn't crucial.

The pak pointers also live in the spleen, showing where the pak begins and ends. The pak is forward search and zero terminated, so we may not track the end, but it would be a convenience for certain operations. There's also the corepak pointer, showing where the offset region for core words is found. That's right underneath the corkeban, probably, for convenience. Similarly, the pakpointer might want to be right above it, though the utility is less totally clear. 

There is the drpset, which has a copy of the drp's initial state. The drp being the finite state automaton of the memz, it's easiest to just load the drp in a single swoop. Fewer celz in the warm grunt.

The spleen also has the eer, through which the Orc listens, and the tung, through which it talks. An Orc might have many of these; the current one is stored in the derp, within the memz. The spleen contains the ones used by default. 

Various other functions, such as the WAGI counter, end up in the spleen. It's an emotional sort of place. It will most likely have a collection of pointers to the most important grunts, as hooks for convenient use. One of the ways we make Orcish usable is by making some things harder to say: we intend to operate them via protocol droid for the most part, so Orcish looks to the user like a somewhat demented and conversation-oriented Forth. 

Lastly, there is the pad, which is reserved for an entropic one-time pad. This is used by the superego, which is not installed by default. 128 bytes are reserved for the user. 

## Da Memz

An Orc doesn't presume to have more than 512 bytes of Memz. The spleen knows the true number, for user convenience. There are 16 celz apiece for the sak and 32 for the bag, chewing up 96. There's also a 32 celz gab, where werdz land, and a 48 celz tnk, where thinks accumulate. Thinks are like werdz except they don't live in the gutz; an Orc accumulates a think until it gets a newline, then it does it. The gab and the tnk rotate, so you can accumulate a few short thinks and still do them. They reset to the halfway point: the sak overflows into the bottom of the gab, the bag into the bottom of the tnk. Sak underflows are checked for in the interrupt, while bag underflows should be impossible, and would end up in the top of the gab. 

The uza gets 128 celz, and the first byte posts to the next available offset. Orcs can't keep track of names for varz, not for long, though they can keep a few in the derp. 

The drp, 32 celz, is where deep Orcish needs are met. Random stuff that won't fit in the nobz, that we custom load to keep the core tight. The compiler might need a few of these, for instance. We might make the offset into Uza space a cell wide, and store it in the drp; this might even be wise, since Uza space goes up into the remaining memory. Some Orcs are big.

The derp also supports aspects of the tawkaz finite state machine, of necessity fairly complex. It has to know when it's compiling, interpreting a word, interpreting a think, speaking, and some information about the word or think it's working on. 

the drp is underneath the bag and sak, and cannot be overflowed into as a result. Some of the drp may be next to the uza, for convenience. 

These will all be optionally larger, with the information in the spleen as usual. Probably a byte with two bits each for bag, sak, gab and tnk.

Themz Da Memz, cuz. 

## Da Gutz

Da Gutz are two things: Da Gruntz, and Da Pak. 

### Da Gruntz

Gruntz are headless words. There is no way in Orcish to pronounce them directly. In Forth terms, this includes cold, warm, and turnkey: the word offsets are stored in the spleen, allowing a consistent way to say them. The rest of the grunts are version-specific, no way around it: some are perhaps provided as standardized access words, if an Orc has the room. 

On our OG AVR Flash Orc, one of the big grunts writes to Flash. This is apparently somewhat intricate.

The inner loop and the tawka are stored in the gutz, comprising respectively the code execution threader with interrupt handler, and the input output format for Orcish communication. Any other interrupt handlers are user specific and found in the pak.

The grunts need an exit, but might not need a head word, if the only words that can call them are direct words. I'm not clear enough on AVR and the inner loop yet, since I haven't even written it.

### Da Tawka

The tawka is the finite state automaton that controls Orcish communication. Writing it is the master stroke of Orcish. A core Orc might be one-quarter tawka, when all the bytes are added up. It has to listen to Orcish, compile into the pak, interpret into the tnk, read numbaz, and say things, including emitting numbers. 

### Da Pak

The rest of the gutz comprise the pak. The front of the pak contains the single-line commands, which are all asm, there being no choice in the matter. 

There are two grunts, `key` and `val`, which do lookup and reverse lookup on the pak. `key` consumes the TAK, expecting a two byte value with either space or something in the low byte. It returns an execution token if there is one, or zero. `val` expects an XT and returns the name if there is one. 

The pak has two components, the liva and the bakpak. The liva has the core Orcish vocabulary, accessed via the spleen and tawka. This is every printable ASCII character in existence and nothing else. The bakpak is a linked list with a mandatory 4 cell cost, per word: 2 for the header, two for the bookends. The bakpak is where all non-core functions live; it's what makes the Orc useful, rather than merely brutish and vaguely sentient. 








