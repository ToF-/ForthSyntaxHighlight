#! /usr/local/bin/gforth

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

: to-string ( n -- addr c )  s>d <# # # # # # # #> ;

: HEX6 ( n -- addr # )
    BASE @ >R HEX
    s>d <# # # # # # # #>
    R> BASE ! ;

\ display a span tag with color and font-weight
: .<SPAN> ( rgb bold? -- )
    S\" <span style=\"color:#" _TYPE
    SWAP HEX6 _TYPE
    S\" ; font-weight:" _TYPE
    IF S\" bold" ELSE S\" normal" THEN _TYPE
    S\" ;\">" _TYPE ;
\ "
\ display a end of span tag
: .</SPAN> 
    S\" </span>" _TYPE ;

[IFUNDEF] TESTING
[THEN]
