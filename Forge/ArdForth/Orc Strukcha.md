Orc Strukcha
==

bit = 1

byte = 8


##Da Mems


memz = 512 byte

sak = 32 byte

bag = 64 byte

gab = 64 byte

tnk = 96 byte

drp = 64 byte

uza = 256 byte

(memz - sak - bag - tnk - uza - drp) / 8 => 0

##Da Spleen

splenz = 512 byte

corkeban = 78 byte

name = 32 bit

rank = 16 bit

serial numba = 32 bit 

wagi = 1 byte

confusion = 1 byte

anger = 1 byte

plex = 1 byte

eer = 1 byte

tung = 1 byte

pakstart = 2 byte

pakend = 2 byte

corestart = 2 byte

drpset = 64 byte

pad = 128 byte

user = 128 byte

status = confusion 
         + anger
         + wagi
         + plex 
         + tung 
         + eer
         + pakstart 
         + pakend

drop = 2 byte

roll = 2 byte

boot = drop + roll

( splenz  
  - corkeban 
  - name  
  - rank  
  - serial numba 
  - status  
  - drpset
  - boot
  - pad
  - user ) / 8 => 90

###Da Pak





