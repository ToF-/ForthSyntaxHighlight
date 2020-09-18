\ HighLight.fs provide syntax highlighting for forth programs

DEFER _EMIT
DEFER _TYPE
DEFER _KEY

' EMIT IS _EMIT
' TYPE IS _TYPE
' KEY  IS _KEY

CREATE TOKEN 1024 ALLOT

: .COLOR ( c -- )
    27 _EMIT
    [CHAR] [ _EMIT
    [CHAR] 3 _EMIT
    [CHAR] 0 + _EMIT
    [CHAR] m _EMIT ;

: IS-SPACE? ( c -- ? )
    DUP  32 =
    OVER 10 = OR
    OVER 13 = OR
    SWAP  9 = OR ;

: .SKIP-SPACE ( -- c )
    BEGIN
        _KEY DUP 4 <>
        OVER IS-SPACE? AND WHILE
        _EMIT
    REPEAT ;

: .>TOKEN ( -- # c )
    0 .SKIP-SPACE
    BEGIN
        DUP 4 <>
        OVER IS-SPACE? 0= AND WHILE
        OVER TOKEN + C!
        1+ _KEY
    REPEAT ;

: ?EMIT ( c -- )
    DUP 4 <> IF _EMIT ELSE DROP THEN ;

: .SKIP-LINE
    BEGIN
        _KEY DUP   4 <>
             OVER 10 <> AND
             OVER 13 <> AND WHILE
        _EMIT
    REPEAT ?EMIT ;

: .SKIP-COMMENT
    BEGIN
        _KEY DUP   4 <>
             OVER [CHAR] ) <> AND WHILE
        _EMIT
    REPEAT ?EMIT ;

: .SKIP-STRING
    BEGIN
        _KEY DUP   4 <>
             OVER [CHAR] " <> AND WHILE
        _EMIT
    REPEAT ?EMIT ;

: LINKED-LIST \ name --
    CREATE 0 , ;

: L, ( list addr # n -- )
    >R ROT HERE              \ addr # list new@
    OVER @ ,                 \ addr # list new@   | link is compiled
    R>     ,                 \ addr # list new@   | n is compiled
    SWAP !                   \ addr #             | new link is saved
    S, ;

: LINK>LINK ( list -- link )
    @ ;

: LINK>NAME ( link -- addr )
    CELL+ CELL+ ;

: LINK>VALUE ( link -- n )
    CELL+ @ ;

: FIND-LINK ( list addr # -- link|0 )
    2>R LINK>LINK
    BEGIN
        DUP ?DUP IF 
            LINK>NAME COUNT
            2R@ COMPARE 
        ELSE 0 THEN WHILE
        LINK>LINK
    REPEAT 2R> 2DROP ;

