using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;

public class DeleteEmployeeRoleUnitDepartmentCommandHandler(
    IEmployeeRoleService employeeRoleService) : IRequestHandler<DeleteEmployeeRoleUnitDepartmentCommand>
{
    public async Task<Unit> Handle(DeleteEmployeeRoleUnitDepartmentCommand request, CancellationToken cancellationToken)
    {
        await employeeRoleService.DeleteEmployeeRoleUnitDepartment(
            request.Id,
            cancellationToken);
        
        return Unit.Value;
    }
}