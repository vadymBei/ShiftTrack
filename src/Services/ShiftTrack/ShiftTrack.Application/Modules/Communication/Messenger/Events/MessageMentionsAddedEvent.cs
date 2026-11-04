using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Events;

/// <summary>Employees were mentioned in a new message or added to an edited one.</summary>
public record MessageMentionsAddedEvent(
    long MessageId,
    IReadOnlyCollection<long> EmployeeIds) : IDomainEvent;
