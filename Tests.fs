INCLUDE ffl/tst.fs
INCLUDE HighLight.fs

CREATE _OUTPUT 1024 ALLOT
CREATE _INPUT  1024 ALLOT
VARIABLE _OUTPUT#
VARIABLE _INPUT# 
VARIABLE _#INPUT

: EMIT>! ( c -- )
    _OUTPUT# @
    _OUTPUT + C!
    1 _OUTPUT# +! ;

: TYPE>! ( addr # -- )
    0 ?DO DUP I + C@ EMIT>! LOOP DROP ;

: KEY<@ ( -- c )
    _INPUT# @ _#INPUT @ < IF 
        _INPUT _INPUT# @ + C@ 
        1 _INPUT# +!
    ELSE
        4
    THEN ;

T{ ." _EMIT can be redirected for the sake of testing" cr
   ' EMIT>! IS _EMIT
   _OUTPUT# OFF
   42 _EMIT
   46 _EMIT
   44 _EMIT
   _OUTPUT# @ 3 ?S
   S" *.," _OUTPUT _OUTPUT# @ COMPARE 0 ?S
}T

T{ ." _TYPE can be redirected for the sake of testing" cr
    ' TYPE>! IS _TYPE
    _OUTPUT# OFF
    S" Hello World!" _TYPE
    S" Hello World!" _OUTPUT _OUTPUT# @ COMPARE 0 ?S
}T

T{ ." _KEY can be redirected for the sake of testing" cr
    ' KEY<@ IS _KEY
    _INPUT# OFF
    S" *.," _INPUT SWAP CMOVE
    3 _#INPUT !
    _KEY 42 ?S
    _KEY 46 ?S
    _KEY 44 ?S
    _KEY 4 ?S
}T

bye
