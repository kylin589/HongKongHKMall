using System;
using Couchbase;
using Enyim.Caching.Memcached;
using Newtonsoft.Json;
//using Couchbase;
//using Enyim.Caching;
//using Enyim.Caching.Memcached;

namespace HKTHMall.Core
{
    //缓存工厂
    public class MemCacheFactory : ICacheManager   //继承缓存的接口
    {
        private static readonly object SyncRoot = new object();    //程序运行时创建一个静态的只读对象(用于下面的加锁)

        #region 缓存工厂的基础属性字段,静态构造方法
        private static readonly CouchbaseClient _instance;

        //静态构造函数,在类初始化的时候执行,不用加 public / private 没有意义,因为这个是由.net自动来调用
        //在创建第一个类实例或任何静态成员被引用时,.NET将自动调用静态构造函数来初始化类
        static MemCacheFactory()
        {
            //_instance = new CouchbaseClient();
            _instance = new CouchbaseClient("couchbase/couchbase_Cache");
        }

        private static CouchbaseClient Instance
        {
            get { return _instance; }
        }

        #endregion

        #region CRUD 接口的实现

        #region AddCache 添加缓存(以序列化保存)
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddCache(string key, object obj)
        {
            //注意:如果我们直接是用obj来保存,则缓存会帮我们自动加密
            //如果我们按照下面的方法,先序列化后,再保存,那么就不会加密
            string jsonobj = JsonConvert.SerializeObject(obj);
            return Instance.Store(StoreMode.Set, key, jsonobj);
        }

        #endregion

        #region AddCache 添加缓存并设置时间(以序列化保存)
        /// <summary>
        /// AddCache 添加缓存并设置时间(以序列化保存)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public bool AddCache(string key, object obj, int minutes)
        {
            string jsonobj = JsonConvert.SerializeObject(obj);
            return Instance.Store(StoreMode.Set, key, jsonobj, DateTime.Now.AddMinutes(minutes));
        }

        /// <summary>
        /// AddCache 添加缓存并设置时间(以序列化保存)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="cacheDate"></param>
        /// <returns></returns>
        public bool AddCache(string key, object obj, DateTime cacheDate)
        {
           string jsonobj = JsonConvert.SerializeObject(obj);
            return Instance.Store(StoreMode.Set, key, obj, cacheDate);
        }
        public bool AddCache<T>(string key, T value, int minutes)
        {
            string serializeStr = JsonConvert.SerializeObject(value);//转码
            return Instance.Store(StoreMode.Set, key, serializeStr, DateTime.Now.AddMinutes(minutes));
        }

        #endregion

        #region GetCache 获得缓存(并且是已经反序列化的)

        ///// <summary>
        ///// 获得缓存
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public T GetCache<T>(string key) where T : class
        //{
        //    string jsonobj = Instance.Get<String>(key);
        //    if (string.IsNullOrEmpty(jsonobj))
        //        return null;
        //    return (T)JsonConvert.DeserializeObject(jsonobj, typeof(T));
        //    //T jsonobj = Instance.Get<T>(key);
        //    ////if (string.IsNullOrEmpty(jsonobj))
        //    ////    return null;
        //    //return jsonobj;//(T)JsonConvert.DeserializeObject(jsonobj, typeof(T));

        //}
        /// <summary>
        /// 获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key) where T : class
        {
            string jsonobj = Instance.Get<String>(key);
            if (string.IsNullOrEmpty(jsonobj))
                return null;
            return (T)JsonConvert.DeserializeObject(jsonobj, typeof(T));

        }
        /// <summary>
        /// 获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key) 
        {
            return Instance.Get(key);

        }

        #endregion

        #region ClearCache 清除缓存
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ClearCache(string key)
        {
            return Instance.Remove(key);
        }

        #endregion

        #region FlushAll 设置缓存过期(失效后通过get就取不出来了)
        /// <summary>
        /// 设置缓存过期(失效后通过get就取不出来了)
        /// </summary>
        public void FlushAll()
        {
            //Instance.FlushAll();
            Couchbase.Management.CouchbaseCluster cluster   = new Couchbase.Management.CouchbaseCluster("couchbase");
            cluster.FlushBucket("default");
        }

        #endregion

        #endregion

        //private  ulong defaultValue =
        //    Convert.ToUInt64(DateTime.UtcNow.Subtract(new DateTime(2014, 1, 1,0,0,0,0)).TotalMilliseconds.ToString("0"));
        //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    return Convert.ToInt64(ts.TotalSeconds).ToString();
        private ulong defaultValue = Convert.ToUInt64((DateTime.UtcNow - new DateTime(2015, 6, 26, 0, 0, 0, 0)).TotalMilliseconds);
        /// <summary>
        /// 自增ID
        /// </summary>
        /// <param name="key">NOSQL的KEY</param>
        /// <param name="delta">自增数量</param>
        /// <param name="expiresAt">缓存多久时间</param>
        /// <returns></returns>
        public long Increment(string key, ulong delta, DateTime expiresAt)
        {
  
            return (long)Instance.Increment(key, defaultValue, delta, expiresAt);
        }
        /// <summary>
        /// 公用KEY　commonId
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Increment(string key)
        {
            return (long)Instance.Increment(key, defaultValue,1);
        }
        /// <summary>
        /// 自增ID(默认自增1)
        /// </summary>
        /// <param name="key">NOSQL的KEY</param>
        /// <param name="expiresAt">缓存多久</param>
        /// <returns></returns>
        public long Increment(string key, DateTime expiresAt)
        {
            return Increment(key,1,expiresAt);
        }

        //public IEnumerable<T> GetAll(int limit = 0) where T : class
        //{
        //    var view = Instance.GetView<T>(_designDoc, "all", true);
        //    if (limit > 0) view.Limit(limit);
        //    return view;
        //}


        #region 工厂的单例
        private static MemCacheFactory _shareInstance;
        public static MemCacheFactory GetCurrentMemCache()
        {
            if (_shareInstance == null)
                lock (SyncRoot)
                {
                    _shareInstance = new MemCacheFactory();
                }
            return _shareInstance;
        }

        #endregion

    }
}
