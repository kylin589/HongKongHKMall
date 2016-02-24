set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTH_test") 
appPool.Stop
set appPool1 = GetObject("IIS://LocalHost/w3svc/AppPools/HKTH_test") 
appPool1.Stop
