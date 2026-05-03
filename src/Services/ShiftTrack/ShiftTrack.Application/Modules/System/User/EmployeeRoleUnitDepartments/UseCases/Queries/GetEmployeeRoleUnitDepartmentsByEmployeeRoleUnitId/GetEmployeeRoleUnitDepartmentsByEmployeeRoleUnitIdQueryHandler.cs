using AutoMapper;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

public class GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleUnitDepartmentRepository employeeRoleUnitDepartmentService)
    : IRequestHandler<GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery,
        IEnumerable<EmployeeRoleUnitDepartmentVm>>
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartmentVm>> Handle(
        GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartments = await employeeRoleUnitDepartmentService
            .GetListByEmployeeRoleUnitId(
                request.EmployeeRoleUnitId,
                cancellationToken);

        return mapper.Map<IEnumerable<EmployeeRoleUnitDepartmentVm>>(employeeRoleUnitDepartments);
    }
}