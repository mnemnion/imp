#Type Annotations

What are we actually going to do with type annotations? Lots of things. On a program level, we're going to want a separate data structure to keep track of our annotations, and an optional slow loop that bothers to keep it updated. The fast loop is just Forth. 

Something I'm intending to formalize with Forge is the tendency to stuff a bunch of printouts and watches into code while you're writing it. Specifically, I want to fight the later tendency to scrub those words out and wish you had them later. Forge can tuck code out of the way and retrieve it later; Fabri is going to be somewhat line-based, simply because our annotations terminate with a newline and reference the physical line of code to the left. 

A simple word like `s"` has the eventual effect of putting a string on the stack. When it happens, when we're keeping track, the stack looks like `1293458720 23` for a 23 byte string. The annotation stack has a `counted-string` on TOS and a mandatory ASCII space, meaning
no-op, at POS. A counted string takes up two stack slots but is one type, this is how we reconcile the difference. 

This is why the first big project is figuring out a semi-sensible way to represent types in memory and building a type directory of the ANS core words. We want to do very simple, ordinary inference: `* dup dup cells + @` reads as `-1 +1 +1 +1 -1 -1` for stack results, requires two numbers on the stack, and should induce a warning if we have reason to believe that @ is not operating on an address.

Note that address arithmetic is perfectly ordinary, and one of the fun things about types is we can check if the resulting address is in-bounds and has appropriately typed contents. If we want to. 

This means we shouldn't have to write very many annotations; Forge can provide them for us, and prompt new ones when novel ground is reached. 

The simplest type directory pushes a type number onto the type stack every time a value is pushed, and when anything is stored or allocated, puts that address into a linked list directory containing the annotations and a link to the associated code. 

This is another example of why we need first-class dictionaries and a smooth way of moving between them. Simple words like `!` and `@` are going to have multiple meanings, depending on whether we're tracing annotations, compiling, interpreting, optimizing, generating documentation, editing code, typing an email address. 

Fortunately the Forth parser is deliberately really, really dumb. So we can in effect rewrite it, and do two passes on code. Heck, we can even use the ordinary stacks for the annotation pass, as long as we clean up before we hand the parser back over. That's for compiling, which is only half the story. A tracing interpreter is moderately more complex. 

 considerable overhead in memory and execution time. It's worth it, Forth is ridiculously small and fast, and better yet, we don't need any of the discipline when the code is functioning up to par. I'm trying to program systems that have 512 *bytes* of RAM, 8k of Flash, and a k of EEPROM. Getting down that small requires some real craftsmanship: Any extra information we can provide will come in handy. There is no substitute for pure instruction on such tight systems, and less so for a prepacked radio OS that is possibly senile and likely to be hostile. 

The annotations are almost like a unit test. When you write words, the annotations you provide are your premise, and the words themselves have certain promises already baked in. The compiler can check if certain of these premises add up, and the interpreter can optionally keep track for you. Certainly if you suspect a problem it's worth slowing down and doing this from time to time. 

Of course, if Fabri doesn't like the way the annotations add up, and the code works anyway, something is wrong, something worth fixing. But nothing will prevent the code from executing in such circumstances. After all, it's the easiest way to deduce where the annotations are screwy, or that the code is in fact wrong. 



