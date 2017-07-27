using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime;

namespace Financial.CommonLib.Cache
{
    /// <summary>
    /// MemoryCache
    /// </summary>
    public class MemoryCacheHelper
    {
        static System.Runtime.Caching.MemoryCache cache = null;

        static MemoryCacheHelper()
        {
            cache = System.Runtime.Caching.MemoryCache.Default;
        }

        /// <summary>
        /// 设置缓存(存在则更新;不存在则添加)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="validTime">有效期(分钟,默认30分钟)</param>
        public static void Set(string key, object value, double validTime)
        {
            //过期策略
            System.Runtime.Caching.CacheItemPolicy policy = new System.Runtime.Caching.CacheItemPolicy();

            if (validTime <= 0)
            {
                validTime = 30;
            }
            //缓存有效期
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(validTime);
            //离最后一次访问超过设定时间后过期
            //policy.SlidingExpiration = TimeSpan.FromMinutes(validTime);

            if (!cache.Contains(key))//缓存项不存在
            {
                cache.Add(key, value, policy);
                return;
            }
            cache.Set(key, value, policy);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static object Get(string key)
        {
            if (!cache.Contains(key))//缓存项不存在
            {
                return null;
            }
            return cache[key];
        }

        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            if (cache.Contains(key))
            {
                cache.Remove(key);
            }
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            for (int i = 0; i < cache.Count(); i++)
            {
                cache.Remove(cache.ElementAt(i).Key);
            }
        }

        /// <summary>
        /// 移除固定前缀键值的缓存
        /// </summary>
        /// <param name="headerStr">键值前缀</param>
        public static void RemoveSome(string headerStr)
        {
            for (int i = 0; i < cache.Count(); i++)
            {
                var key = cache.ElementAt(i).Key;
                if (key.IndexOf(headerStr) == 0)
                {
                    cache.Remove(cache.ElementAt(i).Key);
                }
            }
        }
    }
}