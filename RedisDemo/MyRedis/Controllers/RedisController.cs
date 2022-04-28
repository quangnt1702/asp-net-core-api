using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace MyRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public RedisController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        [HttpPost]
        public async Task<ActionResult<object>> StoreIntoRedis([FromBody] MyObject myObject)
        {
            await CacheManager.CacheManager.SetObjectAsync(null, _distributedCache, myObject.Age.ToString(), myObject);
            return myObject;
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<object>> StoreIntoRedis(string key)
        {
            await CacheManager.CacheManager.RemoveObjectAsync(null, _distributedCache, key);
            return Ok();
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