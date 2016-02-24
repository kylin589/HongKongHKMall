namespace HKTHMall.Core
{
    public interface ICacheManager
    {
        bool AddCache(string key, object obj);
        T GetCache<T>(string key) where T : class;
        bool ClearCache(string key);
        bool AddCache(string key, object obj, int minutes);
        void FlushAll();
    }
}
