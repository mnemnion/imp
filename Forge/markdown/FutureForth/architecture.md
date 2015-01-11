#Ficus Project: Architecture

tl;dr LLVM is proposed as the implementation runtime for Ficus Forth.

##Rationale

Forth excels as a tool for low-level interactive systems programming. Ficus is intended to play to this strength.

The challenge is that a systems programming language aspires, by definition, to control of the hardware. This is in tension with the goal of writing a Unix-native Forth. One of the rationales for the project is the growing abundance of serious computers which are small enough and cheap enough to program umbilically, treating the lap or desktop as a fancy terminal. 

We take Unix as a given for serious development, despite the evidence that the embedded and microcontrol industry continues to favor Windows. The systems language of all Unices is of course C, with some object flavor for the GUI. 

We also want some flexibility, for instance, to run on both 32 and 64 bit, ARM and Intel systems. That's as exotic as we need to go without umbilicals and cross compiling. All of this points to using a metassembler model which is C compatible, or choosing C itself.

###The Case for C

Proper ANSI C has potential for the project. It's a popular choice, there are a number of existing codebases to draw from, and no one was ever fired for picking it. 

It's a strong candidate which I've by no means rejected. It makes deviating from the C paradigm difficult, and on a low level, things like register allocation strategies and use of the hardware stack differ substantially between C and Forth. 

##LLVM

LLVM was born as a backend for a C compiler, and still is. It is admired for the modularity of the toolchain: this is achieved by heavy use of C++, particularly the STL. There is a set of python bindings which is a couple points behind LLVM, which I propose to use when it catches up. 

LLVM is, in a way, an amusing match for Forth. It's register-only and single-assignment, in principle. Turns out that's what we want for optimization purposes, and this won't prevent the impression of a stack machine in the intermediate form. By which I mean, disassembled Ficus code will be LLVM IR, and it will meaningfully resemble a familiar stack machine in unoptimized form. 

Any way you slice it up, we'll be left with three languages: the implementation language for the runtime, the representation language for the machine (virtual or actual), and the actual Forth. Either Ficus, C, Assembly, or Ficus, C++/Python, LLVM. In the latter, we can do more with the actual LLVM and less with the C++. Ideally, we can write the compiler entirely in Python and port it to native C++ when it's reasonably stable. 

That may be the clinching vote, in fact. Writing a compiler is exacting work without having manage memory and security in the compiler itself while doing so. Operating LLVM is in effect a series of object calls, making Python well suited as a binding language. Merely having access to a primitive REPL helps immensely. 

