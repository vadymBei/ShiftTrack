using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoles.UseCases.Commands.DeleteEmployeeRole;

public class DeleteEmployeeRoleCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleCommand>
{
    public async Task Handle(DeleteEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.Delete(
            request.EmployeeRoleId,
            cancellationToken);
    }
}