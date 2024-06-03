using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisTech.Application.Mapping;
using RedisTech.Application.Services;
using RedisTech.Domain.Interfaces.Services;
using StackExchange.Redis;

namespace RedisTech.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(UserMapping));
        
        var redisUrl = configuration.GetConnectionString("RedisUrl");
        services.AddScoped<IDatabase>(cfg =>
        {
            var multiplexer = ConnectionMultiplexer.Connect(redisUrl ?? string.Empty);
            return multiplexer.GetDatabase();
        });
        
        services.AddStackExchangeRedisCache(options => {
            options.Configuration = redisUrl;
            options.InstanceName = "local";
        });
        
        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}