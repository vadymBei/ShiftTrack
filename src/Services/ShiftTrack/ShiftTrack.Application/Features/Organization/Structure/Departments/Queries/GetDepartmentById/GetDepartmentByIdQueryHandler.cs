using AutoMapper;
using ShiftTrack.Application.Features.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(
    IMapper mapper,
    IDepartmentService departmentService) : IRequestHandler<GetDepartmentByIdQuery, DepartmentVm>
{
    public async Task<DepartmentVm> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetById(request.Id, cancellationToken);

        return mapper.Map<DepartmentVm>(department);
    }
}