using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Domain.System.User.Employees.Events;

public record EmployeeContactDetailsChangedEvent(
    string EmployeeIntegrationId,
    string Email,
    string PhoneNumber) : IDomainEvent;