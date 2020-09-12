VARIABLE TOKENS

: TOKENS{
    HERE 0 , ;

: TOKEN, \ <cccccc>bl ( link info -- link' )
    HERE >R
    SWAP , ,
    BL WORD COUNT S,
    R> ;

: }TOKENS
    TOKENS ! ;

: .TOKEN ( 'token -- )
    CELL+ DUP 
    CELL+ COUNT TYPE SPACE 
    [CHAR] : EMIT SPACE
    @ . CR ;

: .TOKENS
    TOKENS @ 
    BEGIN
        DUP @ WHILE
        DUP .TOKEN
        @
    REPEAT DROP ;

unused 
TOKENS{ 
    4807 TOKEN, Qux
    42   TOKEN, Bar
    17   TOKEN, Foo
}TOKENS 

." used:" unused - . cr
.TOKENS
BYE
