using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedisDemo.Attributes;
using RedisDemo.Services;

namespace RedisDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IResponseCacheService _responseCacheService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponseCacheService responseCacheService)
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [HttpGet]
        [Cache((1000))]
        public  ActionResult<List<WeatherForecast>> GetAsync()
        {
            var result = new List<WeatherForecast>()
            {
                new WeatherForecast() {Name = "Nguyen Van B"},
                new WeatherForecast() {Name = "Nguyen Van C"},
                new WeatherForecast() {Name = "Nguyen Van D"},
                new WeatherForecast() {Name = "Nguyen Van A"}
            };
            return Ok(result);
        }

        [HttpGet("Remove")]
        public async Task<IActionResult> Remove()
        {
            await _responseCacheService.RemoveResponseCacheAsync("/WeatherForecast");
            return Ok( );
        }
    }
}