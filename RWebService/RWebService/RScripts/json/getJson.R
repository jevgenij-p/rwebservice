
tryCatch({

    library("jsonlite")

    cars <- head(mtcars, n=3)

    json <- toJSON(cars)

    # Write result to the file
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