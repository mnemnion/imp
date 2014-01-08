#Unusual Words in Orcish

Not all Forthian words work the same in Orcish. It's more brutish and low-level, by design.

##:

Colon flips between compiling and interpreting states. So `: :` does nothing. You'd define a glyph like this: `: sq D * ; :`. That would: turn on the compiler, *attempt* to define a word as `sq`, D (dup) the word, compile an EXIT, and turn off the compiler. 

We say 'attempt' because the Orc might know `sq` already. In that case, you either get a gensym or try another glyph. 

This lets us compile multiple exits into a conditional, the way Chuck likes. But we don't like the Chuck habit of using non-ASCII for, well, anything. Orcs can't hear it if it's not ASCII: they're a cut-and-paste kind of brood. 

Unusually, Orcs will complain if you try and `:` them a word they can't find. They do the usual self-report if they're ready to compile: if they fail, they will try and use the next token, so it's business as usual except for the grumbling. Remember, Orcs don't change state in response to erroneous input. 

##\

We don't waste four characters on commenting.

`\` flips the ignorance bit. The values are written into the Gab. Another `\` flips the ignorance bit back, so again, `\ \` has no effect, since character words consume the following space. non-printable values are still ignored, for speed, brevity, and consistency. 

Orc is a protocol: Two Orcs might conversationally command each other and send data. The ignorance bit keeps data off the stack, and a core Orc will do precisely nothing with it, since core Orcs can only issue a few commands such as (`Or`)c! and (`WA`)GI!. core Orcs do repeat words back to you as numbaz, giving a standard and cheap way to verify that commands were received and understood. 

We can use this to send strings, provided they're printable and have no newlines. If that doesn't fit, best practice is to translate the string into a numba and send it in a comment. You can then tell the Orc to put the gab offset on the sak, and send the count, and do what you want. It's all tediously low-level, but that's Orc. 

Or, you can switch the port to an `eye` and `.` the info raw, a byte at a time. The eye handler writes into the gab without sanitizing input. 