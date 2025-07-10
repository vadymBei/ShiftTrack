using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoles.Commands.DeleteEmployeeRole;

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