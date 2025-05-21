using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;

public class DeleteEmployeeRoleCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleCommand>
{
    public async Task Handle(DeleteEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.DeleteEmployeeRole(
            request.EmployeeRoleId,
            cancellationToken);
    }
}