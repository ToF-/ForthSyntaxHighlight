\ HighLight.fs provide syntax highlighting for forth programs

DEFER _EMIT
DEFER _TYPE
DEFER _KEY

' EMIT IS _EMIT
' TYPE IS _TYPE
' KEY  IS _KEY

CREATE TOKEN 1024 ALLOT

: .NORMAL
    27 _EMIT
    [CHAR] [ _EMIT
    [CHAR] 0 _EMIT
    [CHAR] m _EMIT ;

: .COLOR ( c -- )
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

 0 CONSTANT NAME-CATEGORY
 1 CONSTANT NUMBER-CATEGORY
 2 CONSTANT STRING-CATEGORY
 3 CONSTANT COMMENT-CATEGORY
 4 CONSTANT LINE-COMMENT-CATEGORY
 5 CONSTANT NEW-WORD-CATEGORY
 6 CONSTANT DEFINING-CATEGORY
 7 CONSTANT OPERATOR-CATEGORY
 8 CONSTANT CONTROL-CATEGORY
 9 CONSTANT STACK-CATEGORY
10 CONSTANT ADDRESS-CATEGORY

LINKED-LIST TOKENS
VARIABLE CATEGORY

: ,T \ cccccc/bl
    TOKENS BL PARSE CATEGORY @ LINK, ;
STACK-CATEGORY CATEGORY !
,T DROP  ,T NIP   ,T DUP   ,T OVER   ,T TUCK  ,T SWAP  ,T ROT    ,T -ROT
,T ?DUP  ,T PICK  ,T ROLL  ,T 2DROP  ,T 2NIP  ,T 2DUP  ,T 2OVER  ,T 2TUCK
,T 2SWAP ,T 2ROT  ,T 2-ROT ,T 3DUP   ,T 4DUP  ,T 5DUP  ,T 3DROP  ,T 4DROP
,T 5DROP ,T 8DROP ,T 4SWAP ,T 4ROT   ,T 4-ROT ,T 4TUCK ,T 8SWAP  ,T 8DUP
,T >R    ,T R>    ,T R@    ,T RDROP  ,T 2>R   ,T 2R>   ,T 2R@    ,T 2RDROP
,T 4>R   ,T 4R>   ,T 4R@   ,T 4RDROP ,T FDROP ,T FNIP  ,T FDUP   ,T FOVER
,T FTUCK ,T FSWAP ,T FROT  ,T SP@    ,T SP!   ,T FP@   ,T FP!    ,T RP@
,T RP!   ,T LP@   ,T LP!   ,T DEPTH
OPERATOR-CATEGORY CATEGORY !
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
ADDRESS-CATEGORY CATEGORY !
,T @         ,T !        ,T +!         ,T C@      ,T C!        ,T 2@                ,T 2!      ,T F@ 
,T F!        ,T SF@      ,T SF!        ,T DF@     ,T DF!       ,T CHARS             ,T CHAR+   ,T CELLS 
,T CELL+     ,T CELL     ,T ALIGN      ,T ALIGNED ,T FLOATS    ,T FLOAT+            ,T FLOAT   ,T FALIGN 
,T FALIGNED  ,T SFLOATS  ,T SFLOAT+    ,T SFALIGN ,T SFALIGNED ,T DFLOATS           ,T DFLOAT+ ,T DFALIGN 
,T DFALIGNED ,T MAXALIGN ,T MAXALIGNED ,T CFALIGN ,T CFALIGNED ,T ADDRESS-UNIT-BITS ,T ALLOT   ,T ALLOCATE 
,T HERE      ,T MOVE     ,T ERASE      ,T CMOVE   ,T CMOVE>    ,T FILL              ,T BLANK   ,T UNUSED
DEFINING-CATEGORY CATEGORY !
,T :         ,T CREATE   ,T VARIABLE   ,T CONSTANT ,T 2VARIABLE ,T 2CONSTANT ,T DEFER
CONTROL-CATEGORY CATEGORY !
,T IF      ,T ELSE       ,T ENDIF  ,T THEN    ,T CASE    ,T OF    ,T ENDOF ,T ENDCASE 
,T ?DUP-IF ,T ?DUP-0=-IF ,T AHEAD  ,T CS-PICK ,T CS-ROLL ,T CATCH ,T THROW ,T WITHIN 
,T BEGIN   ,T WHILE      ,T REPEAT ,T UNTIL   ,T AGAIN   ,T ?DO   ,T LOOP  ,T I 
,T J       ,T K          ,T +DO    ,T U+DO    ,T -DO     ,T U-DO  ,T DO    ,T +LOOP 
,T -LOOP   ,T UNLOOP     ,T LEAVE  ,T ?LEAVE  ,T EXIT    ,T DONE  ,T FOR   ,T NEXT 
,T RECURSE
STRING-CATEGORY CATEGORY !
,T S"      ,T ."         ,T ABORT"
COMMENT-CATEGORY CATEGORY !
TOKENS S" (" CATEGORY @ LINK,
LINE-COMMENT-CATEGORY CATEGORY !
TOKENS S" \" CATEGORY @ LINK,

CREATE COLORS

3 C, \ NAME-CATEGORY
2 C, \ NUMBER-CATEGORY
2 C, \ STRING-CATEGORY
5 C, \ COMMENT-CATEGORY
5 C, \ LINE-COMMENT-CATEGORY
6 C, \ NEW-WORD-CATEGORY
7 C, \ DEFINING-CATEGORY
4 C, \ OPERATOR-CATEGORY
1 C, \ CONTROL-CATEGORY
3 C, \ STACK-CATEGORY
3 C, \ ADDRESS-CATEGORY

: CATEGORY>COLOR ( n -- n )
    COLORS + C@ ;

VARIABLE STRING
VARIABLE COMMENT
VARIABLE LINE-COMMENT
VARIABLE DEFINING

: .TOKEN ( addr # -- )
    2DUP TOKENS -ROT FIND-LINK ?DUP IF
        LINK>VALUE 
        DUP STRING-CATEGORY = STRING !
        DUP COMMENT-CATEGORY = COMMENT !
        DUP LINE-COMMENT-CATEGORY = LINE-COMMENT !
        DUP DEFINING-CATEGORY = DEFINING !
        CATEGORY>COLOR .COLOR
    ELSE
        DEFINING @ IF
            2DUP TOKENS -ROT NEW-WORD-CATEGORY LINK,
            DEFINING-CATEGORY CATEGORY>COLOR .COLOR
            DEFINING OFF
        ELSE
            2DUP FIND-NAME IF
                .NORMAL
            ELSE
                NUMBER-CATEGORY CATEGORY>COLOR .COLOR
            THEN
        THEN
    THEN
    _TYPE ;

: .SOURCE
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
    REPEAT 2DROP ;

[IFUNDEF] TESTING .SOURCE BYE [THEN]
