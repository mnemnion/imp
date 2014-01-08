#Roadmap

My, oh my, is this a lot of Markdown.

Assembly programming is done with a pad and paper. This is okay, even if it's my personal bad habit.

I really should get around to polishing up Marmalade but that's a deep dive, and in the wrong direction right now. 

My platforms are finally arriving. Here's what we've got to work with:

##Gizmos

In order of power:

1   SainSmart Mega		atmega2560
1   Arduino Leonardo 	atmega32u4
1   Arduino Flora 		atmega32u4
1   Arduino Uno         atmega328
1   Arduino Light USB   atmega8u2
2   Adafruit Gemma		attiny85
6!  Adafruit Trinket	attiny85

1   Adafruit loader cable (name?)

Giving us 14 kuntrolaz. Not bad. 

##Plan

First step, we're building AmForth with all the bells and whistles for the Mega. It's a comfortable environment. 

Next, we write our own Orcish assembla. All our code needs to be MIT, not GPL, so we're hand-rolling everything, and documenting our inspirations. There's also a lot of public-domain code we can borrow directly, though Forth and Orcish being what they are, this may not work.

It's a bootstrap by definition: we use Forth to write reporting and query tools, which we translate to Orcish later. We use them to debug the assembla, and to start laying out memory and writing the core gruntz. We then go to work on the computational core of Orcish, write some simple self-reports, and put them on a Leonardo-class machine. Boom. 

We'll be writing Forge in pieces, and annotating the code in a Fabri-usable way (though we have no Fabri, as yet). This is where we want a really nice tool for memory visualization, the kind of thing that can disassemble assembly, reverse lookup execution tokens, and otherwise show the meaning of code in a structured layout form. This will require using Fabri, at least on the laptop. Debugging Orc is going to be a lot tidyier if we can run Fabri code on the Mega as well. 

Forth shines at this, fortunately, since it's a lot of shlepping around data without paying much attention to what it is, the value and location. With just enough high-level support to let us manually keep track of that stuff. Which poses a chicken-egg problem until our report tools are reliable, but that's bootstrapping. No substitute for care and attention to detail.

Eventually we'll have a decent little Forth core, with self-report. At that point we need an ANS Forth to run on top of it. This is easy and pleasant, seriously: it's a matter of picking through a half-dozen public domain Forths and choosing elegant, compatible implementations of the words. We'll keep a bibiliography. We're nice people. 

This will be the core of Uruk, our Forth speaking Orc. Its pak is a specialized organ containing both Forthian words and their Orcish equivalent; many / most ANS Forth words will have Orcish command equivalents, though some will generally be inlined. Uruks are able to command multiple Orcs, so they use a somewhat large pak structure to keep track of which Orcs use which werd for what. 

Then we put the OS on some gizmos and write a few specialized Orcs that do cool things, mostly blinky lights and sensor control. At that point we'll hopefully have a nice Possi going. 

At that point I hope to put my attention behind Forge and Fabri in earnest. Also to have figured out a way not to go broke in the process. Life on the cutting edge...