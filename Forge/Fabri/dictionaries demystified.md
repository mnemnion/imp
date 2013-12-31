#Dictionaries Demystified

On a concrete level, a dictionary is an instruction word that points to a region in memory, which contains a routine that takes a value as input and returns a value as output. Often, that value is a string. Often, that routine searches a linked list. 

In Forth, dictionaries are usually more specific than that, and often there's a "the" dictionary. This strikes me as premature optimization. In roomy environments it's common to want lots of dictionaries, and for the search object to be arbitrary. 