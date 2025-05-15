using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

public class DeleteEmployeeRoleUnitDepartmentCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleUnitDepartmentCommand>
{
    public async Task Handle(DeleteEmployeeRoleUnitDepartmentCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.DeleteEmployeeRoleUnitDepartment(
            request.Id,
            cancellationToken);
    }
}