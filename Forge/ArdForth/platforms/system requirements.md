#System Requirements

The entirety of OrcOS will fit in 1kB of Flash with no more than 256 bytes of eeprom. It will be compatible with the Tiny AVR, and fit in an Arduino Uno with enormous room to spare.

Uruk will be designed around an Arduino Uno, with no particular effort to leave room for other purposes. It will require 32k etc, though of course will not in itself fill it. 

The development platform is an Arduino Mega clone with the 2560 chip. 128k Flash, 8k RAM, 4k EEPROM. We'll develop the assembler here, and add the type system if practical, though that might run on-laptop. 

Forge is the development and interaction environment of choice. Our Mega will grow an SD backpack pretty quickly, so we can perhaps, maybe, just barely fit Forge on it. Or at least Forge hooks. We'll be happier if we can actually run Fabri on the Mega, and that will keep us from adding bells and whistles that might prove counterproductive. 

The first version of the OS will make no special effort at optimization, although we'll design and use the corkeban etc for portability. Everything will be indirect threaded and we may use macros for NEXT and EXIT instead of jumps, for speed. We're targeting 32k and can most likely afford it. 

Then we'll compact it as much as we need to, to fit the system requirements. The result will run on Trinkets and other 8k controllers. My thesis is that no one will bother making a controller smaller than 8 bit 8 k, ever again. So if you can Flash it, you can Orc it.