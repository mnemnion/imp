#ChitterBot

Uruks are cool and all, and protocol droids are excellent things.

We aim to have 7k inside a Trinket, and 256 bytes of eeprom. Let's say we need another 1.5 of the flash for Orcish USB matters, leaving 5.5k. 

A ChitterBot fills the remaining space with dictionary. It can do two things: translate the Common tongue into Orcish, and reverse this process. 

5.5k is a fair amount of verbiage for programming tiny Orcish systems. Chitter will run the Uruk core, which dispenses with compression in favor of a thesaurus (Common and Orcish) for the fundamental words. 

ChitterBot is a key part of the chain, because no one should ever speak Orcish unless they take pride in that kind of masochism. It's okay to learn the core words, and all, but only if you've been Forthing for a long time and want to show off at the command line. Truth is, you need an intermediate imp to talk to an imp at all: an imp that speaks USB already has its hands full, add an OS and you're almost half full. ChitterBot won't have a lot of leftover Flash, but it will mean talking ordinary Forth to your imp is as simple as plugging it in. 

An Uruk is a smarter intermediate if you're doing a lot of microcontrol. A Chitter will have to be reflashed to speak any given Orcish dialect, while an Uruk will carry around a whole library to communicate with underlings. It can reflash itself, but it can also buffer from an SSD directly into RAM if it's feeling roomy. 

The design goal with ChitterBot is that it will always produce the same strain of Orcish from Fabri code. This is necessarily somewhat interactive with the imp being loaded, since you have to check the dictionary when 'create' is used to see if the gensym or literal pair is present. The algorithm is still deterministic: we're either providing a dipthong or gensymming, which has a consistent order. If we're providing our dipth, and it's present, we have an error, since redefining Orcish is illegal and silly. If the gensym is present, we make the next one and try again. 

Why even offer the option of specifying the dipthong? Well, there are a lot of cromulent Forth words that are dipthongs, like `s"`, that won't make it as core words. We may as well use them; Orcish isn't designed to be unreadable, that's just a side effect. We'll have a special defining word that passes a name through so a device can have actual commands mixed in with the Orc. 

This all makes for slow loading: a laptop can brew up a full and consistent imp load in a heartbeat, and a Chitterbot can just push that over the line. It's more than fast enough for interactivity: we think we type fast, but we don't.