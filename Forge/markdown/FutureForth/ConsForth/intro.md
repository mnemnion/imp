#ConsForth

The Forth community badly needs a unifying runtime environment.

I am convinced that Forth is a program, or if you must, a metaprogram. It's not a language, though there are useful dialect continuums and ANSI should be viewed as an acrolect of the Forth operating dialects. I have no beef with the ANS dialect, other than a subtle one: if we continue to confuse it with the Forth runtime, Forth will most likely continue to languish in open-source circles. Which are, increasingly, the only ones that matter.

###About License 

I am firmly convinced that the correct license for a Forth environment is MIT or BSD. Any programming system designed to be interactive at runtime should be so licensed; the freedom gained is worth the potential to lose code. I don't want to get philosophical here, but I will say that I do consider it a virtue to allow people to choose to behave with good will. Pragmatically, many open source languages have permissive licenses and appear to flourish under them.

##Rationale

ConsForth is short for Console Forth and Consular Forth.

The intention is to provide a runtime environment that allows development of single codebases which incorproate multiple dialects and can be interactively tested on multiple machines, virtual, actual, and remote. That's ambitious, but it's essentially a hosted Forth with a decent set of cross-compiling and umbilical tools built in. 

Consular, because it relies heavily on Unix tools being available. Python comes with batteries included; why not fire it up and talk to it, when it's doing the best thing? Getting a scripting language to return a concatenated syntax is refreshingly simple. 

Forge is a bell and whistle factory. If you build them in ANS Forth, there are serious problems with incorporating them into other codebases. 

We intend to solve this by generalizing slightly on what a Forth runtime looks like.

###Architecture

In ConsForth, a single environment is called a shelf. At one end of the shelf is the journal, at the other end, the core. In between are books. 

A shelf is conceptually a linked list with shared structure, and this is mostly likely the best way to implement them also. A book, from its own perspective, is a journal, which may require words from other books, with one twist.

That twist is the core: this is either (one of) the native runtime environments, a virtual machine, or a remote host. There must be a clean compilation path from the journal to the core. Books do not specify the core they are targeting: their shelf defaults to the journal's core.

There may be a 'warm' core and a 'cold' core, actually, but no more than this. If so, the warm core is called Root and the cold core is called Core. 

An example of this in practice: Let's say we're working with two cores, one is the ConsCore, the other a partially-developed core running on a remote FPGA called Nelson. A journal may target either ConsCore or Nelson: in either case, any books loaded must be able to run on the chosen environment. 

A nice feature of Forth is, one may provide a very small core and build compatibility layers from there. 

Cores (I think it's cores) come with interpreters, which include the compilers, and executors. What we normally call the 'inner interpreter' in Forth is an executor; I think that term is sufficiently general. An interpreter is the 'outer interpreter' and is a deferred word: each core's interpreter must provide some minimal set of compiling and interpreting words, and defer the user level words such as `interpret` and `:`. 

Interpreters are the most important class of parsing word, but not the only one. We want to provide parsers as first-class citizens in ConsForth. The trick to this is making sure they have absolutely predictable behavior in all states. The 'macro' system in ANS Forth is quite powerful, but unlocking it requires non-standard behavior to be disguised behind workaround words. One of the most important ConsForth efforts is to make this absolutely standard. 

I am of the firm opinion that a Forth does not provide types. That said, I am working on Fabri, an annotation interpreter that runs alongside Forth, is written in Forth, and provides some access conventions to carry over typechecking into a production environment. 

What Forth does provide is first-class abstractions. An execution token is just an address, but it has predictable behavior in some contexts. Subtracting 3 cells from it is an example of an unpredictable effect. 

In order to support high-level interactivity and introspection, the ConsForth dialect needs to be able to provide consistent access to the components of a Forth runtime environment. Thus, a word like "find" has to take on the nature of an ironclad guarantee: we must be able to take a word and introspect on all behaviors it can present. 

##Interpreter

In order to be an interpreter a parsing word **must**:

 - Consider at least newline, tab, and space to be token boundaries. Space may have no additional meaning. 
 - Use stacks in a Forth consistent fashion, requiring at least two stacks.
 - Provide at least compilation semantics, which provide an execution token that the core's executor my execute.
 - Provide a form that acts on a counted string, a line form, and a word-consuming form.

An interpreter must provide `interpret`, which runs the interpreter on a counted string, and `:`, which compiles a definition. Both must be deferred, along with some support words such as `'`, so that a journal may, for example, target them at an umbilical system. In which case, `interpret` may turn the text into host XTs and pipe them, while `'` will compile the execution token for the host system, and `:` will make a dictionary entry containing only the instruction to send that execution token to the host system. 

Interpreters are allowed to parse within tokens, and may encounter parsing words which change their behavior in arbitrary ways. The former is uncommon in Forth, the latter is allowed but viewed askance for good reason. We intend to regulate this behavior in some fashion, though it is admittedly a hard problem. 

Interpreters are a type of parser: these are two abstractions that consume an input stream and produce an effect. It is not a strict subclassing because only an interpreter may call a parser, and a parser must provide predictable behavior for both interpreter states. In turn, an interpreter must provide predictable behavior for the states it provides. Usually this is interpretation and compilation, but not always, viz. ColorForth and friends.

Whitespace delimitation is crucial and non-optional. When we cross this boundary, which we will, we are officially interpreting another language into a Forth runtime environment. Forths use words; words use the spacemark. 

Other restrictions will apply, but we'll cross those bridges when we encounter them. One of my personal rules is "no magic": `+` can't do one thing to a 'number' and another thing to a 'string', because a string is just a number (we use cell-sized byte-counted strings) that points to some data, and that will not change in ConsForth or dialects. The resulting clarity is crucial to the runtime environment. 

##Dictionaries
 
A dictionary in ConsForth is a statically allocated key-value hash. This is even more limited than in Forth. To simulate marking and forgetting, an entirely new dictionary is created and added to the local book's internal search order. 

The ConsForth reference core is envisioned as indirect-threaded, albeit supported by strong primitives. The words themselves will be headlessly allocated into the dictionary pile, with the hashes producing appropriate pointers. 

A single book my provide many dictionaries. We'll use a very limited amount of magic in Fabri to build annotations: the sequence `\ (` or `( \ ` will trigger, to the corresponding `)`, a separate compiling stage into an annotation dictionary. 

Similarly, as `:` is deferred, it may automatically target as many cores as is useful. Say you're testing a parser combinator library against ConsForth, 32-bit ANS Forth, and Ngaro/Retro. The journal, which is running on the ConsForth core, may redefine : to produce ANS and Ngaro dictionaries and recompile its dependencies, then try to boot itself on one of the test cores, and/or run it as an umbilicus across a socket. 

##The Load Process

A front-level program is a journal. On a hosted system such as Consular Forth, this is your application. On the bare-metal systems we intend to design, we will require a coherent way to maintain multiple shelves. For a few of our purposes, two full shelves would be handy.

The whole architecture is moderately complex, especially for Forth. I'll justify it a few steps at a time.

I have no beef with the global-hyperstatic nature of Forth. It's a great strength. The 'book' concept is a way to make this modular without introducing explicitly private words etc.

When a journal loads, it has a core, and a shelf of books that it searches in a particular order. It may jump over blocks using `.:`, so that `word.:book` will retrieve `word` from the book in question. This does not nest: `word.:book.:anotherbook` will look for the word `book.:anotherbook` in `book`, which by the design of ConsForth is one of the only kinds of words you cannot define. Perhaps the only kind: We allow parsing in words, but consider it ever-so-slightly unForthlike. `word .: book ` or even ` book .: word` on the premise that `.:` is consuming the word and applying it against the book, is even more Forthlike, but I would itch to elide it in real systems programming. 

Each book has its own shelf, to which the words may make reference. Any book may `include` another book, literally reading it into the dictionary, but typically we merely `require` a book.

Because (and only because!) we are in Embassytown, where everything is a file, books are files. Files do not have to be books, they may be fragmentary and `load`ed into books.

A journal sees the books it `requires`, but not the contents of those `required` in turn. This applies to all books; a word must be on the seach path of the book being loaded, which always points to a core. I believe this promotes the correct kind of 'private' word: a library or similar can expose its manipulation words in the front, and the rest of the files in the library may be searched explicitly
if plumbing words are needed for any reason. A book may also add only a few words from another book to its own search path, if desired. 

This being forth, a book may arrange the shelf however it chooses during load time. The final state only matters in weird circumstances like changing out the execution token of a particular word the compilation of which is postponed, but is preserved as a link list to all the dictionaries. Only the 'book' dictionary itself is exposed, containing a) new entries and b) entries brought into the book. 

###Why

In ConsForth, the concepts of 'vocabulary search order' and "Forth operating environment" are first-class, and may be manipulated and introspected upon accordingly. This ability is crucial for building large, interconnected systems. 

As is, in my opinion, some concept of type. I am implementing this as a skeletal overstructure on Forth, and it should remain so. The left side of the page is always for action. I intend only to formalize and render coherent the practice of annotating code as to effect, from the programmers perspective. Our type system is all assertion, warning, interaction; there is no bondage and discipline, no confusing inability to compile, with the resulting inability to interact wtih the source of error. 

This combination should prove powerful. A book loads with strings documenting the words, annotations as to effect, and a comprehensive memory structure indicating where words come from and what they do. We embrace the tendency of Forth to evolve into a domain-specific dialect, by encouraging solid documentation of that dialect, and by making it so our introspection tools don't have to sweat hard to figure out what `foo` refers to in this particular context. 

A word like `see` can link to the actual referents of each word in the decompilation, or use a verbose mode to show the book of origin of each word. Consequently, a book must have a unique name within the space; this is one of the few places where ConsForth adds a new source of error, but a book may be `required` under a pseudonym to avoid this. 

`see` has to be a fairly smart word as a result: starting in the journal, navigating down to the book in question, then disassembling the code according to the definitions provided by the local bookshelf.

Name collisions can happen only within one bookshelf, and each book maintains its own shelf. 

One grand result is that ANS Forth code and "BForth" code can coexist peacefully. An ANS library will have a different sense of what's right and proper, but it won't word-pollute a BForth library, and vice versa. They'll run on the same core. 

Any language like Forth accumulates dozens of different uses for the same small words. As indeed it should. ConsForth is built to explain the resulting confusion when it arises. "This `list` works on a counted array and prints to the screen, and comes from the `print` book" is useful information to have. 

##Dependency Management and a Package System

ConForth is not intended as a large, batteries-included Programming Environment. We have plenty of those, and a large 'standard library' is not particularly Forthright. My intention is to make sockets and shell calls work cleanly, then if I need something Python has, I will call Python. 

The ability to sensibly deal with name environments will enable Forth programmers to use arbitrarily large pieces of one another's code. An annotative, inferential typing system can only help this process. Inevitably, this leads to dependence on a particular version of a written codebase.

Nor does the usual 'string containing a hopefully monotonic version number' approach, or its numeric cousin, offer remedy. It is quite possible to imagine a codebase at the same 'version' that targets different cores. While it should be possible to use `[if]` and cousins to combine these in a single book, it shouldn't be necessary to do so. 

The simplest way is to use a hash function to generate an ID for any given codebase. Tracking the history of this may get arbitrarily complex but isn't necessary: by referring to a book by ISBN you may be assure that you will either receive the expected book or have a failure to retrieve. Books themselves should include a couple of docstrings: I encourage every word in ConsForth to have a docstring as well. 