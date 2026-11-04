using Microsoft.Net.Http.Headers;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetAttachment;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RemoveAttachment;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.UploadAttachment;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Dtos;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.AddGroupMembers;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.CreateGroup;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.DeleteMessage;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.EditMessage;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.LeaveConversation;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.MarkConversationRead;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.OpenDirectConversation;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RemoveGroupMember;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.RenameGroup;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendMessage;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendTyping;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetConversation;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessages;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMessengerUnreadCount;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.GetMyConversations;
using ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Queries.SearchChatContacts;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Communication.Messenger;

/// <summary>Direct chats and groups. Anyone can write to anyone in the company.</summary>
[Authorize]
[Route("api/shift-track/communication/messenger")]
public class COM_MSG_MessengerController : ApiController
{
    [HttpGet("conversations")]
    public Task<List<ConversationVm>> GetMyConversations([FromQuery] int take = 100)
        => Mediator.Send(new GetMyConversationsQuery(take));

    [HttpGet("conversations/{id}")]
    public Task<ConversationVm> GetConversation(long id)
        => Mediator.Send(new GetConversationQuery(id));

    /// <summary>Opens the direct chat with the employee, creating it if needed.</summary>
    [HttpPost("conversations/direct")]
    public Task<ConversationVm> OpenDirect([FromBody] OpenDirectConversationDto dto)
        => Mediator.Send(new OpenDirectConversationCommand(dto.EmployeeId));

    [HttpPost("conversations/groups")]
    public Task<ConversationVm> CreateGroup([FromBody] CreateGroupDto dto)
        => Mediator.Send(new CreateGroupCommand(dto.Title, dto.MemberIds ?? []));

    [HttpPut("conversations/{id}/title")]
    public Task<ConversationVm> RenameGroup(long id, [FromBody] RenameGroupDto dto)
        => Mediator.Send(new RenameGroupCommand(id, dto.Title));

    [HttpPost("conversations/{id}/members")]
    public Task<ConversationVm> AddMembers(long id, [FromBody] AddGroupMembersDto dto)
        => Mediator.Send(new AddGroupMembersCommand(id, dto.EmployeeIds ?? []));

    [HttpDelete("conversations/{id}/members/{employeeId}")]
    public Task<ConversationVm> RemoveMember(long id, long employeeId)
        => Mediator.Send(new RemoveGroupMemberCommand(id, employeeId));

    [HttpPost("conversations/{id}/leave")]
    public async Task<IActionResult> Leave(long id)
    {
        await Mediator.Send(new LeaveConversationCommand(id));

        return Ok();
    }

    [HttpPut("conversations/{id}/read")]
    public async Task<IActionResult> MarkRead(long id, [FromBody] MarkConversationReadDto dto)
    {
        await Mediator.Send(new MarkConversationReadCommand(id, dto.MessageId));

        return Ok();
    }

    /// <summary>The user is typing: other members see it for a few seconds.</summary>
    [HttpPost("conversations/{id}/typing")]
    public async Task<IActionResult> Typing(long id)
    {
        await Mediator.Send(new SendTypingCommand(id));

        return Ok();
    }

    /// <summary>Messages before <paramref name="beforeId"/> (or the newest), oldest first.</summary>
    [HttpGet("conversations/{id}/messages")]
    public Task<List<MessageVm>> GetMessages(long id, [FromQuery] long? beforeId,
        [FromQuery] int take = MessengerConstants.DefaultMessagesPage)
        => Mediator.Send(new GetMessagesQuery(id, beforeId, take));

    [HttpPost("conversations/{id}/messages")]
    public Task<MessageVm> SendMessage(long id, [FromBody] SendMessageDto dto)
        => Mediator.Send(new SendMessageCommand(id, dto.Text, dto.ClientMessageId, dto.ReplyToMessageId,
            dto.AttachmentIds ?? []));

    /// <summary>Uploads a file to send in a message; files that are never sent are removed after a day.</summary>
    [HttpPost("conversations/{id}/attachments")]
    [RequestSizeLimit(MessengerConstants.MaxAttachmentSize + 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = MessengerConstants.MaxAttachmentSize + 1024 * 1024)]
    public async Task<AttachmentVm> UploadAttachment(long id, IFormFile file)
    {
        await using var content = file?.OpenReadStream() ?? Stream.Null;

        return await Mediator.Send(new UploadAttachmentCommand(id,
            new AttachmentUpload(content, file?.FileName, file?.Length ?? 0)));
    }

    /// <summary>Removes an upload that isn't sent yet.</summary>
    [HttpDelete("attachments/{id}")]
    public async Task<IActionResult> RemoveAttachment(long id)
    {
        await Mediator.Send(new RemoveAttachmentCommand(id));

        return Ok();
    }

    /// <summary>The file for the members of the conversation; images and PDF open in the browser unless download=true.</summary>
    [HttpGet("attachments/{id}")]
    public async Task<IActionResult> GetAttachment(long id, [FromQuery] bool download = false)
    {
        var file = await Mediator.Send(new GetAttachmentQuery(id, download));

        Response.Headers["X-Content-Type-Options"] = "nosniff";
        Response.Headers.CacheControl = "private, max-age=86400";

        var disposition = new ContentDispositionHeaderValue(file.Inline ? "inline" : "attachment");
        disposition.SetHttpFileName(file.FileName);
        Response.Headers.ContentDisposition = disposition.ToString();

        return File(file.Content, file.ContentType, enableRangeProcessing: true);
    }

    [HttpPut("messages/{id}")]
    public Task<MessageVm> EditMessage(long id, [FromBody] EditMessageDto dto)
        => Mediator.Send(new EditMessageCommand(id, dto.Text));

    [HttpDelete("messages/{id}")]
    public Task<MessageVm> DeleteMessage(long id)
        => Mediator.Send(new DeleteMessageCommand(id));

    [HttpGet("unread/count")]
    public Task<MessengerUnreadCountVm> GetUnreadCount()
        => Mediator.Send(new GetMessengerUnreadCountQuery());

    /// <summary>Employees of the whole company to start a chat or a group with.</summary>
    [HttpGet("contacts")]
    public Task<List<ChatContactVm>> SearchContacts([FromQuery] string search)
        => Mediator.Send(new SearchChatContactsQuery(search));
}
