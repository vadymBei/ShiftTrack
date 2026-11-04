using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Core.Unit.Tests.Builders;

/// <summary>Conversations with members and their employees, as loaded for an update.</summary>
public class ConversationBuilder
{
    private static readonly DateTime Start = new(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc);

    private readonly Conversation _conversation;

    private ConversationBuilder(ConversationType type, string title)
    {
        _conversation = new Conversation { Id = 10, Type = type, Title = title, CreatedAt = Start };
    }

    public static ConversationBuilder Direct() => new(ConversationType.Direct, null);

    public static ConversationBuilder Group(string title = "Продажі") => new(ConversationType.Group, title);

    public static Employee Employee(long id, string surname = "Коваль", string name = "Олена",
        EmployeeGender gender = EmployeeGender.Female) =>
        new() { Id = id, Surname = surname, Name = name, Patronymic = "", Gender = gender };

    public ConversationBuilder WithId(long id)
    {
        _conversation.Id = id;
        return this;
    }

    public ConversationBuilder WithOwner(long employeeId) =>
        WithMember(employeeId, ConversationMemberRole.Owner);

    /// <summary>Members join one minute after another in the order they are added.</summary>
    public ConversationBuilder WithMember(long employeeId, ConversationMemberRole role = ConversationMemberRole.Member,
        long? lastReadMessageId = null)
    {
        _conversation.Members.Add(new ConversationMember
        {
            Id = _conversation.Members.Count + 1,
            ConversationId = _conversation.Id,
            EmployeeId = employeeId,
            Employee = Employee(employeeId, $"Працівник{employeeId}", "Тест", EmployeeGender.Male),
            Role = role,
            JoinedAt = Start.AddMinutes(_conversation.Members.Count),
            LastReadMessageId = lastReadMessageId
        });

        return this;
    }

    public Conversation Build() => _conversation;
}
