using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisDemo.Configurations;
using RedisDemo.Services;
using StackExchange.Redis;

namespace RedisDemo.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = new RedisConfiguration();
            configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);

            services.AddSingleton(redisConfiguration);

            if (!redisConfiguration.Enable) return;

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));

            services.AddStackExchangeRedisCache(opt => opt.Configuration = redisConfiguration.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}