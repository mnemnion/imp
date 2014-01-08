##Memory Count

bit = 1

byte = 8 bit 

kB = 1024 byte

char = 1 byte

cell = 2 byte

osMem = 1 kB

eeprom = 0.25 kB

lexicon = 78 char

(eeprom - lexicon) / 8 => 178

a = (osMem - lexicon * 2) / 16 => 434

b = lexicon / 2 / 8 => 39

a - b => 395


##Countdown

osMem = 1024

eeprom = 256

byte = 1

cell = 2

inner loop = osMem - 16 cell => 992

checksum = eeprom - 2 cell => 252

(lexicon - 2) * 4 => 304