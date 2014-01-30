#Annotation Strings

I've decided the internal representation form for the annotations will be... strings. Yep, strings; Fabri will be interpreted and parsed on the spot. 

You can always crush down an interpreted language; Fabri is not about speed at this stage of the game, it's about understanding what we're doing. The annotations strings will be readable, marked up version of the stack annotations; to make an annotation, subtract the uninteresting data. 

We will aim to make them forward only. Forward only is fast and easy to reason about. I like forward only.

## A transform Annotation

` : + \ ( cell.a cell.b -> := 'cell.a+'cell.b ) `

I believe that's sufficient. From left to right, we have the colon, `+` (which woudl be defined below), and the annotation. The first `\` makes it into a comment, the `(` makes it fabri. The annotation starts with the first `c` in `cell`.

We have no compound words here, hence no need to discuss them. `cell.a` and `cell.b` just means two cells that are not known to be the same. `:=` is our assertion. 

`'` is a maybe, but it means 'we are taking the name of the following word and doing something with it'. It reads until it's found a word, which must be one that it knows. It then concatenates until it hits a space or another ', in which case it begins again. This should probably be 'delimited' actually, like `'cell.a'+'cell.b'`. Fabri parses 'words' in a deliberately similar way to Forth, but sometimes parses within them as well. This lets us build new 'words' and even insert things like `.` that effectively function like commands within words.

The parser per se is envisioned as only extensible insofar as you could define a new parsing word, making a different dialect of Fabri. That is, tokens like `:=` are meant to be reserved words that can't be overloaded by defining a `:=` type. That's not especially Forthright, but type systems are about restriction, in essence. Forth is already Calvinball, it's better if our type system is somewhat less so.

This is a description of how to build the next annotation. So if we call `@ foo @ bar` and that leaves a stack that looks like ` a.type b.type --`, `+` will produce the annotation ` a+b.type -- `, since Fabri figures two types that pass through cell -> cell transforms or combinators still have the same type. If we then type `4 +`, the annotation is `a+b+4.xy`. Internally, this is stored type-forward. The outer interpreter can take more time than the annotation engine can afford. 

A text-based interpreter that runs during the compilation phase is all we need for many applications. As long as the whole Forth base is annotated, that might be all we need. It's all I'll build until I'm sure. 
