using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Employees.Events;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.EventHandlers;

internal sealed class EmployeeContactDetailsChangedEventHandler(
    IEmployeeService employeeService) : IDomainEventHandler<EmployeeContactDetailsChangedEvent>
{
    public async Task Handle(EmployeeContactDetailsChangedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await employeeService.UpdateAuthUser(
            new UserToUpdateDto(
                domainEvent.EmployeeIntegrationId,
                domainEvent.Email,
                domainEvent.PhoneNumber)
            , cancellationToken);
    }
}