using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

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