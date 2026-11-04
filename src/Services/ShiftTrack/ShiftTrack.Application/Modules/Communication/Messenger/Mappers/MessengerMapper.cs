using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Mappers;

/// <summary>Manual mapping: a conversation looks different for every member (title, unread count, role).</summary>
public static class MessengerMapper
{
    public static ConversationVm ToVm(this ConversationSummary summary, long currentEmployeeId)
    {
        var conversation = summary.Conversation;
        var members = conversation.Members
            .OrderByDescending(x => x.Role == ConversationMemberRole.Owner)
            .ThenBy(x => x.Employee?.Surname)
            .ThenBy(x => x.Employee?.Name)
            .ToList();

        var me = members.FirstOrDefault(x => x.EmployeeId == currentEmployeeId);
        var others = members.Where(x => x.EmployeeId != currentEmployeeId).ToList();

        var interlocutor = conversation.Type == ConversationType.Direct
            ? others.FirstOrDefault()?.ToVm()
            : null;

        return new ConversationVm
        {
            Id = conversation.Id,
            Type = conversation.Type,
            Title = conversation.Type == ConversationType.Direct ? interlocutor?.FullName : conversation.Title,
            Interlocutor = interlocutor,
            Members = members.Select(x => x.ToVm()).ToList(),
            LastMessage = summary.LastMessage?.ToVm(),
            LastActivityAt = conversation.LastMessageAt ?? conversation.CreatedAt,
            UnreadCount = summary.UnreadCount,
            ReadByOthersUpToMessageId = others.Max(x => x.LastReadMessageId),
            MyRole = me?.Role ?? ConversationMemberRole.None,
            IsMuted = me?.IsMuted ?? false,
            HasUnreadMention = summary.HasUnreadMention,
            CanWrite = me is not null && (conversation.Type == ConversationType.Group || interlocutor is not null)
        };
    }

    public static ChatMemberVm ToVm(this ConversationMember member) => new()
    {
        EmployeeId = member.EmployeeId,
        FullName = member.Employee?.FullName,
        PositionName = member.Employee?.Position?.Name,
        DepartmentName = member.Employee?.Department?.Name,
        UnitName = member.Employee?.Department?.Unit?.Name,
        HasPhoto = !string.IsNullOrEmpty(member.Employee?.PhotoFullName),
        Role = member.Role,
        LastReadMessageId = member.LastReadMessageId
    };

    public static MessageVm ToVm(this Message message) => new()
    {
        Id = message.Id,
        ConversationId = message.ConversationId,
        Type = message.Type,
        Text = message.IsDeleted ? null : message.Text,
        AuthorId = message.AuthorId,
        AuthorFullName = message.Author?.FullName,
        CreatedAt = message.CreatedAt,
        EditedAt = message.EditedAt,
        IsDeleted = message.IsDeleted,
        ClientMessageId = message.ClientMessageId,
        Mentions = message.IsDeleted
            ? []
            : (message.Mentions ?? [])
                .Where(x => x.Employee is not null)
                .Select(x => new MentionVm { EmployeeId = x.EmployeeId, FullName = x.Employee.FullName })
                .ToList(),
        Attachments = message.IsDeleted
            ? []
            : (message.Attachments ?? []).OrderBy(x => x.Id).Select(x => x.ToVm()).ToList(),
        ReplyTo = message.ReplyToMessage is { } reply
            ? new MessagePreviewVm
            {
                Id = reply.Id,
                AuthorId = reply.AuthorId,
                AuthorFullName = reply.Author?.FullName,
                Text = reply.IsDeleted ? null : Shorten(PlainText(reply)),
                IsDeleted = reply.IsDeleted
            }
            : null
    };

    public static AttachmentVm ToVm(this MessageAttachment attachment) => new()
    {
        Id = attachment.Id,
        FileName = attachment.FileName,
        ContentType = attachment.ContentType,
        Size = attachment.Size,
        IsImage = attachment.IsImage
    };

    public static ChatContactVm ToContactVm(this Employee employee) => new()
    {
        EmployeeId = employee.Id,
        FullName = employee.FullName,
        PositionName = employee.Position?.Name,
        DepartmentName = employee.Department?.Name,
        UnitName = employee.Department?.Unit?.Name,
        HasPhoto = !string.IsNullOrEmpty(employee.PhotoFullName)
    };

    /// <summary>The text with "@Name" for the mention tokens.</summary>
    public static string PlainText(Message message) =>
        Services.MentionTokens.ToPlainText(message.Text, id =>
            message.Mentions?.FirstOrDefault(x => x.EmployeeId == id)?.Employee?.FullName);

    private static string Shorten(string text) =>
        text is null || text.Length <= Constants.MessengerConstants.ReplyPreviewLength
            ? text
            : text[..Constants.MessengerConstants.ReplyPreviewLength] + "…";
}
