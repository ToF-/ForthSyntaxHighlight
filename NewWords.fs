VARIABLE DEFINING

: ITEM-LIST \ cccccbl ( -- list )
    CREATE 0 , ;

: ADD-ITEM ( list addr # -- )
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
DEFINERS S" :" ADD-ITEM 
DEFINERS S" CREATE" ADD-ITEM
DEFINERS S" VARIABLE" ADD-ITEM
DEFINERS S" CONSTANT" ADD-ITEM
DEFINERS S" 2VARIABLE" ADD-ITEM
DEFINERS S" 2CONSTANT" ADD-ITEM

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
        KEY? 0= IF EXIT THEN
        KEY DUP IS-TOKEN-MATERIAL? 0= WHILE
        EMIT
    REPEAT ;

: GET-TOKEN ( -- len )
    0
    SKIP-SPACE
    BEGIN
        KEY? 0= IF EXIT THEN
        DUP IS-TOKEN-MATERIAL? WHILE
        OVER PAD + C!
        1+
        KEY? 0= IF EXIT THEN
        KEY
    REPEAT EMIT ;

: IS-DEFINER? ( addr #  -- ? )
    DEFINERS FIND-ITEM ;

: NAME-COLOR
    ESC[ ." 34m" ;
    
: NUMBER-COLOR
    ESC[ ." 32m" ;

: NEW-WORD-COLOR 
    ESC[ ." 31m" ;

: PRETTY-PRINT
    BEGIN
        KEY? 0= IF EXIT THEN
        GET-TOKEN
        DUP ?DUP IF
            DEFINING @ IF
                NEW-WORD-COLOR
                DEFINING OFF
            ELSE
                PAD OVER NEW-WORDS FIND-ITEM IF
                    NEW-WORD-COLOR
                ELSE
                    PAD OVER FIND-NAME IF 
                        NAME-COLOR 
                        PAD OVER DEFINERS FIND-ITEM IF
                            DEFINING ON 
                        ELSE
                            DEFINING OFF
                        THEN
                    ELSE
                        NUMBER-COLOR
                    THEN
                THEN
            THEN
            PAD SWAP TYPE
        THEN
    0= UNTIL ;

PRETTY-PRINT BYE


