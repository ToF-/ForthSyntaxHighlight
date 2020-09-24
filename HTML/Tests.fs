TRUE CONSTANT TESTING
INCLUDE ffl/tst.fs
INCLUDE HTMLHighlight.fs

CREATE _OUTPUT 1024 ALLOT
CREATE _INPUT  1024 ALLOT
VARIABLE _OUTPUT#
VARIABLE _INPUT#
VARIABLE _#INPUT

\ emitting a char to the _output array
: EMIT>! ( c -- )
    _OUTPUT# @
    _OUTPUT + C!
    1 _OUTPUT# +! ;

\ typing a string to the _output array
: TYPE>! ( addr # -- )
    0 ?DO DUP I + C@ EMIT>! LOOP DROP ;

\ receiving a char from the _input array
: KEY<@ ( -- c )
    _INPUT# @ _#INPUT @ < IF
        _INPUT _INPUT# @ + C@
        1 _INPUT# +!
    ELSE
        4
    THEN ;

\ empty the _output array
: RESET-OUTPUT
    _OUTPUT 1024 ERASE
    _OUTPUT# OFF ;

\ put a specific content in _input array
: SET-INPUT ( addr # -- )
    _INPUT# OFF
    _#INPUT !
    _INPUT _#INPUT @ CMOVE ;

\ check equality between a string and the _output array
: ?OUTPUT ( addr # -- ? )
    _OUTPUT _OUTPUT# @ STR= ?TRUE ;

\ print the _output array for visual check
: .OUTPUT ( addr # -- )
    _OUTPUT _OUTPUT# @ TYPE ;

\ dump the _output array for debugging
: .DUMP
    _OUTPUT _OUTPUT# @ DUMP ;

\ these word are DEFERred in HTMLHighlight, 
\ so we can override  them with our test helpers
' EMIT>! IS _EMIT
' TYPE>! IS _TYPE
' KEY<@ IS _KEY

T{ ." <SPAN>. outputs a span tag with a color and a font weight" CR
    RESET-OUTPUT 4807 FALSE <SPAN>.
    S\" <span style=\"color:#0012C7; font-weight:normal;\">" ?OUTPUT
    RESET-OUTPUT 1024 TRUE <SPAN>.
    S\" <span style=\"color:#000400; font-weight:bold;\">" ?OUTPUT
}T
T{ ." </SPAN>. outputs a end span tag" CR
    RESET-OUTPUT </SPAN>.
    S\" </span>" ?OUTPUT
}T
T{ ." <PRE>. outputs a pre tag with a color and a background" CR
    RESET-OUTPUT 1024 4807 <PRE>.
    S\" <pre style=\"color:#000400;background:#0012C7;\">" ?OUTPUT
}T
T{ ." </PRE>. outputs a end pre tag" CR
    RESET-OUTPUT </PRE>.
    S\" </pre>" ?OUTPUT
}T
T{ ." SKIP-SPACE. prints input chars until non space found" CR
    RESET-OUTPUT
    S\"  \t\n\r  foo" SET-INPUT
    SKIP-SPACE.
    CHAR f ?S
    S\"  \t\n\r  " ?OUTPUT
}T
T{ ." SKIP-SPACE. returns EOF if end of input stream" CR
    RESET-OUTPUT
    S\"    " SET-INPUT
    SKIP-SPACE. 4 ?S
}T
T{ ." SKIP-STRING prints input chars until (and including) a double quote" CR
    RESET-OUTPUT
    S\" foo bar\" qux" SET-INPUT
    SKIP-STRING.
    S\" foo bar\"" ?OUTPUT
}T
T{ ." SKIP-LINE. prints input chars until end of line" CR
    S\" foo bar \n\r" SET-INPUT
    RESET-OUTPUT
    SKIP-LINE.
    S\" foo bar \n" ?OUTPUT
}T
T{ ." SKIP-LINE. prints input chars until end of input" CR
    S\" foo" SET-INPUT
    RESET-OUTPUT
    SKIP-LINE.
    S\" foo" ?OUTPUT
}T
T{ ." SKIP-COMMENT. prints input chars until (and including) right parens or end of input" CR
    S\" foo\n bar) qux" SET-INPUT
    RESET-OUTPUT
    SKIP-COMMENT.
    S\" foo\n bar)" ?OUTPUT
}T
T{ ." a LINKED-LIST starts with 0 as the first link " CR
    LINKED-LIST FOO
    FOO @ 0 ?S
}T
T{ ." ITEM, adds an item to a linked list in the dictionary" CR
    FOO S" Bar" 4807 ITEM,
    FOO @ ITEM>VALUE 4807 ?S
    FOO @ ITEM>NAME COUNT S" Bar" STR= ?TRUE
    FOO @ ITEM>NEXT 0 ?S
}T
T{ ." FIND-ITEM finds an item or returns 0" CR
    FOO S" Qux" 42 ITEM,
    FOO S" Bar" FIND-ITEM ITEM>VALUE 4807 ?S
    FOO S" Qux" FIND-ITEM ITEM>VALUE 42 ?S
    FOO S" Baz" FIND-ITEM 0 ?S
}T
T{ ." TOKENS include all forth standard words with their category " CR
    TOKENS S" SWAP"   FIND-ITEM ITEM>VALUE $STACK ?S
    TOKENS S" /MOD"   FIND-ITEM ITEM>VALUE $OPERATOR ?S
    TOKENS S" UNUSED" FIND-ITEM ITEM>VALUE $MEMORY ?S
    TOKENS S" CREATE" FIND-ITEM ITEM>VALUE $DEFINING ?S
    TOKENS S" ENDIF"  FIND-ITEM ITEM>VALUE $CONTROL ?S
    TOKENS S\" S\""   FIND-ITEM ITEM>VALUE $STRING ?S
    TOKENS S\" .\""   FIND-ITEM ITEM>VALUE $STRING ?S
    TOKENS S" ("      FIND-ITEM ITEM>VALUE $COMMENT ?S
    TOKENS S" \"      FIND-ITEM ITEM>VALUE $LCOMMENT ?S
}T
T{ ." RGB-WEIGHT creates a value with color and font weight attributes" CR
    42 17 23 TRUE RGBW-VALUE
    DUP RGBW>COLOR HEX6 S" 2A1117" STR= ?TRUE
    RGBW>FONT-WEIGHT ?TRUE
}T
T{ ." ATTRIBUTE stores the attributes for each category" CR
    42 17 23 TRUE RGBW-VALUE $COMMENT ATTRIBUTES ! 
    $COMMENT ATTRIBUTES @ RGBW>COLOR HEX6 S" 2A1117" STR= ?TRUE
}T

BYE
