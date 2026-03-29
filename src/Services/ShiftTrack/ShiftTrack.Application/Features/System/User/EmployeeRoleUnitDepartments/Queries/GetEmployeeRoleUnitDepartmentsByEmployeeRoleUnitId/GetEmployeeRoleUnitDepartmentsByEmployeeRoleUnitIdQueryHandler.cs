using AutoMapper;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.User.EmployeeRoleUnitDepartments.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

public class GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQueryHandler(
    IMapper mapper,
    IEmployeeRoleService employeeRoleService)
    : IRequestHandler<GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery,
        IEnumerable<EmployeeRoleUnitDepartmentVm>>
{
    public async Task<IEnumerable<EmployeeRoleUnitDepartmentVm>> Handle(
        GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery request, CancellationToken cancellationToken)
    {
        var employeeRoleUnitDepartments = await employeeRoleService
            .GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId(
                request.EmployeeRoleUnitId,
                cancellationToken);

        return mapper.Map<IEnumerable<EmployeeRoleUnitDepartmentVm>>(employeeRoleUnitDepartments);
    }
}