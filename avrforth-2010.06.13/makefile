# user modifiable secion is up here
DEVICE=m2560
PROGRAMMER=arduino
PROGPORT=/dev/tty.usbmodem1421
TERMPORT=/dev/tty.usbmodem1421
#
#DEVICE=at90can128
#PROGRAMMER=avrispv2
#PROGPORT=/dev/ttyS0
#TERMPORT=/dev/ttyS1

# hopefully, you don't have to touch anything down here
VERSION=2010.06.13

SEND=./send.f $(TERMPORT)
AVRDUDE=avrdude -c $(PROGRAMMER) -p $(DEVICE)  -P $(PROGPORT)
GFORTH=gforth

avrforth: avrforth.f config.f asm/* drivers/*
	$(GFORTH) -e ": device s\" $(DEVICE)\" ;" -e ": version s\" $(VERSION)\" ;" avrforth.f -e "bye"

test: avrforth.f config.f asm/* drivers/*
	$(GFORTH) -e ": device s\" $(DEVICE)\" ;" -e ": version s\" $(VERSION)\" ;" avrforth.f

upload: flash eeprom
	$(AVRDUDE) -e -U flash:w:flash:r -U eeprom:w:eeprom:r

verify: flash eeprom
	$(AVRDUDE) -U flash:v:flash:r -U eeprom:v:flash:r

download:
	$(AVRDUDE) -U flash:r:/tmp/flash:r -U eeprom:r:/tmp/eeprom:r

terminal:
	$(AVRDUDE) -t -u

reset:
	$(AVRDUDE)

serial:
	picocom -l -b 38400 $(TERMPORT)

highlevel: blocks/core.f blocks/assembler.f blocks/bit.f blocks/extend.f \
           blocks/flag.f blocks/lerp.f blocks/debug.f
	for block_ in $^; do $(SEND) $$block_ ; done

clean:
	rm -f flash eeprom
  
dist:
	git archive --format=tar --prefix=avrforth-$(VERSION)/ \
	$(VERSION) | gzip > /tmp/avrforth-$(VERSION).tar.gz

