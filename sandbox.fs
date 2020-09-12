VARIABLE TOKENS

: TOKENS{
    HERE 0 , ;

: TOKEN, \ <cccccc>bl ( link c-info -- link' )
    HERE >R
    SWAP , C,
    BL WORD COUNT S,
    R> ;

: }TOKENS
    TOKENS ! ;

: FIND-TOKEN ( addr count -- 'token|0 )
    2>R
    TOKENS @
    BEGIN
        DUP @ WHILE
        DUP CELL+ 1+ COUNT 2R@ COMPARE 0= IF
            2R> 2DROP EXIT
        THEN
        @
    REPEAT 2R> 2DROP DROP 0 ;

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
    48   TOKEN, Qux
    42   TOKEN, Bar
    17   TOKEN, Foo
}TOKENS 

." used:" unused - . cr
.TOKENS

S" Bar" FIND-TOKEN .S .TOKEN
S" Zip" FIND-TOKEN .S 

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

DISPLAY-COLORS
BYE
