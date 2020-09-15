VARIABLE DEFINING

CREATE TOKEN 1024 ALLOT

: ITEM-LIST \ cccccbl ( -- list )
    CREATE 0 , ;

: ADD-ITEM ( addr # list -- )
    -ROT
    2>R
    HERE OVER @ ,
    SWAP !
    2R> S, ;

: FIND-ITEM ( addr # list -- addr | 0 )
    -ROT 2>R
    BEGIN
        @ DUP WHILE
        DUP CELL+ COUNT
        2R@ COMPARE 0= IF 2R> 2DROP EXIT THEN
    REPEAT 2R> 2DROP ;

            
ITEM-LIST DEFINERS
S" :"         DEFINERS ADD-ITEM 
S" CREATE"    DEFINERS ADD-ITEM
S" VARIABLE"  DEFINERS ADD-ITEM
S" CONSTANT"  DEFINERS ADD-ITEM
S" 2VARIABLE" DEFINERS ADD-ITEM
S" 2CONSTANT" DEFINERS ADD-ITEM

ITEM-LIST NEW-WORDS

: .ITEMS ( addr -- )
    BEGIN
        @ DUP WHILE
        DUP CELL+ COUNT TYPE SPACE
    REPEAT DROP ;

: IS-SPACE? ( c -- )
    DUP  32 =
    OVER 13 = OR
    SWAP 10 = OR ;

: IS-TOKEN-MATERIAL? ( c -- )
    33 256 WITHIN ;

: SKIP-SPACE ( -- c )
    BEGIN
        KEY DUP 4 <> OVER IS-TOKEN-MATERIAL? 0= AND WHILE
        EMIT
    REPEAT ;

: GET-TOKEN ( -- len,c )
    0
    SKIP-SPACE
    BEGIN
        DUP 4 <> OVER IS-TOKEN-MATERIAL? AND WHILE
        OVER TOKEN + C!
        1+
        KEY
    REPEAT ; 

: NAME-COLOR
    ESC[ ." 36m" ;
    
: NUMBER-COLOR
    ESC[ ." 32m" ;

: NEW-WORD-COLOR 
    ESC[ ." 34m" ;

: DEFINING-COLOR
    ESC[ ." 35m" ;

: DEFINE ( addr # -- )
    NEW-WORDS ADD-ITEM ;

: IS-DEFINER? ( addr # -- ? )
    DEFINERS FIND-ITEM ;

: IS-NEW-WORD? ( addr # -- ? )
    NEW-WORDS FIND-ITEM ;
    
: PRETTY-PRINT
    BEGIN
        GET-TOKEN SWAP
        DUP WHILE
        DEFINING @ IF
            TOKEN OVER DEFINE
            DEFINING OFF
            DEFINING-COLOR
        ELSE
            TOKEN OVER IS-NEW-WORD? IF
                NEW-WORD-COLOR
            ELSE
                TOKEN OVER FIND-NAME IF 
                    NAME-COLOR 
                    TOKEN OVER IS-DEFINER? DEFINING ! 
                ELSE
                    NUMBER-COLOR
                THEN
            THEN
        THEN
        TOKEN SWAP TYPE
        EMIT
    REPEAT ;

PRETTY-PRINT BYE


