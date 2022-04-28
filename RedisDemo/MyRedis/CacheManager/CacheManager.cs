using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Reso.Core.Extension;

namespace MyRedis.CacheManager
{
    public static class CacheManager
    {
        public static async System.Threading.Tasks.Task<T> GetObjectAsync<T>(IMemoryCache _memoryCache = null, IDistributedCache _distributedCache = null,
            string key = null) where T : class
        {
            T rs = null;
            if (_memoryCache != null && _memoryCache.TryGetValue(key, out rs))
            {
                return rs;
            }
            if (_distributedCache != null)
            {
                try
                {
                    rs = await _distributedCache.GetAsync<T>(key);
                }
                catch
                {
                    //do nothing
                }
            }
            if (_memoryCache != null && rs != null)
            {
                _memoryCache.Set(key, rs);
            }
            return rs;
        }

        public static async System.Threading.Tasks.Task SetObjectAsync(IMemoryCache _memoryCache = null,
            IDistributedCache _distributedCache = null, string key = null, object obj = null)
        {
            if (_memoryCache != null)
            {
                _memoryCache.Set(key, obj);
            }
            if (_distributedCache != null)
            {
                try
                {
                    await _distributedCache.SetObjectAsync(key, obj);
                }
                catch
                {
                    //do nothing
                }

            }

        }
        
        public static async System.Threading.Tasks.Task RemoveObjectAsync(IMemoryCache _memoryCache = null,
            IDistributedCache _distributedCache = null, string key = null)
        {
            if (_memoryCache != null)
            {
                _memoryCache.Remove(key);
            }
            if (_distributedCache != null)
            {
                try
                {
                    await _distributedCache.RemoveAsync(key);
                }
                catch
                {
                    //do nothing
                }

            }

        }
    }
}
