using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace X.Common
{
    /// <summary>
    /// 缓存类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 缓存前缀
        /// </summary>
        public const string CacheNamePrefix = "CacheNamePrefix";
        private static System.Web.Caching.Cache ObjCache = System.Web.HttpRuntime.Cache;
        /// <summary>
        /// 移除缓存项的文件
        /// </summary>
        /// <param name="key">缓存Key</param>
        public static void Remove(string key)
        {
            key = ConfigHelper.AppSettings(CacheNamePrefix) + key;
            ObjCache.Remove(key);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll(string key = null)
        {
            key = ConfigHelper.AppSettings(CacheNamePrefix) + key;
            IDictionaryEnumerator CacheEnum = ObjCache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                if (key.IsNull() || CacheEnum.Key.ToString().StartsWith(key))
                {
                    Remove(CacheEnum.Key.ToString());
                }
            }
        }
        /// <summary>
        /// 创建缓存项的文件依赖
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="fileName">文件绝对路径</param>
        public static void Insert(string key, object obj, string fileName)
        {
            key = ConfigHelper.AppSettings(CacheNamePrefix) + key;
            //创建缓存依赖项
            CacheDependency dep = new CacheDependency(fileName);
            //创建缓存
            ObjCache.Insert(key, obj, dep);
        }

        /// <summary>
        /// 创建缓存项过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void Insert(string key, object obj, int expires = 10)
        {
            key = ConfigHelper.AppSettings(CacheNamePrefix) + key;
            ObjCache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            key = ConfigHelper.AppSettings(CacheNamePrefix) + key;
            return ObjCache.Get(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

    }
}
