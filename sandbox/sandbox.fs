VARIABLE TOKENS
2VARIABLE TARGET

: TOKENS{ 0 ;

: TOKEN, \ <cccccc>bl ( link c-info -- link' )
    HERE -ROT
    SWAP , C,
    BL WORD COUNT S, ;

: }TOKENS
    TOKENS ! ;

: TOKEN>CATEGORY ( addr -- c )
    CELL+ C@ ;

: TOKEN>ID ( addr -- addr )
    CELL+ 1+ ;

: TOKEN>NEXT ( addr -- addr )
    @ ;

: UPPER ( addr count -- )
    OVER + SWAP DO
        I C@ DUP
        [CHAR] a [CHAR] z 1+ WITHIN
        IF TOUPPER THEN
        I C!
    LOOP ;

: FIND-TOKEN ( addr count -- 'token|0 )
    2DUP UPPER
    TARGET 2!  TOKENS @
    BEGIN
        DUP WHILE
        DUP TOKEN>ID COUNT
        TARGET 2@ COMPARE
        0= IF EXIT THEN
        TOKEN>NEXT
    REPEAT ;

: .TOKEN ( 'token -- )
    CELL+ DUP
    1+ COUNT TYPE SPACE
    [CHAR] : EMIT SPACE
    C@ . CR ;

: .TOKENS
    TOKENS @
    BEGIN
        DUP @ WHILE
        DUP .TOKEN
        @
    REPEAT DROP ;

unused
TOKENS{
    48   TOKEN, QUX
    42   TOKEN, BAR
    17   TOKEN, FOO
}TOKENS

." used:" unused - . cr
.TOKENS

S" bar" FIND-TOKEN .S .TOKEN
S" Zip" FIND-TOKEN .S

: IS-TOKEN-CHAR? ( c -- ? )
    33 128 WITHIN ;

: SIZE ( n -- size )
    1 BEGIN
        SWAP 10 / DUP WHILE
        SWAP 1+
    REPEAT DROP ;

: .COLOR ( code -- )
    ESC[ ." 38;5;" DUP SIZE .R ." m" ;

: DISPLAY-COLORS
    256 0 DO
        I DUP 16 MOD 0= IF CR THEN
        DUP .COLOR
        DUP 3 .R
        ." ### $
    LOOP ;


4 CONSTANT EOF


: CAT 
    0 0
    BEGIN
        KEY DUP EOF <> WHILE
        DUP IS-TOKEN-CHAR? IF
            SWAP 0= IF
                ROT 1+ DUP .COLOR
                -ROT
            THEN
            TRUE SWAP
        ELSE
            SWAP DROP FALSE SWAP
        THEN
        EMIT  
    REPEAT ;

CAT
BYE

