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












###Not So Great Calculation

core offset = 5 bits

nextword = 1 cell

vocab = 78 * core offset * nextword

remaining = ( osMem - vocab ) 

remaining => -6,240 bits + 8,192

vocab => 6,240 bits

kB => 8,192