
args = commandArgs(trailingOnly=TRUE)

if (length(args) == 0) {
  
  print('At least one argument must be supplied');
  
} else {

  for(i in 1:length(args)) {
    print(args[i])
  }
}