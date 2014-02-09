
: .#teststr s" this is a rather long test string. there are no newlines in it. it should fill a small buffer completely to capacity. I'm adding a great deal of extra test to this of the lorem ipsum etce because running out of test string is annoying at best." ;
: .#tnl s\" this \nstring \nhas \nnewlines." ;
: .#longnl s\" this \e[34mstring\e[0m has newlines. \nthey are sufficiently spaced for my nefarious purposes. \nthat should do." ;
: .#rstr s\" prefix: \e[34mthis \e[31mstring is red\e[0m" ;
: .#hex s\" 0123\e[31m4567\e[0m\e[2m89AB\e[0mCDEF" ;
: .#grec s\" ψΓωͲῶ≤+∢" ;
: .#\e  s\" \e[34mfoo" ;
: .#\e\n s\" \e[31m\nbaz" ;
: .#emo s\" 😇 😏 😑 👲 " ;
: .#emj s\" 😇😏😑👲" ;
: .#zh s\" 道德經" ;
: .#\nlead s\" \nlead" ;
: .#\n2    s\" \n2"   ;

2 5 ' + 1 test \ false

2 3 ' noop 2 3 2test \ true
2 3 ' noop 2 4 2test \ false

 .#emo    ' 1-pr 4 1  2test
 .#tnl    ' 1-pr 1 1  2test
 .#\e\n   ' 1-pr 5 0  2test
 .#\e     ' 1-pr 5 0  2test
  .#\n2    ' 1-pr 0 -1 2test
 null-str $@ ' 1-pr 0 {eob} 2test

( 
.#emo ' 1-printable 4 test
.#grec ' 1-printable 2 test
.#\e ' 1-printable 6 test
.#tnl ' 1-printable 1 test
.#hex ' 1-printable 1 test
.#zh ' 1-printable 3 test
.#\nlead ' 1-printable 0 test
.#\e\n 1 ' printables 5 test 
\ .#\e\n 2 ' printables 5 test

.#\nlead 2 ' printables 0 test
.#\n2    2 ' printables 1 test
.#\e     3 ' printables 8 test
.#grec   4 ' printables 8 test
.#zh     3 ' printables 9 test
.#tnl   20 ' printables 5 test
 )

.#emo ' print-length 4 test
.#emo ' print-length -1 stack-test