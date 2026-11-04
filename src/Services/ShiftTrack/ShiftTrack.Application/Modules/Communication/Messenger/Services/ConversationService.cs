using System.Net;
using ShiftTrack.Application.Modules.Communication.Messenger.Constants;
using ShiftTrack.Application.Modules.Communication.Messenger.Exceptions;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Mappers;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Application.Modules.Communication.Messenger.ViewModels;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.Communication.Messenger.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

public class ConversationService(
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork,
    IConversationRepository conversationRepository,
    IMessageRepository messageRepository,
    IEmployeeRepository employeeRepository,
    IMessengerPublisher messengerPublisher,
    IMessageAttachmentRepository attachmentRepository,
    IAttachmentStorage attachmentStorage) : IConversationService
{
    private Employee Me => currentUserService.Employee;

    public Task<IReadOnlyList<ConversationSummary>> GetMy(int take, CancellationToken cancellationToken)
    {
        return conversationRepository.GetSummaries(Me.Id, Math.Clamp(take, 1, MessengerConstants.MaxConversations),
            null, cancellationToken);
    }

    public async Task<ConversationSummary> Get(long id, CancellationToken cancellationToken)
    {
        await EnsureMember(id, cancellationToken);

        return await Load(id, cancellationToken);
    }

    public async Task<ConversationSummary> OpenDirect(long employeeId, CancellationToken cancellationToken)
    {
        if (employeeId == Me.Id)
            throw Error(MessengerExceptionsLocalization.CANNOT_CHAT_WITH_YOURSELF,
                nameof(MessengerExceptionsLocalization.CANNOT_CHAT_WITH_YOURSELF));

        // Throws for a missing or deleted employee.
        await employeeRepository.GetById(employeeId, cancellationToken);

        var id = await conversationRepository.GetOrCreateDirect(Me.Id, employeeId, cancellationToken);

        // The other member sees the chat with its first message, nothing is sent until then.
        return await Load(id, cancellationToken);
    }

    public async Task<ConversationSummary> CreateGroup(string title, IReadOnlyCollection<long> memberIds,
        CancellationToken cancellationToken)
    {
        var employees = await GetEmployees(memberIds.Where(x => x != Me.Id), cancellationToken);

        if (employees.Count == 0)
            throw Error(MessengerExceptionsLocalization.GROUP_NEEDS_MEMBERS,
                nameof(MessengerExceptionsLocalization.GROUP_NEEDS_MEMBERS));

        EnsureLimit(employees.Count + 1);

        var now = DateTime.UtcNow;
        var conversation = new Conversation
        {
            Type = ConversationType.Group,
            Title = title.Trim(),
            Members =
            [
                new ConversationMember { EmployeeId = Me.Id, Role = ConversationMemberRole.Owner, JoinedAt = now },
                ..employees.Select(x => new ConversationMember
                {
                    EmployeeId = x.Id,
                    Role = ConversationMemberRole.Member,
                    JoinedAt = now
                })
            ]
        };

        await conversationRepository.Add(conversation, cancellationToken);

        var allIds = MemberIds(conversation);
        await PostSystemMessage(conversation.Id, SystemMessageTexts.Created(Me, conversation.Title), allIds,
            cancellationToken);
        await PublishChanged(conversation.Id, allIds);

        return await Load(conversation.Id, cancellationToken);
    }

    public async Task<ConversationSummary> Rename(long id, string title, CancellationToken cancellationToken)
    {
        var conversation = await GetGroup(id, cancellationToken);
        EnsureOwner(conversation);

        title = title.Trim();

        if (conversation.Title != title)
        {
            conversation.Title = title;
            await conversationRepository.Save(cancellationToken);

            var memberIds = MemberIds(conversation);
            await PostSystemMessage(id, SystemMessageTexts.Renamed(Me, title), memberIds, cancellationToken);
            await PublishChanged(id, memberIds);
        }

        return await Load(id, cancellationToken);
    }

    public async Task<ConversationSummary> AddMembers(long id, IReadOnlyCollection<long> employeeIds,
        CancellationToken cancellationToken)
    {
        // Any member can invite colleagues, like in other messengers.
        var conversation = await GetGroup(id, cancellationToken);

        var newIds = employeeIds.Where(x => conversation.FindMember(x) is null).Distinct().ToList();

        if (newIds.Count > 0)
        {
            var employees = await GetEmployees(newIds, cancellationToken);

            EnsureLimit(conversation.Members.Count + employees.Count);

            var now = DateTime.UtcNow;

            foreach (var employee in employees)
            {
                conversation.Members.Add(new ConversationMember
                {
                    EmployeeId = employee.Id,
                    Role = ConversationMemberRole.Member,
                    JoinedAt = now,
                    // The history is visible, but it isn't unread for the new member.
                    LastReadMessageId = conversation.LastMessageId
                });
            }

            await conversationRepository.Save(cancellationToken);

            var memberIds = MemberIds(conversation);
            await PostSystemMessage(id, SystemMessageTexts.Added(Me, employees), memberIds, cancellationToken);
            await PublishChanged(id, memberIds);
        }

        return await Load(id, cancellationToken);
    }

    public async Task<ConversationSummary> RemoveMember(long id, long employeeId, CancellationToken cancellationToken)
    {
        var conversation = await GetGroup(id, cancellationToken);
        EnsureOwner(conversation);

        if (employeeId == Me.Id)
            throw Error(MessengerExceptionsLocalization.CANNOT_REMOVE_YOURSELF,
                nameof(MessengerExceptionsLocalization.CANNOT_REMOVE_YOURSELF));

        var member = conversation.FindMember(employeeId)
                     ?? throw Error(MessengerExceptionsLocalization.MEMBER_NOT_FOUND,
                         nameof(MessengerExceptionsLocalization.MEMBER_NOT_FOUND));

        conversation.Members.Remove(member);
        await conversationRepository.Save(cancellationToken);

        var memberIds = MemberIds(conversation);
        await PostSystemMessage(id, SystemMessageTexts.Removed(Me, member.Employee), memberIds, cancellationToken);
        // The removed member's clients drop the conversation when they can't load it anymore.
        await PublishChanged(id, [..memberIds, employeeId]);

        return await Load(id, cancellationToken);
    }

    public async Task Leave(long id, CancellationToken cancellationToken)
    {
        var conversation = await GetGroup(id, cancellationToken);
        var me = conversation.FindMember(Me.Id);

        conversation.Members.Remove(me);

        if (conversation.Members.Count == 0)
        {
            // The rows go with the conversation (cascade), the files are removed after the commit.
            var storedNames = await attachmentRepository.GetStoredNames(id, cancellationToken);

            await conversationRepository.Remove(conversation, cancellationToken);

            await unitOfWork.AfterCommit(() =>
            {
                foreach (var storedName in storedNames)
                    attachmentStorage.Delete(storedName);
                return Task.CompletedTask;
            });
            await PublishChanged(id, [Me.Id]);
            return;
        }

        Employee newOwner = null;

        if (me.Role == ConversationMemberRole.Owner
            && conversation.Members.All(x => x.Role != ConversationMemberRole.Owner))
        {
            var successor = conversation.Members.OrderBy(x => x.JoinedAt).ThenBy(x => x.Id).First();
            successor.Role = ConversationMemberRole.Owner;
            newOwner = successor.Employee;
        }

        await conversationRepository.Save(cancellationToken);

        var memberIds = MemberIds(conversation);
        await PostSystemMessage(id, SystemMessageTexts.Left(Me), memberIds, cancellationToken);

        if (newOwner is not null)
            await PostSystemMessage(id, SystemMessageTexts.NewOwner(newOwner), memberIds, cancellationToken);

        await PublishChanged(id, [..memberIds, Me.Id]);
    }

    public async Task<ConversationSummary> SetMuted(long id, bool muted, CancellationToken cancellationToken)
    {
        await EnsureMember(id, cancellationToken);

        if (await conversationRepository.SetMuted(id, Me.Id, muted, cancellationToken))
        {
            // The user's other tabs: the badge and the counter in the menu.
            await unitOfWork.AfterCommit(() =>
                messengerPublisher.ConversationChanged([Me.Id], id, CancellationToken.None));
        }

        return await Load(id, cancellationToken);
    }

    public async Task MarkRead(long id, long messageId, CancellationToken cancellationToken)
    {
        await EnsureMember(id, cancellationToken);

        if (!await messageRepository.Exists(id, messageId, cancellationToken))
            throw Error(MessengerExceptionsLocalization.MESSAGE_FROM_OTHER_CONVERSATION,
                nameof(MessengerExceptionsLocalization.MESSAGE_FROM_OTHER_CONVERSATION));

        if (!await conversationRepository.MarkRead(id, Me.Id, messageId, cancellationToken))
            return;

        // Others show their messages as read, the reader's other tabs update the unread counters.
        var memberIds = await conversationRepository.GetMemberIds(id, cancellationToken);
        var read = new ConversationReadVm { ConversationId = id, EmployeeId = Me.Id, LastReadMessageId = messageId };

        await unitOfWork.AfterCommit(() => messengerPublisher.ConversationRead(memberIds, read, CancellationToken.None));
    }

    public async Task Typing(long id, CancellationToken cancellationToken)
    {
        var memberIds = await conversationRepository.GetMemberIds(id, cancellationToken);

        if (!memberIds.Contains(Me.Id))
            throw NotAMember();

        var others = memberIds.Where(x => x != Me.Id).ToList();
        var typing = new TypingVm { ConversationId = id, EmployeeId = Me.Id, FullName = Me.FullName };

        await unitOfWork.AfterCommit(() => messengerPublisher.Typing(others, typing, CancellationToken.None));
    }

    public Task<MessengerUnreadCount> CountUnread(CancellationToken cancellationToken)
    {
        return conversationRepository.CountUnread(Me.Id, cancellationToken);
    }

    public Task<IReadOnlyList<Employee>> SearchContacts(string search, CancellationToken cancellationToken)
    {
        return employeeRepository.SearchContacts(search.Trim(), Me.Id, MessengerConstants.MaxContacts,
            cancellationToken);
    }

    private async Task<ConversationSummary> Load(long id, CancellationToken cancellationToken)
    {
        var summaries = await conversationRepository.GetSummaries(Me.Id, 1, id, cancellationToken);

        return summaries.FirstOrDefault() ?? throw new EntityNotFoundException(typeof(Conversation), id);
    }

    private async Task<Conversation> GetGroup(long id, CancellationToken cancellationToken)
    {
        var conversation = await conversationRepository.GetForUpdate(id, cancellationToken);

        if (conversation?.FindMember(Me.Id) is null)
            throw NotAMember();

        if (conversation.Type != ConversationType.Group)
            throw Error(MessengerExceptionsLocalization.NOT_A_GROUP, nameof(MessengerExceptionsLocalization.NOT_A_GROUP));

        return conversation;
    }

    private async Task EnsureMember(long id, CancellationToken cancellationToken)
    {
        if (!await conversationRepository.IsMember(id, Me.Id, cancellationToken))
            throw NotAMember();
    }

    /// <summary>
    /// The owner manages the group. If the owner is no longer available (a deleted employee), the earliest member does.
    /// </summary>
    private void EnsureOwner(Conversation conversation)
    {
        var owner = conversation.Members.FirstOrDefault(x => x.Role == ConversationMemberRole.Owner)
                    ?? conversation.Members.OrderBy(x => x.JoinedAt).ThenBy(x => x.Id).First();

        if (owner.EmployeeId != Me.Id)
            throw Error(MessengerExceptionsLocalization.ONLY_OWNER, nameof(MessengerExceptionsLocalization.ONLY_OWNER),
                HttpStatusCode.Forbidden);
    }

    private static void EnsureLimit(int count)
    {
        if (count > MessengerConstants.MaxGroupMembers)
            throw Error(MessengerExceptionsLocalization.TOO_MANY_MEMBERS,
                nameof(MessengerExceptionsLocalization.TOO_MANY_MEMBERS));
    }

    private async Task<IReadOnlyList<Employee>> GetEmployees(IEnumerable<long> ids, CancellationToken cancellationToken)
    {
        var distinctIds = ids.Distinct().ToList();

        if (distinctIds.Count == 0)
            return [];

        var employees = (await employeeRepository.GetListByIds(distinctIds, cancellationToken)).ToList();

        if (employees.Count != distinctIds.Count)
            throw Error(MessengerExceptionsLocalization.EMPLOYEES_NOT_FOUND,
                nameof(MessengerExceptionsLocalization.EMPLOYEES_NOT_FOUND));

        return employees.OrderBy(x => x.Surname).ThenBy(x => x.Name).ToList();
    }

    private async Task PostSystemMessage(long conversationId, string text, IReadOnlyCollection<long> memberIds,
        CancellationToken cancellationToken)
    {
        var message = new Message
        {
            ConversationId = conversationId,
            Type = MessageType.System,
            Text = text
        };

        await messageRepository.Add(message, cancellationToken);
        await conversationRepository.SetLastMessage(conversationId, message.Id, message.CreatedAt, cancellationToken);
        await conversationRepository.MarkRead(conversationId, Me.Id, message.Id, cancellationToken);

        var messageVm = (await messageRepository.GetById(message.Id, cancellationToken)).ToVm();

        await unitOfWork.AfterCommit(() =>
            messengerPublisher.MessageCreated(memberIds, messageVm, CancellationToken.None));
    }

    private Task PublishChanged(long conversationId, IReadOnlyCollection<long> employeeIds)
    {
        var recipients = employeeIds.Distinct().ToList();

        return unitOfWork.AfterCommit(() =>
            messengerPublisher.ConversationChanged(recipients, conversationId, CancellationToken.None));
    }

    private static List<long> MemberIds(Conversation conversation) =>
        conversation.Members.Select(x => x.EmployeeId).ToList();

    private static MessengerException NotAMember() =>
        Error(MessengerExceptionsLocalization.NOT_A_MEMBER, nameof(MessengerExceptionsLocalization.NOT_A_MEMBER),
            HttpStatusCode.Forbidden);

    private static MessengerException Error(string message, string type, HttpStatusCode code = HttpStatusCode.BadRequest) =>
        new(message, type, code);
}
