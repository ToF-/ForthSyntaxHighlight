\ Bowling.fs          gforth Bowling.fs <input.dat
VARIABLE SCORE
VARIABLE BONUS
VARIABLE FRAME#
VARIABLE FRAME

: START
    0 SCORE !
    0 BONUS !
    0 FRAME# !
    0 FRAME ! ;

: SPARE \ set the bonus to 1, no supplement
    1 BONUS ! ;

: STRIKE \ increase bonus, and set supplement to 1
    BONUS @ 1+ 4 OR BONUS ! ;

: BONUS> ( -- n ) \ produce bonus factor, get supplement ready
    BONUS @ DUP 3 AND
    SWAP 2/ 2/ BONUS ! ;

: COLLECT-BONUS ( #pins -- ) \ add extra points to score and advance bonus
    BONUS> * SCORE +! ;

: FRAME#> \ advance frame count
    FRAME# @ 1+ 10 MIN FRAME# ! ;

: OPEN-FRAME? ( -- ? )
    FRAME @ ;

: CLOSE-FRAME \ close frame and increment frame count
    0 FRAME ! FRAME#> ;

: NEW-FRAME? ( -- ? )
    OPEN-FRAME? 0= ;

: OPEN-FRAME ( #pins -- ) \ open frame, saving 1st roll value+1
    1+ FRAME ! ;

: LAST-ROLL ( -- #pins ) \ retrieve the first roll value
    FRAME @ 1- ;

: CHECK-STRIKE ( #pins -- )
    DUP 10 = IF
        DROP STRIKE CLOSE-FRAME
    ELSE
        OPEN-FRAME
    THEN ;

: CHECK-SPARE ( #pins -- )
    LAST-ROLL + 10 = IF SPARE THEN
    CLOSE-FRAME ;

: CHECK-BONUS ( #pins -- )
    NEW-FRAME? IF CHECK-STRIKE ELSE CHECK-SPARE THEN ;

: ROLL+ ( #pins -- )
    DUP COLLECT-BONUS
    FRAME# @ 0 10 WITHIN IF
        DUP SCORE +!
        CHECK-BONUS
    ELSE
        DROP
    THEN ;

: SKIP-NON-DIGIT ( -- n )
    BEGIN KEY DIGIT? 0= WHILE REPEAT ;

: GET-NUMBER ( -- n )
    0 SKIP-NON-DIGIT
    BEGIN
        SWAP 10 * +
        KEY DIGIT?
    0= UNTIL ;

: BOWLING
    GET-NUMBER 0 DO
        START
        GET-NUMBER 0 DO
            GET-NUMBER ROLL+
        LOOP
        SCORE ? CR
    LOOP ;

