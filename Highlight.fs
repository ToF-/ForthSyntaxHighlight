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
        1+
        _KEY
    REPEAT ;
