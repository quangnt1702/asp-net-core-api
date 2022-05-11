using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace MyRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IConfiguration _configuration;

        public RedisController(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult<object>> StoreIntoRedis([FromBody] MyObject myObject)
        {
            await CacheManager.CacheManager.SetObjectAsync(null, _distributedCache, myObject.Age.ToString(), myObject);
            return myObject;
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<object>> DeleteInRedisByKey(string key)
        {
            await CacheManager.CacheManager.RemoveObjectAsync(null, _distributedCache, key);
            return Ok();
        }
        
        [HttpDelete]
        public async Task<ActionResult<object>> DeleteAll()
        {
            var keys =  CacheManager.CacheManager.GetAllkeys(_connectionMultiplexer, _configuration);
            foreach (var key in keys)
            {
                await CacheManager.CacheManager.RemoveObjectAsync(null, _distributedCache, key);
            }
            return Ok();
        }
        
        [HttpGet("all")]
        public  ActionResult<object> GetAllKey()
        {
            var keys =  CacheManager.CacheManager.GetAllkeys(_connectionMultiplexer, _configuration);
            
            return Ok(keys);
        }
    }

    public class MyObject
    {
        public int Age { get; set; }
        public string MyName { get; set; }
        public string MyPhone { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}