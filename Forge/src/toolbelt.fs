\ ANEW --TOOLBELT--                           \  Wil Baden  2003-02-25

\  *******************************************************************
\  *                                                                 *
\  *  Wil Baden  2003-02-25                                          *
\  *                                                                 *
\  *                         ToolBelt 2002                           *
\  *                                                                 *
\  *  These are common tools used in several source files.  Many     *
\  *  have been around for awhile, invented and implemented          *
\  *  independently.  They are given here so you can avoid           *
\  *  duplicate definitions.  Comment out definitions that you       *
\  *  already have or are enhancing. Many of them should be CODE     *
\  *  definitions.                                                   *
\  *                                                                 *
\  *******************************************************************

\  In any Forth system, the definitions should be optimized when
\  possible.  The definitions here are in Standard Forth for
\  portability.

\  I hope that readers will submit environmental optimizations.

\  mailto:neilbawd@earthlink.net

\  Please identify the environment for your modifications.

\  Definitions in Standard Forth by Wil Baden. Any similarity with
\  anyone else's code is coincidental, historical, or inevitable.

\  ***********************  In File TOOL2002  ************************

\  General tools for Personal and Sharable code.

\     #Chars/Line    APPEND         H#             POSSIBLY
\     #EOL-CHAR      BOUNDS         Is-White       R'@
\     'th            BUFFER:        Memory-Check   Rewind-File
\     (.)            C#             Next-Word      STRING,
\     ,"             C+!            NOT            TEMP
\     2NIP           COUNTER        OFF            THIRD
\     3DROP          EMPTY          OFFSET:        TIMER
\     3dup           EXPIRED        ON             VOCABULARY
\     ??             File-Check     ORIF           [DEFINED]
\     ANDIF          FLAG           OUT            [UNDEFINED]
\     ANEW           FOURTH         PLACE          \\

\  ***********************  In File CHARCASE  ************************

\  Character/string comparison and conversion.  Latin-1 Characters.

\     .LOWER         Char>Upper     Is-Alpha       Is-Upper
\     .UPPER         COMPARE(NC)    Is-Digit       String->Lower
\     Char>Lower     Is-Alnum       Is-Lower       String->Upper

\  ***********************  In File CHARSCAN  ************************

\  Scanning Character strings.

\     BACK           EQUALS?        SCAN[          STRING/
\     BACK[          HUNT           SIMILAR?       th-Word
\     CHOP           JOIN           SKIP           th-Word-Back
\     CONTAINS?      Last-Word      SPLIT          th-Word-Forward
\     END-C@         Replace-Char   SPLIT[         TRIM
\     ENDS?          SCAN           STARTS?

\  ************************  In File COMMON  *************************

\  Common functions. Defined when used.

\     !++            /PAD           C!++           HIWORD
\     #BACKSPACE-CHAR               C@++           LOWORD
\     #BITS/CELL     @++            EMITS          MAX-N
\     #TAB-CHAR      ADDRESS-UNIT-BITS             SIGN-BIT
\     /COUNTED-STRING               ENUM           [VOID]

\  Deprecated:

\     ++             -CELL          CELL           CELL-
\     !+             Append-Char    LEXEME
\     /SPLIT         BL-Scan        Split-Next-Line
\     @+             BL-Skip        View-Next-Line

\  *******************************************************************
\  *         Forth Programmer's Handbook, Conklin and Rather         *
\  *******************************************************************

\  ANEW                            ( "name" -- )( Run: -- )
\     Compiler directive used in the form: `ANEW _name_`. If the word
\     _name_ already exists, it and all subsequent words are
\     forgotten from the current dictionary, then a `MARKER` word
\     _name_ is created. This is usually placed at the start of a
\     file. When the code is reloaded, any prior version is
\     automatically pruned from the dictionary.
\     Executing _name_ will also cause it to be forgotten, since
\     it is a `MARKER` word.
\     Useful implementation factor of `EMPTY`.

\  C+!                          ( n addr -- )
\     Add the low-order byte of _n_ to the byte at _addr_, removing
\     both from the stack.

\  EMPTY                        ( -- )
\     Reset the dictionary to a predefined golden state, discarding
\     all definitions and releasing all allocated data space beyond
\     that state.
\     This `EMPTY` uses `--EMPTY--` to separate kernel words and
\     user words.  Rename `--EMPTY--` if you wish.
\     `MARKER --EMPTY--` will setup a new golden area for `EMPTY`.
\     `--EMPTY--` will restore the previous golden area.

\  NOT                          ( x -- flag )
\     Identical to `0=`, used for program clarity to reverse the
\     result of a previous test.  It should used only when `0=` as
\     well as `INVERT` could take its place.

\  POSSIBLY                        ( "name" -- )
\     Execute _name_ if it exists; otherwise, do nothing. Useful
\     implementation factor of `ANEW`.

\  VOCABULARY                   ( "name" -- )
\     Create a word list _name_. Subsequent execution of _name_
\     replaces the first word list in the search order with _name_.
\     When _name_ is made the compilation word list, new definitions
\     will be added to _name_'s list.

\  [DEFINED]                    ( "name" -- flag )
\     Search the dictionary for _name_. If _name_ is found, return
\     TRUE; otherwise return FALSE. Immediate for use in definitions.

\  [UNDEFINED]                  ( "name" -- flag )
\     Search the dictionary for _name_. If _name_ is found, return
\     FALSE; otherwise return TRUE. Immediate for use in definitions.

\  ******************  Forth Programmer's Handbook  ******************

: NOT  ( x -- flag )  S" 0= " EVALUATE ; IMMEDIATE

: [DEFINED]                 ( "name" -- flag )
    BL WORD FIND NIP 0<> ; IMMEDIATE

: [UNDEFINED]               ( "name" -- flag )
    BL WORD FIND NIP 0= ; IMMEDIATE

: C+!  ( n addr -- )  dup >R  C@ +  R> C! ;

: POSSIBLY  ( "name" -- )  BL WORD FIND  ?dup AND IF  EXECUTE  THEN ;

: ANEW  ( "name" -- )( Run: -- )  >IN @ POSSIBLY  >IN ! MARKER ;

: EMPTY                         ( "name" -- )
    S" ANEW --EMPTY-- DECIMAL  ONLY FORTH DEFINITIONS "
    EVALUATE ;

\ : Do-Vocabulary               ( -- )

\     \  From Standard Forth Rationale A.16.6.2.0715.
\     DOES>  @ >R               ( )( R: widnew)
\         GET-ORDER  SWAP DROP  ( wid_n ... wid_2 n)
\     R> SWAP SET-ORDER ;

\ : VOCABULARY                  ( "name" -- )
\     WORDLIST CREATE ,  Do-Vocabulary ;

\  *******************************************************************
\  *                Environmentally Dependent Values                 *
\  *******************************************************************

\  #Chars/Line                   ( -- n )
\     Preferred width of line in text files.  User defined and
\     changeable.  "number of characters for each line"

\  #EOL-CHAR                     ( -- char )
\     The end-of-line character. 13 for Mac and DOS, 10 for Unix.
\     "the end-of-line character"

70 VALUE  #Chars/Line

13 CONSTANT  #EOL-CHAR

\  *******************************************************************
\  *                           Common Use                            *
\  *******************************************************************

\  (.)                             ( n -- str len )
\     Convert number to string.

\  The traditional definition for  (.)  is:

\ : (.)                            ( n -- str len )
\     dup ABS 0 <#  #S  ROT SIGN  #> ;

\  But we like to see TRUE and SIGN-BIT in hex as FFFFFFFF and
\  80000000, and also see bit masks as unsigned.  It would be nice to 
\  let the program do it.

\ : (.)                            ( n -- str len )
\     BASE @ 10 = IF  dup ABS  ELSE  0 SWAP  THEN
\     0 <#  #S  ROT SIGN  #> ;

\  Even nicer would be the following.  This helps in distinguishing
\  similar binary numbers.  Like 0FFFFFFF from FFFFFFFF and 08000000
\  from 80000000.

: (.)                             ( n -- str len )
    CASE BASE @
    10 OF  dup ABS 0 <# #S ROT SIGN #>                       ENDOF
    16 OF          0 <#  BEGIN  # #  2dup OR 0= UNTIL #>     ENDOF
     2 OF          0 <#  BEGIN  # # # #  2dup OR 0= UNTIL #> ENDOF
                   0 <#  #S #>
     0 ENDCASE ;

\  Of course we want with whichever one...

: .  ( n -- )  (.) TYPE SPACE ;

\  And we want it with  .S  as well.

: .S                              ( ... -- same )
    DEPTH 0< ABORT" Stack Underflow "
    DEPTH BEGIN  dup WHILE
        dup PICK S" . " EVALUATE
        1-
    REPEAT DROP ;

\  ,"                           ( "<ccc><quote>" -- )
\     Store a quote-delimited string in data space as a counted string.
\     See file QUOTSTR

\  APPEND                       ( str len addr -- )
\     Catenate the string at _str_, whose length is _len_, to the
\     counted string already existing at _addr_.  Does not check
\     whether space is allocated for the final string.  AKA `+PLACE`.

\  BOUNDS                       ( str len -- str+len str )
\     Convert _str len_ to range for DO-loop.

\  C#                           ( -- addr )
\     Variable for character count.  Should be updated by `CR`, `EMIT`,
\     `TYPE`, etc.

\  Is-White                     ( char -- flag )
\     Test _char_ for white space.  Any character with value
\     less than 33 is taken as white space.

\  Next-Word                    ( -- str len )
\     Get the next word in the input stream as a character string -
\     extending the search across line breaks as necessary, until the
\     end-of-file is reached - and return its address and length.
\     Returns a string length of 0 at the end of the file.

\  OFF                          ( addr  -- )
\     Set the flag at _addr_ to true.  Already defined in many
\     implementations.

\  ON                           ( addr -- )
\     Set the flag at _addr_ to false.  Already defined in many
\     implementations.

\  PLACE                        ( str len addr -- )
\     Copy the string at _str_, whose length is _len_, to _addr_,
\     formatting it as a counted string, i.e., the length is in the
\     first byte.  Does not check whether space is allocated for the
\     final string.

\  STRING,                      ( str len -- )
\     Store a string in data space as a counted string.

\  **************************  Common Use  ***************************

: APPEND                    ( str len addr -- )
    2dup 2>R  COUNT chars +  SWAP chars MOVE ( ) 2R> C+! ;

: BOUNDS  ( str len -- str+len str )  over + SWAP ;

VARIABLE  C#  \  Should be USER variable.

: Is-White                        ( char -- flag )
    33 - 0< ;

\  Note.  Michael Gassanenko observes that the cited 
\  definition will not work on...
\    a) still standard systems with signed chars
\    b) unstandard but not less numerous 8-bit systems
\  In such a case -
\  : Is-White ( char -- flag ) 33 U< ;

: Next-Word                 ( -- str len )
    BEGIN  BL WORD COUNT            ( str len)
        dup IF EXIT THEN
        REFILL
    WHILE  2DROP ( ) REPEAT ;       ( str len)

: OFF  ( addr -- )  FALSE SWAP ! ;

: ON  ( addr -- )  TRUE SWAP ! ;

: PLACE                     ( str len addr -- )
    2dup 2>R  char+  SWAP chars MOVE  2R> C! ;

: STRING,                   ( str len -- )
    HERE  over 1+ chars ALLOT  PLACE ;

: ," [char] " PARSE  STRING, ; IMMEDIATE

\  *******************************************************************
\  *                         Stack Handling.                         *
\  *******************************************************************

\  2NIP                         ( w x y z -- y z )
\     Drop the third and fourth elements from the stack.

\  3DROP                        ( x y z -- )
\     Drop the top three elements from the stack.

\  3DUP                         ( x y z -- x y z x y z )
\     Copy top three elements on the stack onto top of stack.

\  FOURTH                       ( w x y z -- w x y z w )
\     Copy fourth element on the stack onto top of stack.

\  R'@                          ( -- x )( R: x y -- x y )
\     The second element on the return stack.

\  THIRD                        ( x y z -- x y z x )
\     Copy third element on the stack onto top of stack.

\  ************************  Stack Handling.  ************************

: THIRD  ( x y z -- x y z x )      2 PICK ;    \  Should be CODE defn.

: FOURTH ( w x y z -- w x y z w )  3 PICK ;    \  Should be CODE defn.

: 3dup   ( x y z -- x y z x y z )  THIRD THIRD THIRD ;

: 3DROP  ( x y z -- )            2DROP DROP ;  \  Should be CODE defn.

: 2NIP   ( w x y z -- y z )      2SWAP 2DROP ; \  Should be CODE defn.

: R'@    S" 2R@ DROP " EVALUATE ; IMMEDIATE    \  Should be CODE defn.

\  *******************************************************************
\  *                    Short-Circuit Conditional                    *
\  *******************************************************************

\  ANDIF                           ( p "... THEN" -- flag )
\     Given `p ANDIF q THEN`:
\     If _p_ is 0 then test _q_ will not be performed and the result
\     will be _p_, i.e. 0;
\     If _p_ is not 0 then the result will be _q_.

\  ORIF                            ( p "... THEN" -- flag )
\     Given `p ORIF q THEN`: If _p_ is 0 then the result will be _q_; 
\     If _p_ is not 0 then test _q_ will not be performed and the 
\     result will be _p_. 

: ANDIF   S" DUP IF DROP " EVALUATE ; IMMEDIATE

: ORIF   S" DUP 0= IF DROP " EVALUATE ; IMMEDIATE
\     or
\ : ORIF   S" ?DUP 0= IF " EVALUATE ; IMMEDIATE

\  These could be defined with `POSTPONE`.

\  : ANDIF   postpone DUP  postpone IF  postpone DROP ; IMMNEDIATE
\  : ORIF postpone DUP  postpone 0=  postpone IF  postpone DROP ; IMMNEDIATE

\  *******************************************************************
\  *           Promiscuous Variables Available in Any Task           *
\  *******************************************************************

\  TEMP                         ( -- addr )
\     Promiscuous variable available in any task.

\  OUT                          ( -- addr )
\     Promiscuous variable available in any task.

\  FLAG                         ( -- addr )
\     Promiscuous variable available in any task.

VARIABLE  TEMP   \  Should be USER variable.

VARIABLE  OUT    \  Should be USER variable.

VARIABLE  FLAG   \  Should be USER variable.

\  *******************************************************************
\  *                         Error Checking                          *
\  *******************************************************************

\  File-Check                   ( n -- )
\     Check for file access error.

\  Memory-Check                 ( n -- )
\     Check for memory allocation error.

\  ********  These words should be tailored for your system.  ********

: File-Check      ( n -- )  ABORT" File Access Error " ;
: Memory-Check    ( n -- )  ABORT" Memory Allocation Error " ;

\ : File-Check    ( n -- )  THROW ;
\ : Memory-Check  ( n -- )  THROW ;

\ : File-Check    ( n -- )  SHOWERROR ;  \  PMF
\ : Memory-Check  ( n -- )  SHOWERROR ;  \  PMF

\  *******************************************************************
\  *                         Interval Timing                         *
\  *******************************************************************

\  COUNTER                      ( -- ms )
\     Return the current value of the millisecond timer.

\  TIMER                        ( u -- )
\     Repeat COUNTER, then subtract the two values and display the
\     interval between the two in milliseconds.

\  EXPIRED                      ( u -- flag )
\     Return true if the current millisecond timer reading has passed
\     _u_.

[DEFINED] _TickCount [IF]  \  Environment Dependent

    60 CONSTANT  Ticks-per-Second

: COUNTER  ( -- u )  _TickCount  1000 Ticks-per-Second */ ;

: TIMER                     ( u -- )
    COUNTER SWAP -
        0 <#  # # # [char] . HOLD  #S  #> TYPE SPACE
    ;

: EXPIRED ( u -- flag ) COUNTER - 0< ;

[THEN]

\  *******************************************************************
\  *                          Miscellaneous                          *
\  *******************************************************************

\  BUFFER:                      ( n "name" -- )
\     Create buffer _name_ of length _n_ in data space.

: BUFFER:                   ( n "name" -- )
    CREATE  ALLOT ;

\  H#                           ( "hexnumber" -- n )
\     Get the next word in the input stream as an unsigned hex
\     single-number literal. (Adopted from Open Firmware.)
\
\     "The best way to manage BASE is to establish a global default,
\     e.g. DECIMAL. If you change to another, e.g. HEX, you assume
\     responsibility for changing back when you're done with it. The
\     scope of a changed region can be part of a file or all of a
\     file, but shouldn't span files. Always return to your default
\     at the end of a file _if_ you've changed it. This minimizes
\     fussy saving & restoring, and you always know where you are."
\     -- Elizabeth D. Rather

: H#  ( "hexnumber" -- u )  \  Simplified for easy porting.
    0 0 BL WORD COUNT               ( 0 0 str len)
    BASE @ >R  HEX  >NUMBER  R> BASE !
        ABORT" Not Hex " 2DROP      ( u)
    STATE @ IF  postpone LITERAL  THEN
    ; IMMEDIATE

\  'th                          ( n "addr" -- &addr[n] )
\     The address `_n_ CELLS _addr_ +`.

: 'th                       ( n "addr" -- &addr[n] )
    S" CELLS "    EVALUATE
    BL WORD COUNT EVALUATE
    S" + "        EVALUATE
    ; IMMEDIATE

\  OFFSET:               ( n "name" -- )( Run: addr -- addr+n )
\     Field-defining word.  Create a word which adds a constant to
\     the number (usually an address) on top of the stack.  Used  in
\     the form  _n OFFSET: <fieldname>_  to create a field definition
\     _<fieldname>_.  AKA `FIELD`.  (`FIELD` has been given other
\     conflicting definitions, so `OFFSET:` hopes to avoid conflict.)

: OFFSET:             ( n "name" -- )( Run: addr -- addr+n )
    S" : " PAD PLACE
    BL WORD COUNT  PAD APPEND
    S"  " PAD APPEND
    dup 0= IF DROP
        S" ; IMMEDIATE " PAD APPEND
    ELSE
        (.) PAD APPEND
        S"  + ; " PAD APPEND
    THEN
    PAD COUNT EVALUATE ;

\  REWIND-FILE                  ( file-id -- ior )
\     Reposition the file at its beginning.

: Rewind-File               ( file-id -- ior )
    0 0 ROT REPOSITION-FILE ;

\  ******************************  \\  *******************************

\  \\                           ( "...<eof>" -- )
\     During an INCLUDE operation, treat anything following this word
\     as a comment; i.e., nothing after `\\` is interpreted in a
\     source file will be interpreted or compiled.

: \\                          ( "...<eof>" -- )
   BEGIN  -1 PARSE  2DROP  REFILL 0= UNTIL ;

\\   //   \\   //   \\   //   \\   //   \\   //   \\   //   \\   //   \\  

\  ******************  Tests and Notes may Follow  *******************

