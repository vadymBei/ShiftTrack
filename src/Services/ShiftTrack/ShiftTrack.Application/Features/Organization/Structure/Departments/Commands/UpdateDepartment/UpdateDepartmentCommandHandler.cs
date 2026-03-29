using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateDepartmentCommand, DepartmentVm>
{
    public async Task<DepartmentVm> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (department == null)
            throw new EntityNotFoundException(typeof(Department), request.Id);

        department.Name = request.Name;

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<DepartmentVm>(department);
    }
}