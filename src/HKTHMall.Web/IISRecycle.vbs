set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Portal_Test") 
appPool.Recycle

set appPoolDev = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Portal") 
appPoolDev.Recycle
