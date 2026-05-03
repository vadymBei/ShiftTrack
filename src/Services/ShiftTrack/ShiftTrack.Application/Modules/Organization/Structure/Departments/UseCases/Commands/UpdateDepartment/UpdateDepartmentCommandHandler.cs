using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler(
    IMapper mapper,
    IDepartmentRepository departmentRepository)
    : IRequestHandler<UpdateDepartmentCommand, DepartmentVm>
{
    public async Task<DepartmentVm> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.Update(request.Data, cancellationToken);

        return mapper.Map<DepartmentVm>(department);
    }
}