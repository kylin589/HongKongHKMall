set appPool = GetObject("IIS://LocalHost/w3svc/AppPools/HKTH_test") 
appPool.Recycle
set appPool1 = GetObject("IIS://LocalHost/w3svc/AppPools/HKTH_test") 
appPool1.Recycle
