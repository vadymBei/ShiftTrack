using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.Communication.Notifications.Interfaces;
using ShiftTrack.Application.Modules.Communication.Notifications.Services;

namespace ShiftTrack.Application.Modules.Communication;

public static class CommunicationModule
{
    public static IServiceCollection AddCommunicationServices(this IServiceCollection services)
    {
        //Notifications
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<INotificationRecipientResolver, NotificationRecipientResolver>();

        return services;
    }
}
