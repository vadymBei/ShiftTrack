using AutoMapper;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler(
    IMapper mapper,
    IUnitService unitService,
    IDepartmentService departmentService,
    IApplicationDbContext applicationDbContext)
    : IRequestHandler<CreateDepartmentCommand, DepartmentVM>
{
    public async Task<DepartmentVM> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
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

        return mapper.Map<DepartmentVM>(department);
    }
}