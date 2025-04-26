using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Commands.DeleteEmployeeRoleUnit;

public class DeleteEmployeeRoleUnitCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleUnitCommand>
{
    public async Task<Unit> Handle(DeleteEmployeeRoleUnitCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.DeleteEmployeeRoleUnit(
            request.Id,
            cancellationToken);
        
        return Unit.Value;
    }
}