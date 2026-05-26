using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Commands.DeleteEmployeeRoleUnitDepartment;

public class DeleteEmployeeRoleUnitDepartmentCommandHandler(
    IEmployeeRoleUnitDepartmentService employeeRoleUnitDepartmentService)
    : IRequestHandler<DeleteEmployeeRoleUnitDepartmentCommand>
{
    public async Task Handle(DeleteEmployeeRoleUnitDepartmentCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleUnitDepartmentService.Delete(
            request.Id,
            cancellationToken);
    }
}