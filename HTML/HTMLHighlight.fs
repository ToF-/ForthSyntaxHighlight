#! /usr/local/bin/gforth

 4 CONSTANT EOF
 9 CONSTANT TAB
10 CONSTANT NL
13 CONSTANT CR%
1024 CONSTANT %TOKEN
CREATE TOKEN %TOKEN ALLOT

\ DEFER the basic output and input words so that
\ we can substitute any word instead of emit type and key
DEFER _EMIT
DEFER _TYPE
DEFER _KEY

\ use the standard emit type and key by default
\ a testing module can override this
' EMIT IS _EMIT
' TYPE IS _TYPE
' KEY  IS _KEY

\ emits a char if it's not EOF
: _?EMIT ( c -- )
    DUP EOF <> IF _EMIT ELSE DROP THEN ;

\ convert a number to 6 digit hex string
: HEX6 ( n -- addr # )
    BASE @ >R HEX
    s>d <# # # # # # # #>
    R> BASE ! ;

\ display a span tag with color and font-weight
: <SPAN>. ( rgb bold? -- )
    S\" <span style=\"color:#" _TYPE
    SWAP HEX6 _TYPE
    S\" ; font-weight:" _TYPE
    IF S\" bold" ELSE S\" normal" THEN _TYPE
    S\" ;\">" _TYPE ;
\ "
\ display an end of span tag
: </SPAN>.
    S\" </span>" _TYPE ;

\ display a pre tag with color and background
: <PRE>. ( col col -- )
    S\" <pre style=\"color:#" _TYPE
    SWAP HEX6 _TYPE
    S\" ;background:#" _TYPE
    HEX6 _TYPE
    S\" ;\">" _TYPE ;
\ "
\ display an end of pre tag
: </PRE>.
    S\" </pre>" _TYPE ;

\ true if char is a space char
: IS-SPACE? ( c -- ? )
    DUP  BL  =
    OVER NL  = OR
    OVER CR% = OR
    SWAP TAB = OR ;

\ read and print chars until a non-space is met
: SKIP-SPACE. ( -- c )
    BEGIN
        _KEY DUP EOF <>
        OVER IS-SPACE? AND WHILE
        _EMIT
    REPEAT ;

\ print a char if it's not eof
: ?_EMIT ( c -- )
    DUP EOF <> IF _EMIT ELSE DROP THEN ;

\ read and print chars until end of line is met
\ emit that end of line if it's not eof
: SKIP-LINE.
    BEGIN
        _KEY DUP  EOF <>
             OVER NL  <> AND
             OVER CR% <> AND WHILE
        _EMIT
    REPEAT ?_EMIT ;

\ read and print chars until " or eol or eof
\ emit that end of string if it's not eof
: SKIP-STRING.
    BEGIN
        _KEY DUP  EOF <>
             OVER NL  <> AND
             OVER CR% <> AND
             OVER [CHAR] " <> AND WHILE
        _EMIT
    REPEAT ?_EMIT ;

\ read and print chars until ) or eof
\ emit that end of string if it's not eof
: SKIP-COMMENT.
    BEGIN
        _KEY DUP  EOF <>
             OVER [CHAR] ) <> AND WHILE
        _EMIT
    REPEAT ?_EMIT ;

\ create a new, empty linked list
: LINKED-LIST \ name
    CREATE 0 , ;

\ chain new link in linked list
: CHAIN-LINK ( list -- )
    HERE                 \ address of the new link
    OVER @               \ address of the old link
    ,                    \ compiled in the new link
    SWAP ! ;             \ list = address of new link

\ add an item to a link list
: ITEM, ( list addr # item -- )
    >R ROT CHAIN-LINK
    R> , S, ;

\ given a link, return item
: ITEM>VALUE ( addr -- item )
    CELL+ @ ;

\ given a link, return name
: ITEM>NAME  ( addr -- str )
    CELL+ CELL+ ;

\ given a link, return next
: ITEM>NEXT ( addr -- link )
    @ ;

\ find a item by its name
: FIND-ITEM ( list addr # -- link|0 )
    2>R ITEM>NEXT
    BEGIN
        DUP ?DUP IF
            ITEM>NAME COUNT
            2R@ COMPARE
        ELSE 0 THEN WHILE
        ITEM>NEXT
    REPEAT 2R> 2DROP ;

 0 CONSTANT $NUMBER
 1 CONSTANT $STRING
 2 CONSTANT $COMMENT
 3 CONSTANT $LCOMMENT
 4 CONSTANT $USERDEF
 5 CONSTANT $DEFINING
 6 CONSTANT $OPERATOR
 7 CONSTANT $CONTROL
 8 CONSTANT $STACK
 9 CONSTANT $MEMORY
10 CONSTANT $DEFAULT

LINKED-LIST TOKENS
VARIABLE CATEGORY

: ,T \ cccccc/bl
    TOKENS BL PARSE CATEGORY @ ITEM, ;
$STACK CATEGORY !
,T DROP  ,T NIP   ,T DUP   ,T OVER   ,T TUCK  ,T SWAP  ,T ROT    ,T -ROT
,T ?DUP  ,T PICK  ,T ROLL  ,T 2DROP  ,T 2NIP  ,T 2DUP  ,T 2OVER  ,T 2TUCK
,T 2SWAP ,T 2ROT  ,T 2-ROT ,T 3DUP   ,T 4DUP  ,T 5DUP  ,T 3DROP  ,T 4DROP
,T 5DROP ,T 8DROP ,T 4SWAP ,T 4ROT   ,T 4-ROT ,T 4TUCK ,T 8SWAP  ,T 8DUP
,T >R    ,T R>    ,T R@    ,T RDROP  ,T 2>R   ,T 2R>   ,T 2R@    ,T 2RDROP
,T 4>R   ,T 4R>   ,T 4R@   ,T 4RDROP ,T FDROP ,T FNIP  ,T FDUP   ,T FOVER
,T FTUCK ,T FSWAP ,T FROT  ,T SP@    ,T SP!   ,T FP@   ,T FP!    ,T RP@
,T RP!   ,T LP@   ,T LP!   ,T DEPTH
$OPERATOR CATEGORY !
,T +      ,T -       ,T *        ,T /      ,T MOD     ,T /MOD  ,T NEGATE  ,T AB[
,T MIN    ,T MAX     ,T AND      ,T OR     ,T XOR     ,T NOT   ,T LSHIFT  ,T RSHIFT
,T INVERT ,T 2*      ,T 2/       ,T 1+     ,T 1-      ,T 2+    ,T 2-      ,T 8*
,T UNDER+ ,T M+      ,T */       ,T */MOD  ,T M*      ,T UM*   ,T M*/     ,T UM/MOD
,T FM/MOD ,T SM/REM  ,T D+       ,T D-     ,T DNEGATE ,T DAB[  ,T DMIN    ,T DMAX
,T D2*    ,T D2/     ,T F+       ,T F-     ,T F*      ,T F/    ,T FNEGATE ,T FAB[
,T FMAX   ,T FMIN    ,T FLOOR    ,T FROUND ,T F**     ,T FSQRT ,T FEXP    ,T FEXPM1
,T FLN    ,T FLNP1   ,T FLOG     ,T FALOG  ,T FSIN    ,T FCO[  ,T FSINCO[ ,T FTAN
,T FASIN  ,T FACO[   ,T FATAN    ,T FATAN2 ,T FSINH   ,T FCOSH ,T FTANH   ,T FASINH
,T FACOSH ,T FATANH  ,T F2*      ,T F2/    ,T 1/F     ,T F~REL ,T F~AB[   ,T F~
,T 0<     ,T 0<=     ,T 0<>      ,T 0=     ,T 0>      ,T 0>=   ,T <       ,T <=
,T <>     ,T =       ,T >        ,T >=     ,T U<      ,T U<=   ,T U>      ,T U>=
,T D0<    ,T D0<=    ,T D0<>     ,T D0=    ,T D0>     ,T D0>=  ,T D<      ,T D<=
,T D<>    ,T D=      ,T D>       ,T D>=    ,T DU<     ,T DU<=  ,T DU>     ,T DU>=
,T WITHIN ,T ?NEGATE ,T ?DNEGATE ,T TRUE   ,T FALSE   ,T .     ,T U.      ,T .R
,T U.R
$MEMORY CATEGORY !
,T @         ,T !        ,T +!         ,T C@      ,T C!        ,T 2@                ,T 2!      ,T F@
,T F!        ,T SF@      ,T SF!        ,T DF@     ,T DF!       ,T CHARS             ,T CHAR+   ,T CELLS
,T CELL+     ,T CELL     ,T ALIGN      ,T ALIGNED ,T FLOATS    ,T FLOAT+            ,T FLOAT   ,T FALIGN
,T FALIGNED  ,T SFLOATS  ,T SFLOAT+    ,T SFALIGN ,T SFALIGNED ,T DFLOATS           ,T DFLOAT+ ,T DFALIGN
,T DFALIGNED ,T MAXALIGN ,T MAXALIGNED ,T CFALIGN ,T CFALIGNED ,T ADDRESS-UNIT-BITS ,T ALLOT   ,T ALLOCATE
,T HERE      ,T MOVE     ,T ERASE      ,T CMOVE   ,T CMOVE>    ,T FILL              ,T BLANK   ,T UNUSED
,T C,        ,T ON       ,T OFF        ,T ,
$DEFINING CATEGORY !
,T :         ,T CREATE   ,T VARIABLE   ,T CONSTANT ,T 2VARIABLE ,T 2CONSTANT ,T DEFER
$CONTROL CATEGORY !
,T IF      ,T ELSE       ,T ENDIF  ,T THEN    ,T CASE    ,T OF    ,T ENDOF ,T ENDCASE
,T ?DUP-IF ,T ?DUP-0=-IF ,T AHEAD  ,T CS-PICK ,T CS-ROLL ,T CATCH ,T THROW ,T WITHIN
,T BEGIN   ,T WHILE      ,T REPEAT ,T UNTIL   ,T AGAIN   ,T ?DO   ,T LOOP  ,T I
,T J       ,T K          ,T +DO    ,T U+DO    ,T -DO     ,T U-DO  ,T DO    ,T +LOOP
,T -LOOP   ,T UNLOOP     ,T LEAVE  ,T ?LEAVE  ,T EXIT    ,T DONE  ,T FOR   ,T NEXT
,T RECURSE ,T ;
$STRING CATEGORY !
,T S"      ,T ."         ,T ABORT"
$COMMENT CATEGORY !
TOKENS S" (" CATEGORY @ ITEM,
$LCOMMENT CATEGORY !
TOKENS S" \" CATEGORY @ ITEM,

\ aggregates r g b 
: RGB ( r g b -- v )
    SWAP  8 LSHIFT OR
    SWAP 16 LSHIFT OR ;
\ aggregates r g b and font weight in one value
: RGBW-VALUE ( r g b ? -- )
    1 AND  24 LSHIFT 
    >R RGB R> OR ;

HEX
\ extract the rgb color from an attribute value
: RGBW>COLOR ( n -- rgb )
    FFFFFF AND ;
DECIMAL

\ extract the font-weight from an attribute value
: RGBW>FONT-WEIGHT ( n -- ? )
    24 RSHIFT 1 AND ;

CREATE ATTRIBUTES 10 CELLS ALLOT
    DOES> SWAP CELLS + ;

VARIABLE DEFINING

DEFINING OFF

\ display a span element with attributes
: SPAN. ( addr # attr -- )
    DUP  RGBW>COLOR
    SWAP RGBW>FONT-WEIGHT
    <SPAN>. _TYPE </SPAN>. ;

\ convert a char to uppercase
: C>UPPER ( c -- c )
    DUP [CHAR] a [CHAR] { WITHIN IF 32 - THEN ;

\ copy and convert a string to uppercase
: >UPPER ( addr # -- addr # )
    SWAP OVER 0 DO
        I OVER + C@ C>UPPER PAD I + C!
    LOOP DROP PAD SWAP ;

\ display a forth token
: (TOKEN.) ( addr # -- )
    2DUP
    TOKENS -ROT >UPPER FIND-ITEM ?DUP IF
        ITEM>VALUE
        DUP $DEFINING = DEFINING !
    ELSE
        2DUP FIND-NAME IF $DEFAULT ELSE $NUMBER THEN
    THEN
    ATTRIBUTES @ SPAN. ;

\ add a new forth word in the list
: ADD-TOKEN ( addr # -- )
    TOKENS -ROT $USERDEF ITEM, ;

\ display a token possibly adding it first
: TOKEN. ( addr # -- )
    DEFINING @ IF
        2DUP ADD-TOKEN
        DEFINING OFF
    THEN (TOKEN.) ;

\ get the next token on the input stream
\ return the length of token and firt non token char
: TOKEN<. ( -- # c )
    TOKEN %TOKEN ERASE
    0 SKIP-SPACE.
    BEGIN
        DUP EOF <> 
        OVER IS-SPACE? 0= AND WHILE
        OVER TOKEN + C!
        1+ _KEY
    REPEAT ;

VARIABLE COLOR
VARIABLE BACKGROUND

\ display forth source code with syntax highlighting
: SOURCE.
    COLOR @ BACKGROUND @ <PRE>.
    DEFINING OFF
    BEGIN
        TOKEN<. OVER WHILE
        SWAP TOKEN SWAP 
        TOKEN.
        _?EMIT
    REPEAT 
    2DROP
    </PRE>. ;

[IFUNDEF] TESTING
[THEN]
