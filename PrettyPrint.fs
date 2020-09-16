VARIABLE DEFINING
VARIABLE IN-STRING?
VARIABLE IN-COMMENT?
VARIABLE IN-LINE-COMMENT?

CREATE TOKEN 1024 ALLOT

: ITEM-LIST \ cccccbl ( -- list )
    CREATE 0 , ;

: FIND-ITEM ( addr # list -- addr | 0 )
    -ROT 2>R
    BEGIN
        @ DUP WHILE
        DUP CELL+ COUNT
        2R@ COMPARE 0= IF 2R> 2DROP EXIT THEN
    REPEAT 2R> 2DROP ;

: ], ( list addr # -- list )
    2>R HERE OVER @ , OVER ! 2R> S, ;

: ]. ( list -- )
    ], DROP ;

: %[ ( -- addr # )
    BL PARSE ;

: .ITEMS ( addr -- )
    BEGIN
        @ DUP WHILE
        DUP CELL+ COUNT TYPE SPACE
    REPEAT DROP ;



ITEM-LIST DEFINERS
DEFINERS
%[ : ],
%[ CREATE ],
%[ VARIABLE ],
%[ CONSTANT ],
%[ 2VARIABLE ],
%[ 2CONSTANT ].

ITEM-LIST FORTH-OPERATORS
FORTH-OPERATORS
%[ + ], %[ - ], %[ * ], %[ / ], %[ MOD ], %[ /MOD ], %[ NEGATE ], %[ AB[ ], %[ MIN ], %[ MAX ],
%[ AND ], %[ OR ], %[ XOR ], %[ NOT ], %[ LSHIFT ], %[ RSHIFT ], %[ INVERT ], %[ 2* ], %[ 2/ ], %[ 1+ ],
%[ 1- ], %[ 2+ ], %[ 2- ], %[ 8* ], %[ UNDER+ ],
%[ M+ ], %[ */ ], %[ */MOD ], %[ M* ], %[ UM* ], %[ M*/ ], %[ UM/MOD ], %[ FM/MOD ], %[ SM/REM ],
%[ D+ ], %[ D- ], %[ DNEGATE ], %[ DAB[ ], %[ DMIN ], %[ DMAX ], %[ D2* ], %[ D2/ ],
%[ F+ ], %[ F- ], %[ F* ], %[ F/ ], %[ FNEGATE ], %[ FAB[ ], %[ FMAX ], %[ FMIN ], %[ FLOOR ], %[ FROUND ],
%[ F** ], %[ FSQRT ], %[ FEXP ], %[ FEXPM1 ], %[ FLN ], %[ FLNP1 ], %[ FLOG ], %[ FALOG ], %[ FSIN ],
%[ FCO[ ], %[ FSINCO[ ], %[ FTAN ], %[ FASIN ], %[ FACO[ ], %[ FATAN ], %[ FATAN2 ], %[ FSINH ],
%[ FCOSH ], %[ FTANH ], %[ FASINH ], %[ FACOSH ], %[ FATANH ], %[ F2* ], %[ F2/ ], %[ 1/F ],
%[ F~REL ], %[ F~AB[ ], %[ F~ ],
%[ 0< ], %[ 0<= ], %[ 0<> ], %[ 0= ], %[ 0> ], %[ 0>= ], %[ < ], %[ <= ], %[ <> ], %[ = ], %[ > ], %[ >= ], %[ U< ], %[ U<= ],
%[ U> ], %[ U>= ], %[ D0< ], %[ D0<= ], %[ D0<> ], %[ D0= ], %[ D0> ], %[ D0>= ], %[ D< ], %[ D<= ], %[ D<> ],
%[ D= ], %[ D> ], %[ D>= ], %[ DU< ], %[ DU<= ], %[ DU> ], %[ DU>= ], %[ WITHIN ], %[ ?NEGATE ],
%[ ?DNEGATE ], %[ TRUE ], %[ FALSE ], %[ . ], %[ U. ], %[ .R ], %[ U.R ].

ITEM-LIST CONTROL-WORDS
CONTROL-WORDS
%[ IF ], %[ ELSE ], %[ ENDIF ], %[ THEN ], %[ CASE ], %[ OF ], %[ ENDOF ], %[ ENDCASE ], %[ ?DUP-IF ],
%[ ?DUP-0=-IF ], %[ AHEAD ], %[ CS-PICK ], %[ CS-ROLL ], %[ CATCH ], %[ THROW ], %[ WITHIN ],
%[ BEGIN ], %[ WHILE ], %[ REPEAT ], %[ UNTIL ], %[ AGAIN ],
%[ ?DO ], %[ LOOP ], %[ I ], %[ J ], %[ K ], %[ +DO ], %[ U+DO ], %[ -DO ], %[ U-DO ], %[ DO ], %[ +LOOP ], %[ -LOOP ],
%[ UNLOOP ], %[ LEAVE ], %[ ?LEAVE ], %[ EXIT ], %[ DONE ], %[ FOR ], %[ NEXT ], %[ RECURSE ].

ITEM-LIST STACK-WORDS
STACK-WORDS
%[ DROP ], %[ NIP ], %[ DUP ], %[ OVER ], %[ TUCK ], %[ SWAP ], %[ ROT ], %[ -ROT ], %[ ?DUP ], %[ PICK ], %[ ROLL ],
%[ 2DROP ], %[ 2NIP ], %[ 2DUP ], %[ 2OVER ], %[ 2TUCK ], %[ 2SWAP ], %[ 2ROT ], %[ 2-ROT ],
%[ 3DUP ], %[ 4DUP ], %[ 5DUP ], %[ 3DROP ], %[ 4DROP ], %[ 5DROP ], %[ 8DROP ], %[ 4SWAP ], %[ 4ROT ],
%[ 4-ROT ], %[ 4TUCK ], %[ 8SWAP ], %[ 8DUP ],
%[ >R ], %[ R> ], %[ R@ ], %[ RDROP ], %[ 2>R ], %[ 2R> ], %[ 2R@ ], %[ 2RDROP ],
%[ 4>R ], %[ 4R> ], %[ 4R@ ], %[ 4RDROP ],
%[ FDROP ], %[ FNIP ], %[ FDUP ], %[ FOVER ], %[ FTUCK ], %[ FSWAP ], %[ FROT ],
%[ SP@ ], %[ SP! ], %[ FP@ ], %[ FP! ], %[ RP@ ], %[ RP! ], %[ LP@ ], %[ LP! ], %[ DEPTH ].

ITEM-LIST ADDRESS-WORDS
ADDRESS-WORDS
%[ @ ], %[ ! ], %[ +! ], %[ C@ ], %[ C! ], %[ 2@ ], %[ 2! ], %[ F@ ], %[ F! ], %[ SF@ ], %[ SF! ], %[ DF@ ], %[ DF! ],
%[ CHARS ], %[ CHAR+ ], %[ CELLS ], %[ CELL+ ], %[ CELL ], %[ ALIGN ], %[ ALIGNED ], %[ FLOATS ],
%[ FLOAT+ ], %[ FLOAT ], %[ FALIGN ], %[ FALIGNED ], %[ SFLOATS ], %[ SFLOAT+ ],
%[ SFALIGN ], %[ SFALIGNED ], %[ DFLOATS ], %[ DFLOAT+ ], %[ DFALIGN ], %[ DFALIGNED ],
%[ MAXALIGN ], %[ MAXALIGNED ], %[ CFALIGN ], %[ CFALIGNED ],
%[ ADDRESS-UNIT-BITS ], %[ ALLOT ], %[ ALLOCATE ], %[ HERE ],
%[ MOVE ], %[ ERASE ], %[ CMOVE ], %[ CMOVE> ], %[ FILL ], %[ BLANK ], %[ UNUSED ].

ITEM-LIST STRING-WORDS
STRING-WORDS %[ S" ], %[ ." ], %[ ABORT" ].

ITEM-LIST COMMENT-WORDS
COMMENT-WORDS S" (" ].

ITEM-LIST NEW-WORDS

: IS-SPACE? ( c -- )
    DUP  32 =
    OVER 13 = OR
    SWAP 10 = OR ;

: IS-TOKEN-MATERIAL? ( c -- )
    33 256 WITHIN ;

: TIL-EOL ( -- )
    BEGIN
        KEY DUP 4 <> OVER 13 <> AND OVER 10 <> AND WHILE
        EMIT
    REPEAT 
    DUP 4 <> IF EMIT ELSE DROP THEN 
    IN-LINE-COMMENT? OFF ;

: TIL-EOC ( -- )
    BEGIN
        KEY DUP 4 <> OVER [CHAR] ) <> AND OVER 13 <> AND OVER 10 <> AND WHILE
        EMIT
    REPEAT 
    DUP 4 <> IF EMIT ELSE DROP THEN 
    IN-COMMENT? OFF ;

: TIL-EOS ( -- )
    BEGIN
        KEY DUP 4 <> OVER [CHAR] " <> AND OVER 13 <> AND OVER 10 <> AND WHILE
        EMIT
    REPEAT 
    DUP 4 <> IF EMIT ELSE DROP THEN 
    IN-STRING? OFF ;

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

: STRING-COLOR
    ESC[ ." 32m" ;

: COMMENT-COLOR
    ESC[ ." 35m" ;

: NEW-WORD-COLOR
    ESC[ ." 37m" ;

: DEFINING-COLOR
    ESC[ ." 37m" ;

: OPERATOR-COLOR 
    ESC[ ." 31m" ;

: CONTROL-COLOR
    ESC[ ." 33m" ;

: STACK-WORD-COLOR
    ESC[ ." 33m" ;

: ADDRESS-WORD-COLOR
    ESC[ ." 38m" ;

: DEFINE ( addr # -- )
    NEW-WORDS -ROT ]. ;

: IS-DEFINER? ( addr # -- ? )
    DEFINERS FIND-ITEM ;

: IS-NEW-WORD? ( addr # -- ? )
    NEW-WORDS FIND-ITEM ;
   
: PRETTY-PRINT
    IN-STRING? OFF
    IN-COMMENT? OFF
    IN-LINE-COMMENT? OFF
    BEGIN
        IN-STRING? @ IF TIL-EOS THEN
        IN-COMMENT? @ IF TIL-EOC THEN
        IN-LINE-COMMENT? @ IF TIL-EOL THEN
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
                    TOKEN OVER STRING-WORDS FIND-ITEM IF 
                        IN-STRING? ON
                        STRING-COLOR
                    THEN
                    TOKEN OVER COMMENT-WORDS FIND-ITEM IF 
                        IN-COMMENT? ON
                        COMMENT-COLOR
                    THEN
                    TOKEN OVER S" \" COMPARE 0= IF
                        IN-LINE-COMMENT? ON
                        COMMENT-COLOR
                    THEN \ this is a line comment
                    TOKEN OVER IS-DEFINER? DEFINING !
                    TOKEN OVER FORTH-OPERATORS FIND-ITEM IF OPERATOR-COLOR THEN
                    TOKEN OVER CONTROL-WORDS FIND-ITEM IF CONTROL-COLOR THEN
                    TOKEN OVER STACK-WORDS FIND-ITEM IF STACK-WORD-COLOR THEN
                    TOKEN OVER ADDRESS-WORDS FIND-ITEM IF ADDRESS-WORD-COLOR THEN
                ELSE
                    NUMBER-COLOR
                THEN
            THEN
        THEN
        TOKEN SWAP TYPE
        EMIT
    REPEAT ;

PRETTY-PRINT BYE


