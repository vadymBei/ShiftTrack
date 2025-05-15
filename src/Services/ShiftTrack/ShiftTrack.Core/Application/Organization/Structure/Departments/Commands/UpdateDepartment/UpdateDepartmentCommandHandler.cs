using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<UpdateDepartmentCommand, DepartmentVM>
{
    public async Task<DepartmentVM> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (department == null)
            throw new EntityNotFoundException(typeof(Department), request.Id);

        department.Name = request.Name;

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<DepartmentVM>(department);
    }
}