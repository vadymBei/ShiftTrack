using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (department == null)
            throw new EntityNotFoundException(typeof(Department), request.Id);

        dbContext.Departments.Remove(department);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}