#! /usr/local/bin/gforth

 4 CONSTANT EOF
 9 CONSTANT TAB
10 CONSTANT NL
13 CONSTANT CR%

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

[IFUNDEF] TESTING
[THEN]
