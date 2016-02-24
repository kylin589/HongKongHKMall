set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Admin_Test") 
appPool.Stop

set appPoolDev = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Admin") 
appPoolDev.Stop