
tryCatch({

    png(filename = "iris.png", width = 450, height = 300, units = "px")

    plot(iris$Sepal.Width ~ iris$Petal.Width, col = iris$Species, las = 1)
    dev.off()
},
error = function(e) {

    # Print error message to some log file
    error <- conditionMessage(e)
    #conn <- file("D:/Temp/log.txt")
    #writeLines(error, conn)
    #close(conn)
})