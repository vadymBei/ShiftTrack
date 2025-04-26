using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;

public class DeleteEmployeeRoleCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleCommand>
{
    public async Task<Unit> Handle(DeleteEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.DeleteEmployeeRole(
            request.EmployeeRoleId,
            cancellationToken);
        
        return Unit.Value;
    }
}