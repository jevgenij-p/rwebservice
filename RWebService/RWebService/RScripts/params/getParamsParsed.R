
tryCatch({

    library("optparse")

    option_list = list(
      make_option(c("--num"), type="integer", default=0),
      make_option(c("--txt"), type="character")
    ); 

    opt_parser = OptionParser(option_list=option_list);
    opt = parse_args(opt_parser);

    print(paste0('num = ', opt$num))
    print(paste0('txt = ', opt$txt))
},
error = function(e) {

    # Print error message to some log file
    error <- conditionMessage(e)
    #conn <- file("D:/Temp/log.txt")
    #writeLines(error, conn)
    #close(conn)
})