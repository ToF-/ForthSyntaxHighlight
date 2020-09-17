VARIABLE DEFINING           \ the next token will be a user defined word
VARIABLE IN-STRING?         \ chars currently output are inside a string
VARIABLE IN-COMMENT?        \ chars currently output are inside a comment
VARIABLE IN-LINE-COMMENT?   \ chars currently output are part of a line comment

\ categories of forth tokens
 0 CONSTANT NAME_
 1 CONSTANT NUMBER_
 2 CONSTANT STRING_
 3 CONSTANT COMMENT_
 4 CONSTANT LINE-COMMENT_
 5 CONSTANT NEW-WORD_
 6 CONSTANT DEFINING_
 7 CONSTANT OPERATOR_
 8 CONSTANT CONTROL_
 9 CONSTANT STACK_
10 CONSTANT ADDRESS_

\ ANSI color codes associated with categories
CREATE COLORS 
33 C,  \ NAME
32 C,  \ NUMBER
32 C,  \ STRING
35 C,  \ COMMENT
35 C,  \ LINE-COMMENT
36 C,  \ NEW WORD
37 C,  \ DEFINING WORD
34 C,  \ OPERATOR
34 C,  \ CONTROL
33 C,  \ STACK
33 C,  \ ADDRESS

\ Logical Color	Terminal Color	RGB Value Used by SGD
\ Color_0	Black	0 0 0
\ Color_1	Light red	255 0 0
\ Color_2	Light green	0 255 0
\ Color_3	Yellow	255 255 0
\ Color_4	Light blue	0 0 255
\ Color_5	Light magenta	255 0 255
\ Color_6	Light cyan	0 255 255
\ Color_7	High white	255 255 255
\ Color_8	Gray	128 128 128
\ Color_9	Red	128 0 0
\ Color_10	Green	0 128 0
\ Color_11	Brown	128 128 0
\ Color_12	Blue	0 0 128
\ Color_13	Magenta	128 0 128
\ Color_14	Cyan	0 128 128
\ Color_15	White	192 192 192

\ RGB color codes associated with categories
CREATE RGBCOLORS
HEX
800000 ,  \ NAME
008000 ,  \ NUMBER
008000 ,  \ STRING
800080 ,  \ COMMENT
800080 ,  \ LINE-COMMENT
008080 ,  \ NEW WORD
c0c0c0 ,  \ DEFINING WORD
000080 ,  \ OPERATOR
000080 ,  \ CONTROL
808000 ,  \ STACK
808000 ,  \ ADDRESS

: .HEX-BYTE ( c -- )
    DUP 000F0 AND 4 RSHIFT HEX-DIGIT EMIT
    000F AND HEX-DIGIT EMIT ;

: .RGB-HEX ( n -- )
    [CHAR] # EMIT
    DUP FF0000 AND 10 RSHIFT .HEX-BYTE
    DUP 00FF00 AND 08 RSHIFT .HEX-BYTE
        0000FF AND           .HEX-BYTE ;
DECIMAL

CREATE TOKEN 1024 ALLOT

: TOKEN-LIST \ cccccbl ( -- list )
    CREATE 0 , ;

: FIND-TOKEN ( addr # list -- addr | 0 )
    -ROT 2>R
    BEGIN
        @ DUP WHILE
        DUP CELL+ CELL+ COUNT
        2R@ COMPARE 0= IF 2R> 2DROP EXIT THEN
    REPEAT 2R> 2DROP ;

: TOKEN>CATEGORY ( addr -- info )
    CELL+ @ ;

: L, ( list info addr # -- list info )
    2>R SWAP HERE OVER @ , OVER ! SWAP DUP , 2R> S, ;

: L. ( list,info -- )
    L, 2DROP ;

: L ( -- addr # )
    BL PARSE ;

: .TOKENS ( addr -- )
    BEGIN
        @ DUP WHILE
        DUP CELL+ CELL+ COUNT TYPE SPACE
    REPEAT DROP ;


TOKEN-LIST FORTH-TOKENS
FORTH-TOKENS DEFINING_
L :         L, L CREATE    L, L VARIABLE  L, L CONSTANT  L, L 2VARIABLE L, L 2CONSTANT L.

FORTH-TOKENS OPERATOR_
L + L, L - L, L * L, L / L, L MOD L, L /MOD L, L NEGATE L, L AB[ L, L MIN L, L MAX L, L AND L, L OR L, L XOR L, L NOT L, L LSHIFT L, L RSHIFT L, L INVERT L, L 2* L, L 2/ L, L 1+ L, L 1- L, L 2+ L, L 2- L, L 8* L, L UNDER+ L, L M+ L, L */ L, L */MOD L, L M* L, L UM* L, L M*/ L, L UM/MOD L, L FM/MOD L, L SM/REM L, L D+ L, L D- L, L DNEGATE L, L DAB[ L, L DMIN L, L DMAX L, L D2* L, L D2/ L, L F+ L, L F- L, L F* L, L F/ L, L FNEGATE L, L FAB[ L, L FMAX L, L FMIN L, L FLOOR L, L FROUND L, L F** L, L FSQRT L, L FEXP L, L FEXPM1 L, L FLN L, L FLNP1 L, L FLOG L, L FALOG L, L FSIN L, L FCO[ L, L FSINCO[ L, L FTAN L, L FASIN L, L FACO[ L, L FATAN L, L FATAN2 L, L FSINH L, L FCOSH L, L FTANH L, L FASINH L, L FACOSH L, L FATANH L, L F2* L, L F2/ L, L 1/F L, L F~REL L, L F~AB[ L, L F~ L, L 0< L, L 0<= L, L 0<> L, L 0= L, L 0> L, L 0>= L, L < L, L <= L, L <> L, L = L, L > L, L >= L, L U< L, L U<= L, L U> L, L U>= L, L D0< L, L D0<= L, L D0<> L, L D0= L, L D0> L, L D0>= L, L D< L, L D<= L, L D<> L, L D= L, L D> L, L D>= L, L DU< L, L DU<= L, L DU> L, L DU>= L, L WITHIN L, L ?NEGATE L, L ?DNEGATE L, L TRUE L, L FALSE L, L . L, L U. L, L .R L, L U.R L.

FORTH-TOKENS CONTROL_
L IF L, L ELSE L, L ENDIF L, L THEN L, L CASE L, L OF L, L ENDOF L, L ENDCASE L, L ?DUP-IF L, L ?DUP-0=-IF L, L AHEAD L, L CS-PICK L, L CS-ROLL L, L CATCH L, L THROW L, L WITHIN L, L BEGIN L, L WHILE L, L REPEAT L, L UNTIL L, L AGAIN L, L ?DO L, L LOOP L, L I L, L J L, L K L, L +DO L, L U+DO L, L -DO L, L U-DO L, L DO L, L +LOOP L, L -LOOP L, L UNLOOP L, L LEAVE L, L ?LEAVE L, L EXIT L, L DONE L, L FOR L, L NEXT L, L RECURSE L.

FORTH-TOKENS STACK_
L DROP L, L NIP L, L DUP L, L OVER L, L TUCK L, L SWAP L, L ROT L, L -ROT L, L ?DUP L, L PICK L, L ROLL L, L 2DROP L, L 2NIP L, L 2DUP L, L 2OVER L, L 2TUCK L, L 2SWAP L, L 2ROT L, L 2-ROT L, L 3DUP L, L 4DUP L, L 5DUP L, L 3DROP L, L 4DROP L, L 5DROP L, L 8DROP L, L 4SWAP L, L 4ROT L, L 4-ROT L, L 4TUCK L, L 8SWAP L, L 8DUP L, L >R L, L R> L, L R@ L, L RDROP L, L 2>R L, L 2R> L, L 2R@ L, L 2RDROP L, L 4>R L, L 4R> L, L 4R@ L, L 4RDROP L, L FDROP L, L FNIP L, L FDUP L, L FOVER L, L FTUCK L, L FSWAP L, L FROT L, L SP@ L, L SP! L, L FP@ L, L FP! L, L RP@ L, L RP! L, L LP@ L, L LP! L, L DEPTH L.

FORTH-TOKENS ADDRESS_
L @ L, L ! L, L +! L, L C@ L, L C! L, L 2@ L, L 2! L, L F@ L, L F! L, L SF@ L, L SF! L, L DF@ L, L DF! L, L CHARS L, L CHAR+ L, L CELLS L, L CELL+ L, L CELL L, L ALIGN L, L ALIGNED L, L FLOATS L, L FLOAT+ L, L FLOAT L, L FALIGN L, L FALIGNED L, L SFLOATS L, L SFLOAT+ L, L SFALIGN L, L SFALIGNED L, L DFLOATS L, L DFLOAT+ L, L DFALIGN L, L DFALIGNED L, L MAXALIGN L, L MAXALIGNED L, L CFALIGN L, L CFALIGNED L, L ADDRESS-UNIT-BITS L, L ALLOT L, L ALLOCATE L, L HERE L, L MOVE L, L ERASE L, L CMOVE L, L CMOVE> L, L FILL L, L BLANK L, L UNUSED L.

FORTH-TOKENS STRING_ L S" L, L ." L, L ABORT" L.

FORTH-TOKENS COMMENT_ S" (" L.

FORTH-TOKENS LINE-COMMENT_ S" \" L.
\ "

: IS-SPACE? ( c -- ? )
    DUP  32 =
    OVER 13 = OR
    SWAP 10 = OR ;

: IS-TOKEN-MATERIAL? ( c -- ? )
    33 256 WITHIN ;

: IS-EOL? ( c -- ? )
    DUP 4 =
    OVER 13 = OR
    SWAP 10 = OR ;

: ?EMIT ( c -- )
    DUP 4 <> IF EMIT ELSE DROP THEN ;

: TIL-EOL ( -- )
    BEGIN
        KEY DUP IS-EOL? 0= WHILE
        EMIT
    REPEAT
    ?EMIT
    IN-LINE-COMMENT? OFF ;

: TIL-EOC ( -- )
    BEGIN
        KEY DUP IS-EOL? 0=
        OVER [CHAR] ) <> AND WHILE
        EMIT
    REPEAT
    ?EMIT
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
        KEY DUP 4 <>
        OVER IS-TOKEN-MATERIAL? 0= AND WHILE
        EMIT
    REPEAT ;

: GET-TOKEN ( -- len,c )
    0
    SKIP-SPACE
    BEGIN
        DUP 4 <>
        OVER IS-TOKEN-MATERIAL? AND WHILE
        OVER TOKEN + C!
        1+
        KEY
    REPEAT ;

: COLOR ( category -- )
    COLORS + C@
    ESC[ 2 .R [CHAR] m EMIT ;

: DEFINE ( addr # -- )
    FORTH-TOKENS NEW-WORD_ 2SWAP L. ;

: ?TIL-MODES ( category -- )
    DUP STRING_ = IF IN-STRING? ON DROP
    ELSE DUP COMMENT_ = IF IN-COMMENT? ON DROP
    ELSE LINE-COMMENT_ = IF IN-LINE-COMMENT? ON THEN
    THEN THEN ;

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
            DEFINING_ COLOR
        ELSE
            TOKEN OVER FIND-NAME IF
                NAME_ COLOR
                TOKEN OVER FORTH-TOKENS FIND-TOKEN ?DUP IF
                    TOKEN>CATEGORY
                    DUP COLOR
                    DUP ?TIL-MODES
                    DEFINING_ = IF
                        DEFINING ON
                    THEN
                THEN \ this is a line comment
            ELSE
                NUMBER_ COLOR
            THEN
        THEN
        TOKEN SWAP TYPE
        EMIT
    REPEAT ;

ESC[ ." 0m"
PRETTY-PRINT
ESC[ ." 0m"
BYE


