set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Portal_Test") 
appPool.Stop

set appPoolDev = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Portal") 
appPoolDev.Stop