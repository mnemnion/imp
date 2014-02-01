( # Subjects )

( These are conceptually simple. A subject is an address and an XT. 

	To return a subject, we call the XT on the address. The subject will return as a 
	start of memory and a count in bytes.

	So a subject that is a string will hold the xt `$@` and `perform` it on the str. 

	Why? Well, we modify subjects. We might want to replace it with 'offset-by-n', an XT
	that pulls down the address of the string, calculates an offset, and returns the remainder.

	Subjects are a single level of indirection over buffers, in effect. 

)