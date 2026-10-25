using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.DeleteNotification;
using ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.MarkAllNotificationsAsRead;
using ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Commands.MarkNotificationAsRead;
using ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Queries.GetMyNotifications;
using ShiftTrack.Application.Modules.Communication.Notifications.UseCases.Queries.GetUnreadNotificationsCount;
using ShiftTrack.Application.Modules.Communication.Notifications.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Communication.Notifications;

[Authorize]
[Route("api/shift-track/communication/notifications")]
public class COM_NTF_NotificationsController : ApiController
{
    [HttpGet]
    public Task<IEnumerable<NotificationVm>> GetMyNotifications(
        [FromQuery] int take = 30,
        [FromQuery] bool onlyUnread = false)
        => Mediator.Send(new GetMyNotificationsQuery(take, onlyUnread));

    [HttpGet("unread/count")]
    public Task<UnreadNotificationsCountVm> GetUnreadCount()
        => Mediator.Send(new GetUnreadNotificationsCountQuery());

    [HttpPut("read/{id}")]
    public async Task<IActionResult> MarkAsRead(long id)
    {
        await Mediator.Send(new MarkNotificationAsReadCommand(id));

        return Ok();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        await Mediator.Send(new MarkAllNotificationsAsReadCommand());

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await Mediator.Send(new DeleteNotificationCommand(id));

        return Ok();
    }
}
