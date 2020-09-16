VARIABLE DEFINING

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

ITEM-LIST DEFINERS
DEFINERS
S" :"         ],
S" CREATE"    ],
S" VARIABLE"  ],
S" CONSTANT"  ],
S" 2VARIABLE" ],
S" 2CONSTANT" ].

ITEM-LIST FORTH-OPERATORS
FORTH-OPERATORS

S" +" ], S" -" ], S" *" ], S" /" ], S" MOD" ], S" /MOD" ], S" NEGATE" ], S" ABS" ], S" MIN" ], S" MAX" ],
S" AND" ], S" OR" ], S" XOR" ], S" NOT" ], S" LSHIFT" ], S" RSHIFT" ], S" INVERT" ], S" 2*" ], S" 2/" ], S" 1+" ],
S" 1-" ], S" 2+" ], S" 2-" ], S" 8*" ], S" UNDER+" ],
S" M+" ], S" */" ], S" */MOD" ], S" M*" ], S" UM*" ], S" M*/" ], S" UM/MOD" ], S" FM/MOD" ], S" SM/REM" ],
S" D+" ], S" D-" ], S" DNEGATE" ], S" DABS" ], S" DMIN" ], S" DMAX" ], S" D2*" ], S" D2/" ],
S" F+" ], S" F-" ], S" F*" ], S" F/" ], S" FNEGATE" ], S" FABS" ], S" FMAX" ], S" FMIN" ], S" FLOOR" ], S" FROUND" ],
S" F**" ], S" FSQRT" ], S" FEXP" ], S" FEXPM1" ], S" FLN" ], S" FLNP1" ], S" FLOG" ], S" FALOG" ], S" FSIN" ],
S" FCOS" ], S" FSINCOS" ], S" FTAN" ], S" FASIN" ], S" FACOS" ], S" FATAN" ], S" FATAN2" ], S" FSINH" ],
S" FCOSH" ], S" FTANH" ], S" FASINH" ], S" FACOSH" ], S" FATANH" ], S" F2*" ], S" F2/" ], S" 1/F" ],
S" F~REL" ], S" F~ABS" ], S" F~" ],
S" 0<" ], S" 0<=" ], S" 0<>" ], S" 0=" ], S" 0>" ], S" 0>=" ], S" <" ], S" <=" ], S" <>" ], S" =" ], S" >" ], S" >=" ], S" U<" ], S" U<=" ],
S" U>" ], S" U>=" ], S" D0<" ], S" D0<=" ], S" D0<>" ], S" D0=" ], S" D0>" ], S" D0>=" ], S" D<" ], S" D<=" ], S" D<>" ],
S" D=" ], S" D>" ], S" D>=" ], S" DU<" ], S" DU<=" ], S" DU>" ], S" DU>=" ], S" WITHIN" ], S" ?NEGATE" ],
S" ?DNEGATE" ], S" TRUE" ], S" FALSE" ].

ITEM-LIST CONTROL-WORDS
CONTROL-WORDS
S" IF" ], S" ELSE" ], S" ENDIF" ], S" THEN" ], S" CASE" ], S" OF" ], S" ENDOF" ], S" ENDCASE" ], S" ?DUP-IF" ],
S" ?DUP-0=-IF" ], S" AHEAD" ], S" CS-PICK" ], S" CS-ROLL" ], S" CATCH" ], S" THROW" ], S" WITHIN" ],
S" BEGIN" ], S" WHILE" ], S" REPEAT" ], S" UNTIL" ], S" AGAIN" ],
S" ?DO" ], S" LOOP" ], S" I" ], S" J" ], S" K" ], S" +DO" ], S" U+DO" ], S" -DO" ], S" U-DO" ], S" DO" ], S" +LOOP" ], S" -LOOP" ],
S" UNLOOP" ], S" LEAVE" ], S" ?LEAVE" ], S" EXIT" ], S" DONE" ], S" FOR" ], S" NEXT" ], S" RECURSE" ].

ITEM-LIST STACK-WORDS
STACK-WORDS
S" DROP" ], S" NIP" ], S" DUP" ], S" OVER" ], S" TUCK" ], S" SWAP" ], S" ROT" ], S" -ROT" ], S" ?DUP" ], S" PICK" ], S" ROLL" ],
S" 2DROP" ], S" 2NIP" ], S" 2DUP" ], S" 2OVER" ], S" 2TUCK" ], S" 2SWAP" ], S" 2ROT" ], S" 2-ROT" ],
S" 3DUP" ], S" 4DUP" ], S" 5DUP" ], S" 3DROP" ], S" 4DROP" ], S" 5DROP" ], S" 8DROP" ], S" 4SWAP" ], S" 4ROT" ],
S" 4-ROT" ], S" 4TUCK" ], S" 8SWAP" ], S" 8DUP" ],
S" >R" ], S" R>" ], S" R@" ], S" RDROP" ], S" 2>R" ], S" 2R>" ], S" 2R@" ], S" 2RDROP" ],
S" 4>R" ], S" 4R>" ], S" 4R@" ], S" 4RDROP" ],
S" FDROP" ], S" FNIP" ], S" FDUP" ], S" FOVER" ], S" FTUCK" ], S" FSWAP" ], S" FROT" ],
S" SP@" ], S" SP!" ], S" FP@" ], S" FP!" ], S" RP@" ], S" RP!" ], S" LP@" ], S" LP!" ], S" DEPTH" ].

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
    ESC[ ." 37m" ;

: OPERATOR-COLOR 
    ESC[ ." 31m" ;

: CONTROL-COLOR
    ESC[ ." 33m" ;

: STACK-WORD-COLOR
    ESC[ ." 35m" ;

: DEFINE ( addr # -- )
    NEW-WORDS -ROT ]. ;

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
                    TOKEN OVER FORTH-OPERATORS FIND-ITEM IF OPERATOR-COLOR THEN
                    TOKEN OVER CONTROL-WORDS FIND-ITEM IF CONTROL-COLOR THEN
                    TOKEN OVER STACK-WORDS FIND-ITEM IF STACK-WORD-COLOR THEN
                ELSE
                    NUMBER-COLOR
                THEN
            THEN
        THEN
        TOKEN SWAP TYPE
        EMIT
    REPEAT ;

PRETTY-PRINT BYE


