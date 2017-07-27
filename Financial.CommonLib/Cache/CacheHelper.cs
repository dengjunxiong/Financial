using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Financial.CommonLib.Cache
{
    /// <summary>
    /// Cache
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Add(string key, object value)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert(key, value);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="validTime">有效期</param>
        public static void Add(string key, object value, TimeSpan validTime)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Insert(key, value, null, DateTime.UtcNow.Add(validTime), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object Get(string key)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            return cache[key];
        }

        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Remove(key);
        }
        
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}