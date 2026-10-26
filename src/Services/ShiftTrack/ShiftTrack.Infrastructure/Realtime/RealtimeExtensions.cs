using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using StackExchange.Redis;

namespace ShiftTrack.Infrastructure.Realtime;

public static class RealtimeExtensions
{
    /// <summary>
    /// SignalR with the Redis backplane. The API hosts the hub, other hosts (ShiftTrack.Jobs) only send to it:
    /// a message goes through Redis to the API instance the client is connected to.
    /// </summary>
    public static IServiceCollection AddRealtime(this IServiceCollection services, IConfiguration configuration)
    {
        var signalR = services
            .AddSignalR()
            .AddJsonProtocol(options =>
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        var redisConnectionString = configuration.GetConnectionString("Redis");

        if (!string.IsNullOrWhiteSpace(redisConnectionString))
        {
            signalR.AddStackExchangeRedis(redisConnectionString, options =>
                options.Configuration.ChannelPrefix = RedisChannel.Literal("shift-track"));
        }

        services.AddScoped<INotificationPublisher, SignalRNotificationPublisher>();

        return services;
    }
}
