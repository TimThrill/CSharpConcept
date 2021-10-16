# Summary
The intent of this project is to compare the performance of synchronous and asynchronous operations under different scenarios in .NET applications.

# Scenarios
## CPU intensive
In this case, the sync/async operation is a CPU intensive task. I use the divide operation here.

## Network intensive
In this case, the cost is mostly on the network side. In this example, it simply loads the home page of `https://www.google.com` many times.

## Disk IO intensive
In this case, the operation is to write/read data from harddisk.