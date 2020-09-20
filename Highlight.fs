\ HighLight.fs provide syntax highlighting for forth programs

VARIABLE HTML
HTML OFF

DEFER _EMIT
DEFER _TYPE
DEFER _KEY
DEFER _COLOR

' EMIT IS _EMIT
' TYPE IS _TYPE
' KEY  IS _KEY


CREATE TOKEN 1024 ALLOT

: ANSI-NORMAL
    27 _EMIT
    [CHAR] [ _EMIT
    [CHAR] 0 _EMIT
    [CHAR] m _EMIT ;

: ANSI-COLOR ( c -- )
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
    TOKEN 1024 ERASE
    0 .SKIP-SPACE
    BEGIN
        DUP 4 <>
        OVER IS-SPACE? 0= AND WHILE
        OVER TOKEN + C!
        1+ _KEY
    REPEAT ;

: ?_EMIT ( c -- )
    DUP 4 <> IF _EMIT ELSE DROP THEN ;

: .SKIP-LINE
    BEGIN
        _KEY DUP   4 <>
             OVER 10 <> AND
             OVER 13 <> AND WHILE
        _EMIT
    REPEAT ?_EMIT ;

: .SKIP-COMMENT
    BEGIN
        _KEY DUP   4 <>
             OVER [CHAR] ) <> AND WHILE
        _EMIT
    REPEAT ?_EMIT ;

: .SKIP-STRING
    BEGIN
        _KEY DUP   4 <>
             OVER [CHAR] " <> AND WHILE
        _EMIT
    REPEAT ?_EMIT ;

: LINKED-LIST \ name --
    CREATE 0 , ;

: LINK, ( list addr # n -- )
    >R ROT HERE              \ addr # list new@
    OVER @ ,                 \ addr # list new@   | link is compiled
    R>     ,                 \ addr # list new@   | n is compiled
    SWAP !                   \ addr #             | new link is saved
    S, ;

: LINK>LINK ( list -- link )
    @ ;

: LINK>NAME ( link -- addr )
    CELL+ CELL+ ;

: LINK>VALUE ( link -- n )
    CELL+ @ ;

: FIND-LINK ( list addr # -- link|0 )
    2>R LINK>LINK
    BEGIN
        DUP ?DUP IF
            LINK>NAME COUNT
            2R@ COMPARE
        ELSE 0 THEN WHILE
        LINK>LINK
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

LINKED-LIST TOKENS
VARIABLE CATEGORY

: ,T \ cccccc/bl
    TOKENS BL PARSE CATEGORY @ LINK, ;
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
,T RECURSE
$STRING CATEGORY !
,T S"      ,T ."         ,T ABORT"
$COMMENT CATEGORY !
TOKENS S" (" CATEGORY @ LINK,
$LCOMMENT CATEGORY !
TOKENS S" \" CATEGORY @ LINK,

CREATE COLORS
2 C, \ $NUMBER
2 C, \ $STRING
5 C, \ $COMMENT
5 C, \ $LCOMMENT
6 C, \ $USERDEF
7 C, \ $DEFINING
4 C, \ $OPERATOR
1 C, \ $CONTROL
3 C, \ $STACK
3 C, \ $MEMORY

HEX
CREATE RGB-COLORS
008000 , \ $NUMBER
800000 , \ $STRING
800080 , \ $COMMENT
800080 , \ $LCOMMENT
008080 , \ $USERDEF
4682B4 , \ $DEFINING
0000FF , \ $OPERATOR
FF8C00 , \ $CONTROL
66CDAA , \ $STACK
808000 , \ $MEMORY
DECIMAL

: SET-RGB-COLOR ( col cat -- )
    CELLS RGB-COLORS + ! ;

: CATEGORY>COLOR ( n -- n )
    HTML @ IF
        CELLS RGB-COLORS + @
    ELSE
        COLORS + C@ 
    THEN ;

VARIABLE PRE-COLOR
VARIABLE PRE-BACKGROUND
VARIABLE BOLD 

HEX 
000000 PRE-COLOR !
FFFFFF PRE-BACKGROUND !
BOLD ON

: .HEXDIGIT ( n -- )
    DUP 0A < IF [CHAR] 0 ELSE 0A - [CHAR] a THEN 
    + _EMIT ;

: .HEXBYTE ( c -- )
    DUP 04 RSHIFT .HEXDIGIT
        0F AND   .HEXDIGIT ;

: .RGB-COLOR ( n -- )
    [CHAR] # _EMIT
    DUP 10 RSHIFT 0000FF AND .HEXBYTE
    DUP 08 RSHIFT 0000FF AND .HEXBYTE
                  0000FF AND .HEXBYTE ;
DECIMAL

: .<PRE>
    S\" <pre style=\"color:" _TYPE PRE-COLOR @ .RGB-COLOR
    S\" ;background:" _TYPE PRE-BACKGROUND @ .RGB-COLOR
    S\" ;\">" _TYPE ;
\ "
: .<SPAN> ( col ? -- )
    S\" <span style=\"color:" _TYPE SWAP .RGB-COLOR 
    S\" ;" _TYPE
    IF S\"  font-weight:bold;" _TYPE THEN
    S\" \">" _TYPE ;
\ "

: HTML-COLOR ( col -- )
    BOLD @ .<SPAN> ;

VARIABLE STRING
VARIABLE COMMENT
VARIABLE LINE-COMMENT
VARIABLE DEFINING

: DEFINE-COLOR
    HTML @ IF
        ['] HTML-COLOR IS _COLOR
    ELSE
        ['] ANSI-COLOR IS _COLOR
    THEN ;

DEFINE-COLOR

: TOKEN. 
    HTML @ IF
        S" </span>" _TYPE
    THEN ;

: NORMAL
    HTML @ 0= IF ANSI-NORMAL THEN ;

: SOURCE-BEGIN
    HTML @ IF .<PRE> THEN ;

: SOURCE-END
    HTML @ IF S" </pre>" _TYPE THEN ;

: .TOKEN ( addr # -- )
    2DUP TOKENS -ROT FIND-LINK ?DUP IF
        LINK>VALUE 
        DUP $STRING = STRING !
        DUP $COMMENT = COMMENT !
        DUP $LCOMMENT = LINE-COMMENT !
        DUP $DEFINING = DEFINING !
        CATEGORY>COLOR _COLOR
    ELSE
        DEFINING @ IF
            2DUP TOKENS -ROT $USERDEF LINK,
            $DEFINING CATEGORY>COLOR _COLOR
            DEFINING OFF
        ELSE
            2DUP FIND-NAME IF
                NORMAL
            ELSE
                $NUMBER CATEGORY>COLOR _COLOR
            THEN
        THEN
    THEN
    _TYPE 
    TOKEN. ;

LINKED-LIST CATEGORIES
CATEGORIES S" NUMBER"   0 LINK,
CATEGORIES S" STRING"   1 LINK,
CATEGORIES S" COMMENT"  2 LINK,
CATEGORIES S" LCOMMENT" 3 LINK,
CATEGORIES S" USERDEF"  4 LINK,
CATEGORIES S" DEFINING" 5 LINK,
CATEGORIES S" OPERATOR" 6 LINK,
CATEGORIES S" CONTROL"  7 LINK,
CATEGORIES S" STACK"    8 LINK,
CATEGORIES S" MEMORY"   9 LINK,

VARIABLE _BASE
: >HEX-NUMBER ( addr # -- u )
    BASE @ _BASE ! HEX
    0 S>D 2SWAP >NUMBER 2DROP D>S
    _BASE @ BASE ! ;


: READ-ARG ( addr # -- )
    2DUP S" HTML" COMPARE 0= IF 
        TRUE HTML ! 
        DEFINE-COLOR 
    ELSE
        2DROP
    THEN ;

: READ-ARGS
    ARGC @ 0 ?DO
        I ARG READ-ARG
    LOOP ;

: .SOURCE
    READ-ARGS
    SOURCE-BEGIN
    STRING OFF
    COMMENT OFF
    LINE-COMMENT OFF
    DEFINING OFF
    BEGIN
        STRING @ IF .SKIP-STRING STRING OFF THEN
        COMMENT @ IF .SKIP-COMMENT COMMENT OFF THEN
        LINE-COMMENT @ IF .SKIP-LINE LINE-COMMENT OFF THEN
        .>TOKEN
        SWAP DUP WHILE
        TOKEN SWAP .TOKEN
        ?_EMIT
    REPEAT 2DROP 
    SOURCE-END ;

[IFUNDEF] TESTING .SOURCE BYE [THEN]
