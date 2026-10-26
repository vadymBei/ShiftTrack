using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;

namespace ShiftTrack.Infrastructure.Realtime;

/// <summary>
/// Clients only listen: "notificationReceived" (a new notification) and
/// "notificationsChanged" (read / resolved / deleted — reload the counter).
/// Every connection joins the group of its employee, so delivery works across
/// several API instances and from ShiftTrack.Jobs through the Redis backplane.
/// </summary>
[Authorize]
public class NotificationsHub(IEmployeeRepository employeeRepository) : Hub
{
    public const string Path = "/hubs/shift-track/notifications";

    public static string EmployeeGroup(long employeeId) => $"employee:{employeeId}";

    public override async Task OnConnectedAsync()
    {
        var integrationId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(integrationId))
        {
            var employee = await employeeRepository.GetByIntegrationId(integrationId, Context.ConnectionAborted);

            if (employee is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, EmployeeGroup(employee.Id), Context.ConnectionAborted);
            }
        }

        await base.OnConnectedAsync();
    }
}
