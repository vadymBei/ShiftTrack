using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByRoles;

public class GetDepartmentsByRolesQueryHandler(
    IMapper mapper,
    IDepartmentService departmentService) : IRequestHandler<GetDepartmentsByRolesQuery, IEnumerable<DepartmentVm>>
{
    public async Task<IEnumerable<DepartmentVm>> Handle(GetDepartmentsByRolesQuery request,
        CancellationToken cancellationToken = default)
    {
        var departments = await departmentService.GetDepartmentsByRoles(
            request.UnitId,
            cancellationToken);

        return mapper.Map<IEnumerable<DepartmentVm>>(departments);
    }
}