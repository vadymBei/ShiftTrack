using AutoMapper;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(
    IMapper mapper,
    IDepartmentService departmentService) : IRequestHandler<GetDepartmentByIdQuery, DepartmentVM>
{
    public async Task<DepartmentVM> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentService.GetById(request.Id, cancellationToken);

        return mapper.Map<DepartmentVM>(department);
    }
}