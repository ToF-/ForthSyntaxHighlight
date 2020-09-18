INCLUDE ffl/tst.fs
INCLUDE HighLight.fs

CREATE _OUTPUT 1024 ALLOT
CREATE _INPUT  1024 ALLOT
VARIABLE _OUTPUT#
VARIABLE _INPUT#
VARIABLE _#INPUT

: EMIT>! ( c -- )
    _OUTPUT# @
    _OUTPUT + C!
    1 _OUTPUT# +! ;

: TYPE>! ( addr # -- )
    0 ?DO DUP I + C@ EMIT>! LOOP DROP ;

: KEY<@ ( -- c )
    _INPUT# @ _#INPUT @ < IF
        _INPUT _INPUT# @ + C@
        1 _INPUT# +!
    ELSE
        4
    THEN ;

: RESET-OUTPUT
    _OUTPUT# OFF ;

: SET-INPUT ( addr # -- )
    _INPUT# OFF
    _#INPUT !
    _INPUT _#INPUT @ CMOVE ;

: ?OUTPUT ( addr # -- ? )
    _OUTPUT _OUTPUT# @
    COMPARE 0 ?S ;
  
' EMIT>! IS _EMIT
' TYPE>! IS _TYPE
' KEY<@ IS _KEY

T{ ." _EMIT can be redirected for the sake of testing" CR
    RESET-OUTPUT
   42 _EMIT 46 _EMIT 44 _EMIT
   S" *.," ?OUTPUT
}T

T{ ." _TYPE can be redirected for the sake of testing" CR
    _OUTPUT# OFF
    S" Hello World!" _TYPE
    S" Hello World!" ?OUTPUT
}T

T{ ." _KEY can be redirected for the sake of testing" CR
    S" *.," SET-INPUT
    _KEY 42 ?S
    _KEY 46 ?S
    _KEY 44 ?S
    _KEY 4 ?S
}T

T{ ." .COLOR changes the terminal color" CR
    RESET-OUTPUT
    2 .COLOR
    _OUTPUT C@ 27 ?S
    _OUTPUT 1+ 4 S" [32m" COMPARE 0 ?S
}T

T{ ." .SKIP-SPACE prints input chars until non space found" CR
    RESET-OUTPUT
    S"    FOO" SET-INPUT
    .SKIP-SPACE
    CHAR F ?S
    S"    " ?OUTPUT
}T

T{ ." .SKIP-SPACE prints input chars until non space found" CR
    RESET-OUTPUT
    S\"   \t\n\rFOO" SET-INPUT
    .SKIP-SPACE
    CHAR F ?S
    S\"   \t\n\r" ?OUTPUT
}T
T{ ." .SKIP-SPACE yields EOF is no input chars anymore" CR
    RESET-OUTPUT
    S\"    " SET-INPUT
    .SKIP-SPACE
    4 ?S
}T
T{ ." .>TOKEN gets the next token from the input stream" CR
    S"    FOO  " SET-INPUT
    RESET-OUTPUT
    .>TOKEN 32 ?S
    TOKEN SWAP S" FOO" COMPARE 0 ?S
    S"    " ?OUTPUT
}T

T{ ." .SKIP-LINE prints input chars until end of line" CR
    S\" foo bar \n\r" SET-INPUT
    RESET-OUTPUT
    .SKIP-LINE
    S\" foo bar \n" ?OUTPUT
}T
T{ ." .SKIP-LINE prints input chars until end of input" CR
    S\" foo" SET-INPUT
    RESET-OUTPUT
    .SKIP-LINE
    S\" foo" ?OUTPUT
}T
T{ ." .SKIP-COMMENT prints input chars until end of comment or input" CR
    S\" foo ) bar \n\r" SET-INPUT
    RESET-OUTPUT
    .SKIP-COMMENT
    S\" foo )" ?OUTPUT
    S\" foo" SET-INPUT
    RESET-OUTPUT
    .SKIP-COMMENT
    S\" foo" ?OUTPUT
}T
T{ ." .SKIP-STRING prints input chars until double quote or end of input" CR
    S\" foo \" bar \n\r" SET-INPUT
    RESET-OUTPUT
    .SKIP-STRING
    S\" foo \"" ?OUTPUT
    S\" foo" SET-INPUT
    RESET-OUTPUT
    .SKIP-STRING
    S\" foo" ?OUTPUT
}T

T{ ." a LINKED-LIST starts with 0 as the first link " CR
    LINKED-LIST FOO
    FOO @ 0 ?S
}T

T{ ." LINK, adds an item to a new linked list links to 0 " CR
    FOO S" Bar" 4807 LINK,
    FOO @ LINK>VALUE 4807 ?S
    FOO @ LINK>NAME COUNT S" Bar" COMPARE 0 ?S
    FOO @ LINK>LINK 0 ?S
}T

T{ ." FIND-LINK finds an item or returns 0 " CR
    FOO S" Qux" 42 LINK,
    FOO S" Bar" FIND-LINK LINK>VALUE 4807 ?S
    FOO S" Qux" FIND-LINK LINK>VALUE 42 ?S
    FOO S" Baz" FIND-LINK 0 ?S
}T

T{ ." TOKENS include all forth standard words with their category " CR
    TOKENS S" SWAP" FIND-LINK LINK>VALUE STACK-CATEGORY ?S
    TOKENS S" /MOD" FIND-LINK LINK>VALUE OPERATOR-CATEGORY ?S
    TOKENS S" UNUSED" FIND-LINK LINK>VALUE ADDRESS-CATEGORY ?S
    TOKENS S" CREATE" FIND-LINK LINK>VALUE DEFINING-CATEGORY ?S
    TOKENS S" ENDIF" FIND-LINK LINK>VALUE CONTROL-CATEGORY ?S
}T
BYE

