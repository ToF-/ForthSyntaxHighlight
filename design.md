

to start html code snippet with syntax highlighting:
<pre style="color:#000020;background:#f6f8ff;">
to output of a word:
<span style="color:#200080; font-weight:bold; ">word</span>

have constants for categories (like an enum)
have a category/color table (array with color code for 
to end html code snippet
</pre>


    modes:
    space ( the char is not a printable char) : 
        if we are collecting a word, 
        print the char, loop
    collecting a word
    comment ( : we are looking for ) 
    comment \ : we are looking for EOL
    string ." S" ABORT" we are looking for "
    :  the next word is a new colon definition
    VARIABLE CONSTANT 2VARIABLE 2CONSTANT CREATE : the next word is a new word
    normal mode:
        read the token
        if new colon definition : insert into the linked list with category colon definition
        if new word             : insert into the linked list with category new word
        if the token is in the linked list
            set the color of its category
        if it is not in the linked list, it's a number
            set the color of numbers
        print the token

