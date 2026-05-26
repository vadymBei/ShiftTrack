using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetGroupedDepartmentsByUnit;

public class GetGroupedDepartmentsByUnitQueryHandler(
    IMapper mapper,
    IDepartmentService departmentService)
    : IRequestHandler<GetGroupedDepartmentsByUnitQuery, IEnumerable<GroupedDepartmentsByUnitVm>>
{
    public async Task<IEnumerable<GroupedDepartmentsByUnitVm>> Handle(GetGroupedDepartmentsByUnitQuery request,
        CancellationToken cancellationToken)
    {
        var groupedDepartments = await departmentService
            .GetGroupedDepartmentsByUnit(cancellationToken);

        return mapper.Map<List<GroupedDepartmentsByUnitVm>>(groupedDepartments);
    }
}