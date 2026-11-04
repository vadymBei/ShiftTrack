using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Core.Unit.Tests.Fakes;
using ShiftTrack.Application.Modules.Communication.Messenger.Interfaces;
using ShiftTrack.Application.Modules.Communication.Messenger.Models;
using ShiftTrack.Application.Modules.Communication.Messenger.Services;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Communication.Messenger;

/// <summary>The services with mocked repositories; the current user is employee <see cref="Me"/>.</summary>
public abstract class MessengerFixture
{
    protected const long Me = 1;

    protected readonly FakeUnitOfWork UnitOfWork = new();
    protected readonly Mock<IConversationRepository> Conversations = new();
    protected readonly Mock<IMessageRepository> Messages = new();
    protected readonly Mock<IEmployeeRepository> Employees = new();
    protected readonly Mock<IMessengerPublisher> Publisher = new();
    protected readonly Mock<IMessageRateLimiter> RateLimiter = new();
    protected readonly Mock<IMessageAttachmentRepository> Attachments = new();
    protected readonly Mock<IAttachmentStorage> Storage = new();

    protected readonly ConversationService ConversationService;
    protected readonly MessageService MessageService;
    protected readonly MessageAttachmentService AttachmentService;

    protected MessengerFixture()
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.Setup(x => x.Employee).Returns(ConversationBuilder.Employee(Me));

        RateLimiter.Setup(x => x.TryAcquire(It.IsAny<long>())).Returns(true);

        // Saved messages get ids, and are read back as they were saved.
        var nextId = 100L;
        var saved = new Dictionary<long, Message>();

        Messages
            .Setup(x => x.Add(It.IsAny<Message>(), It.IsAny<CancellationToken>()))
            .Callback<Message, CancellationToken>((message, _) =>
            {
                message.Id = ++nextId;
                message.AuthorId = Me;
                saved[message.Id] = message;
            })
            .Returns(Task.CompletedTask);
        Messages
            .Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long id, CancellationToken _) => saved.GetValueOrDefault(id));

        Conversations
            .Setup(x => x.GetSummaries(Me, It.IsAny<int>(), It.IsAny<long?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long _, int _, long? id, CancellationToken _) =>
                [new ConversationSummary { Conversation = new Conversation { Id = id ?? 0 } }]);

        ConversationService = new ConversationService(currentUser.Object, UnitOfWork, Conversations.Object,
            Messages.Object, Employees.Object, Publisher.Object, Attachments.Object, Storage.Object);
        MessageService = new MessageService(currentUser.Object, UnitOfWork, Conversations.Object, Messages.Object,
            RateLimiter.Object, Publisher.Object, Attachments.Object, Storage.Object);
        AttachmentService = new MessageAttachmentService(currentUser.Object, UnitOfWork, Conversations.Object,
            Attachments.Object, Storage.Object, NullLogger<MessageAttachmentService>.Instance);

        Attachments
            .Setup(x => x.GetUnsent(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<IReadOnlyCollection<long>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        Attachments
            .Setup(x => x.GetStoredNames(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
    }

    protected Conversation Existing(Conversation conversation)
    {
        Conversations
            .Setup(x => x.GetForUpdate(conversation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conversation);
        Conversations
            .Setup(x => x.IsMember(conversation.Id, It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long _, long employeeId, CancellationToken _) => conversation.FindMember(employeeId) is not null);
        Conversations
            .Setup(x => x.GetMemberIds(conversation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => conversation.Members.Select(m => m.EmployeeId).ToList());

        return conversation;
    }

    protected void EmployeesExist(params long[] ids) =>
        Employees
            .Setup(x => x.GetListByIds(It.IsAny<IEnumerable<long>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<long> requested, CancellationToken _) =>
                requested.Where(ids.Contains).Select(id => ConversationBuilder.Employee(id, $"Працівник{id}")).ToList());

    /// <summary>Runs the action as a command: in a transaction, with real-time messages after the commit.</summary>
    protected Task<T> Command<T>(Func<Task<T>> action) => UnitOfWork.ExecuteInTransaction(action, CancellationToken.None);

    protected Task Command(Func<Task> action) => UnitOfWork.ExecuteInTransaction(async () =>
    {
        await action();
        return 0;
    }, CancellationToken.None);

    protected static IReadOnlyCollection<long> Ids(params long[] ids) =>
        It.Is<IReadOnlyCollection<long>>(x => x.OrderBy(i => i).SequenceEqual(ids.OrderBy(i => i)));
}
