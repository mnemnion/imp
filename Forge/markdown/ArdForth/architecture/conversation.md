#Conversation

Orcish looks like a computer programming language.

Orcish is a computer *conversation* language. A programmable protocol that fits in a kiloword and change. 

Therefore, it only has as much Forth in the command set as absolutely necessary. Most of the brain is given over to understanding and emitting the Orcish language. Since you can teach an Orc a word, temporarily or permanently, this is a reasonable tradeoff.

##Orc Forth

Orc Forth is the minimal computation core. We're going to learn from eforth and colorforth.

Here's [colorForth](http://www.colorforth.com/inst.htm), here's [eForth](http://www.figuk.plus.com/build/heart.htm). Two tricks I'll borrow from Chuck's latest thinking: `;` will compile an exit, not end a definition, so we can have multiple return points in a word. Also, new words are defined immediately, so we don't have to `smudge` or have a `recursive` word or anything dumb like that. Orcs use a forward dictionary and won't accept a token they already know. 

Either way, I estimate we'll need ~48 instructions. Some of these will be grunts: words without commands. Orcish only exposes a word if it's useful in communication: the extended command set provides enough Forth to get the job done. Orcs specialize in what they do: the common nature of Orcs is that they can do anything you tell them (if they're smart enough), and that they can tell you what they're able to do. 

Self-documentation is difficult to imagine squeezing into 1k words. We're not planning on putting any other capacity in there. A general-purpose Orc in a roomy chip will have 2 to 4k words of core ability; the 1k OrcOS provides hooks so dumber Orcs can do smart Orc things, albeit slowly. 


##Integrity

OrcOS will have an Adler32 checksum built in. Conversation can switch to a multiplexed mode, where each ASCII value is sent multiple times and is 'acked' the same number. Orcs have the ability to freeze, and dump their spleen and gutz into ASCII over the line. 

Orcs checksum themselves on a 'warm' start. If they fail, they complain and try to operate anyway. 

Philosophically we consider it much more important that an Orc keep working, than that it be convenient to program. Orcs keep a 'confusion' byte in their spleen, and every time they don't understand something, they accumulate it, decrementing when things make sense. Orcs don't complain when they don't understand you: since you can query the spleen, you can figure out manually how confused an Orc is, or use that information inside the Orc itself. 

Eventually, a confused Orc will start multiplexing. You have to multiplex back until it calms down.


##Conversation

Orcs eerz are immune to the non-printable ASCII spectrum. They will silently drop anything but a glyph, a space, or a newline. Notably, that includes tabs. A cheap and low prank is to stick tabs into Orcish in places where you'd look for a space. 

These values are simply invisible: the first thing an Orc does for any data coming in off an ear port is reject unprintables. Mouths are not similarly gated at an OS level: you may spit or speak from a tung port. If you expect control sequences, Unicode, or anything but Orcish, use an eye. Orcish is a deliberately constricted ASCII subset: If you can't copy-paste it successfully, it's not Orcish.

Note that even archaic systems which use `CR` and `LF` at the end of lines will work fine with ORCs, who have never heard of a `LF` and don't care about them even slightly.

There's no room to make Orcs that speak Orcish, in the core OS. Speaking is hard, we'l have plenty of fun just making them understand Orcish. It's a very simple language: every Orc knows all single-letter commands. Even making that possible, in 1k words, is daunting. We have only 9 machine instructions per character-word, and we know that a few will take quite a bit more than that.

###Scenario

Let's pop out into the real world as experienced by humans for a second. This should illustrate why we have a fantasy world as experienced by Orcs.

Our user has bought a used fabber off UrBay, and it doesn't entirely work, which is why it was cheap. The vendor's English was not as good as her Roumanian, so documentation is slim. Fortunately, the fabber is chock full of Orcs! 

Now, this is the grim cyberpunk future of the 20s, so the fabber, like any worthy machine, is custom. It's running something weird, cobbled together as a cooperative endeavor between Orcs and higher level computers. 

So we've got a robot, and something's not tracking right, and one of the microcontrollers is suspected. A bit of playing around with a voltmeter shows that substantial noise has crept into the trace, and it's not clear why just yet.

So she hooks up a protocol droid and starts sending `WAGI WAGI WAGI WAGI WAGI` to some pins on the suspect chip. Good luck! She gets back something like `Oc! O(n!eOr=! Orc%+`. So she sends the 8x multiplex command to try and surmount the noise problem, and starts asking the Orc what it's able to do. This Orc turns out to be the control Orc, not the sensor Orc, but the sensor Orc is upline.

The sensor Orc is just fucked up, because it turns out one of the traces has a piece of swarf stuck to it, pulling overvoltage from a capacitor into a feedback sensor and causing the whole thing to shake up and down. It can't even stay up long enough to get into a multiplex, though she does manage to reset it, causing it to occasionally get as far as `Orc!` in the turnkey before falling over again. Shrapnel removed, the Orc is pretty fried, but after swapping in a fresh chip, the backupBot passes a fresh copy of the Orc back along the line and everything is working again.

At no point was the Web consulted. There is no remote documentation. This is a machine designed to last, by people who care. 

Orcs aren't smart. They're sentient. 

##Protocol

Orcish communication is all plain text and can be conducted over a single line. Orcish is radio-friendly. Not that a 1k Orc can operate a radio, of course, but there are some mightly roomy chipz out there. 

Orcs reply to you under one circumstance: when they understand what you've said to them. For most commands, they will then repeat it back, in numbaz. An orc assumes a byte is a numba first, so any of `0-9a-f` will be interpreted as value commands. So if you tell an orc `*`, ASCII value 2a, followed by a space, he'll send back `2a`, just like that, no spaces. He'll also multiply the top two sak values and put the result on his sak. 

That's a dangerous capability for an open line. All Orcs can be told to be surly, in which case, they're looking for one command: `WA`. Orcs eat commands two bytes at a time, so they see `*` as `*_`, mul-space. If they hear a `WA`, they want a `GI` after it, no space, and then a space. Orcs hear multiple space marks as a single. 

If they hear a WAGI, they increment the WAGI counter and start looking for another one. Again, Orcs reject without comment any input they don't understand, so a whole lot of garbage could follow, and the Orc will just keep interrupting, looking for `WA`, and going about his business. The WAGI counter is in the spleen, so it survives resets: even a seriously broken Orc, if it's still core looping, will eventually accumulate WAGIs, presuming you keep sending them. 

An Orc who captured three WAGIs will send back `Orc! Orc! Orc!` and go into 'ready' mode. Depending on confusion level, he may start multiplexed, in which case you might see `OOOOrrrrcccc!!!!   OOOO` etc. If so, the user will have to match the 'plex level, which may be stepped down if the line isn't actually noisy.

This may seem dumb. If you've got an Orc with a weak radio at the bottom of a mine shaft, you'll praise Bob for this. 

We use simple/stupid repetition rather than try and add a multiplexed intelligence because the Orc is probably busy doing something. Looking for `WA`, we can afford, in time and space, barely. Nothing else. These are microcontrollers that often make real-time guarantees to their matez. 

###Another Aside

I may have to abandon some of this complexity in the pursuit of the core. It's really rather small. I'll keep as much as I can, because communication is the only other important function besides computation and basic system integrity.

##Modes

Like any Forth, an alert Orc has two basic modes: interpretation, and compilation. They don't work quite the same.

In interpretation mode, an Orc understands words a command at a time, and replies a command at a time. It does things when it hears a newline. As mentioned, Orcs reply in hexidecimal, but there may be a twist: we have an 'ignore' command, which matches with itself, so `\ 22 22 * \` does nothing at all. It's kind of like a comment, but we can't afford matching braces, that's two whole commands just for ignoring things. 

The reason is that an Orc who hears a number will normally put it on his sak. We'd like to avoid that behavior if two Orcs are talking to each other. Which, two entirely stupid Orcs won't be able to do that, since a core Orc doesn't have the gift of speech, per se. They can make understandable noises, but so can a Barbie doll.

Still, Orcish is designed to be brutish and leave little to the imagination. Replies may as well be verbose: Orc kind dialects out pretty quickly if things like latency are major problems. Since an Orc could easily tell another Orc a number, the reply to a number like `23` is `\ 23 \`. "ignore this number: 23."

If at any point an Orc doesn't understand something, he will do absolutely nothing. This is a design feature. 

Zooming out to the Forge environment, if typing to an Orc using ordinary Forthish, each command is sent when you type a newline. One word at a time. If the Orc understand the word, it turns green. If it fails, the word turns red, and the rest stay amber. They haven't been sent, you can backspace and delete/change them and hit enter again, it will start from the next non-green word and keep trying. If the command was good, the whole line turns green, and actually pops up a line giving you a fresh start. 

On a concrete level, the Orc is searching the pak each time it recognizes a command, then storing the execution token in a RAM buffer. When it gets a newline, it executes the quasiword it was just given. If everything is Orc, it says `Orc!` just like that, no spaces. If not, it uses the `WAGI!` protocol to complain. 

###Rationale

So when an Orc sends a command, it can retain that command at TOS with a simple dup. The `\` command redirects input to the gab, and retains the offset count, so the Orc can trivially find the reply. The reply will always be numbaz (this is not inherent to `\`, it is context-specific to command-reply situations), so converting the value and putting it on TOS will give an identical result whether the command was in numbaz or lettaz.

Since Orcs don't reply if they don't understand you (there are exceptions), we need to establish a timeout counter, after which the reply is considered to be 'what?'. This is far beyond the 1kword capacity of a core Orc, which again, won't send commands without further training.

##Queries

The most impressive thing an Orc can do is recite a word that it knows. If I can fit this ability in 1k celz of gutz, I'm a wizard indeed. It's more important that Orcs have this ability than that OrcOS be 1k celz. 

I won't compromise on this ability. The entire philosophy of Orc kind is predicated on finding a dusty, oily machine in some forgotten basement and being able to do something useful with it. Even the old alien anthropologist should be able to get them going, assuming they've managed to suss out Orcish. 

Since Orcs are custom, they need to be able to describe what they do. Merely belching their data out isn't sufficient here: Orcish fails if it relies too heavily on communicating assembler level instructions. Acorns and AVRs and Intels and Etc. need to be able to *possibly* get along with some core vocabulary: nothing about Orcish communication is 'implementation specific', celz are 16 bits which is always the sak width etc. 

Orcs can report themselves in three basic ways: checksum, hexidecimal conversion of regions of memz gutz or splenz, and a readout, in Orcish, of a command's meaning. 

The last is reused in compiling. When you send an Orc `:`, it's prepared to learn a command. If you send it one that it knows, it recites the command back at you instead. Otherwise, it repeats the command in hex. This is an important metaprogramming feature: you can teach an Orc a new word, in ignorance of what it already knows. If it knows a word by that name, it will tell it to you. 

There are also certain commands an Orc is 'supposed' to understand. That is, if it doesn't have a word for that diglyph, it will complain in a specific way. This is one of the compromises a 1k operating system has to make. From the user perspective, these commands are alwyay available, because a protocol droid can spell them out a few words at a time. We give the impression of a much larger and richer programming language than we actually provide. Don't write Orcish unless you have the inclination; it's moderately demented stuff. 
To put it in perspective, a 32k Flash system with 2k each of EEPROM and SRAM will have plenty of room for the entire command set, with room left over to do useful things. Your 8k tiny controller might be a little too busy just being a stepper. But you might need to change a couple numbers in the spleen to compensate for wear on your backlash nut. Or something. 

There is probably one command, `barf`, that makes an Orc do the barf thing. Three stack arguments: end region, start region, and barf number: memz, splenz, gutz, or werdz. Three options remain, but we probably don't have the celz to implement uses for them. 

##Tokin Werdz

If asked to speak a word, the Orc must first find it, which is an ordinary dictionary search. It must then find out what kind of word it is. If it's a one letta, it already knows. How can it tell? It's just looking at the last word it saw, which it stores with the space if it's one letta. So the middle stack argument of the 3 will be a space for a one letta, and a letta for a two letta. 

We check for space so much we'll probably have a special grunt for duplicate-and-check-for-space. 

In any case, one it finds the word, it checks the top bookend to see which kind, direct or indirect. If direct, it hexidecimals the string a word at a time. If indirect, it calls reverse lookup on each word, which is hard.

It loads the core offsets from the spleen, and does a range check. If it's in the spleen, it back-calculates which letter by the ordinary arithmetic. If it's not, the pak is searched until the correct offset is found, and the word read off in ASCII. 

