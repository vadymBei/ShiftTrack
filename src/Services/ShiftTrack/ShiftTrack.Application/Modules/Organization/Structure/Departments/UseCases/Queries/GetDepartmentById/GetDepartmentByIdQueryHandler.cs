using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler(
    IMapper mapper,
    IDepartmentRepository departmentRepository) : IRequestHandler<GetDepartmentByIdQuery, DepartmentVm>
{
    public async Task<DepartmentVm> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetById(request.Id, cancellationToken);

        return mapper.Map<DepartmentVm>(department);
    }
}