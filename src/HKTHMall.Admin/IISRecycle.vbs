set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Admin_Test") 
appPool.Recycle

set appPoolDev = GetObject("IIS://LocalHost/w3svc/AppPools/HKTHMall_Admin") 
appPoolDev.Recycle
