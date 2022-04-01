using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace RedisDemo.Services
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCachedResponseAsync(string key);
        Task RemoveResponseCacheAsync(string pattern);
    }

    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
            {
                return;
            }

            var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(key);
            return string.IsNullOrWhiteSpace(cacheResponse) ? null : cacheResponse;
        }

        public async Task RemoveResponseCacheAsync(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Value cannot be null or whitespace");
            }

            await foreach (var key in GetKeyAsync(pattern + "*"))
            {
                await _distributedCache.RemoveAsync(key);
            }
        }

        private async IAsyncEnumerable<string> GetKeyAsync(string partern)
        {
            if (string.IsNullOrEmpty(partern))
            {
                throw new ArgumentException("Value cannot be null or whitespace");
            }

            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                foreach (var key in server.Keys(pattern: partern))
                {
                    yield return key.ToString();
                }
            }
        }
    }
}