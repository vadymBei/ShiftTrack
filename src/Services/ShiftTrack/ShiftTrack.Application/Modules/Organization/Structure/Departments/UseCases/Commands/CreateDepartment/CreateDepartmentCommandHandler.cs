using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(
    IMapper mapper,
    IUnitRepository unitRepository,
    IDepartmentRepository departmentRepository)
    : IRequestHandler<CreateDepartmentCommand, DepartmentVm>
{
    public async Task<DepartmentVm> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        await unitRepository.GetById(request.Data.UnitId, cancellationToken);

        var department = await departmentRepository.Create(request.Data, cancellationToken);

        department = await departmentRepository.GetById(department.Id, cancellationToken);

        return mapper.Map<DepartmentVm>(department);
    }
}