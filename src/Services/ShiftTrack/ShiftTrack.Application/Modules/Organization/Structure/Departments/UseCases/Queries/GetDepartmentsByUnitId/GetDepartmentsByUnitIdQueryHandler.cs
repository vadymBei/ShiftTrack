using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByUnitId;

public class GetDepartmentsByUnitIdQueryHandler(
    IMapper mapper,
    IUnitRepository unitRepository,
    IDepartmentRepository departmentRepository)
    : IRequestHandler<GetDepartmentsByUnitIdQuery, IEnumerable<DepartmentVm>>
{
    public async Task<IEnumerable<DepartmentVm>> Handle(GetDepartmentsByUnitIdQuery request,
        CancellationToken cancellationToken)
    {
        await unitRepository
            .GetById(request.UnitId, cancellationToken);

        var departments = await departmentRepository.GetDepartmentsByUnitId(
            request.UnitId,
            cancellationToken);

        return mapper.Map<IEnumerable<DepartmentVm>>(departments);
    }
}