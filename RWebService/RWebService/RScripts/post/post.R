
tryCatch({

    library("jsonlite")

    conn <- file("input.json")
    data <- fromJSON(conn)

    json <- toJSON(data)

    # Write json back to file
    conn <- file("output.json")
    writeLines(json, conn)
    close(conn)
},
error = function(e) {

    # Print error message to some log file
    error <- conditionMessage(e)
    #conn <- file("D:/Temp/log.txt")
    #writeLines(error, conn)
    #close(conn)
})