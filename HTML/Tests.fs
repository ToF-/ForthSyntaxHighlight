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
BYE
