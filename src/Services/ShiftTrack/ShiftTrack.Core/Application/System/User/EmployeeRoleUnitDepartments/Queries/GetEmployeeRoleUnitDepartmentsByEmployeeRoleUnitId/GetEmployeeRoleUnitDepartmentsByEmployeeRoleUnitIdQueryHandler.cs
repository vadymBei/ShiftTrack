using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;

namespace ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;

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