
    read the input, printing all spaces, tabs and CR
    collect the next word
        if it's a new user-defined word:
            insert the word in the list with category user-defined
        if it's a (
            switch to comment attributes until a ) is found
        if it's a \ 
            switch to comment attributes until CR
        if it's S" or ." or ABORT"
            switch to string attributes until "
        if it's : or CREATE or VARIABLE or CONSTANT or 2VARIABLE or 2CONSTANT:
            emit the attributes for defining words
            the next word has to enter the list with category user-defined words
        look up the word in the word list
        if found:
            fetch its category
            emit the attributes matching that category
            print the word
        if not found:
            emit the attributes for numbers

