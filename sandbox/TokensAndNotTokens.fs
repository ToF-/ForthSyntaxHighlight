
: IS-SPACE? ( c -- )
    DUP  32 =
    OVER 13 = OR
    SWAP 10 = OR ;

: SKIP-SPACE ( -- c )
    BEGIN
        KEY DUP IS-SPACE? WHILE
        DROP
    REPEAT ;

: IS-TOKEN-MATERIAL? ( c -- )
    33 256 WITHIN ;

: GET-TOKEN ( -- len )
    0
    SKIP-SPACE
    BEGIN
        DUP IS-TOKEN-MATERIAL? WHILE
        OVER PAD + C!
        1+
        KEY
    REPEAT DROP ;

: NAME-COLOR
    ESC[ ." 34m" ;
    
: NUMBER-COLOR
    ESC[ ." 32m" ;

: .TOKENS&NOT-TOKENS
    BEGIN
        KEY? 0= IF EXIT THEN
        GET-TOKEN
        DUP ?DUP IF 
            PAD OVER FIND-NAME IF NAME-COLOR ELSE NUMBER-COLOR THEN
            PAD SWAP TYPE CR
        THEN
    0= UNTIL ;

.TOKENS&NOT-TOKENS
BYE
