#OrcOS

OrcOS is a simple project: design an interactive operating system and shell for the [ATTiny](http://www.atmel.com/devices/attiny85.aspx) series of controllers, and larger members of the AVR family.

The OS itself is constrained to a single K of memory and 256 bytes of EEPROM. This will include a full interpreted shell, in Orcish. 

The development environment will run on an Arduino Uno, and will allow for communicating with OrcOS using a fairly ordinary dialect of Forth. 

The only folder with original code is Forge. ArdForth contains the core functionality of OrcOS. Careful perusal will reveal that there is none. There is a lot of unfocused discussion, with historical layers of ideas; rough notes.

When my Arduino arrives and I get AmForth and the assembler running on it, we'll be in business. 
