using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(
    IMapper mapper,
    IUnitService unitService,
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateDepartmentCommand, DepartmentVm>
{
    public async Task<DepartmentVm> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        await unitService.GetById(request.UnitId, cancellationToken);

        var department = new Department()
        {
            Name = request.Name,
            UnitId = request.UnitId
        };

        await applicationDbContext.Departments.AddAsync(department, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        department = await departmentService.GetById(department.Id, cancellationToken);

        return mapper.Map<DepartmentVm>(department);
    }
}