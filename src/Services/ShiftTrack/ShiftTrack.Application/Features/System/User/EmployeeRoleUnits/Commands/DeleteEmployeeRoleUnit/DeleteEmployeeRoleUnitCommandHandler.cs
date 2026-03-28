using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnits.Commands.DeleteEmployeeRoleUnit;

internal class DeleteEmployeeRoleUnitCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleUnitCommand>
{
    public async Task Handle(DeleteEmployeeRoleUnitCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.DeleteEmployeeRoleUnit(
            request.Id,
            cancellationToken);
    }
}