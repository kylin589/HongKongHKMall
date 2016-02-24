set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Portal_Test") 
appPool.Start

set appPoolDev = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Portal") 
appPoolDev.Start
