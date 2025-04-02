using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteDepartmentCommand>
{
    public async Task<MediatR.Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (department == null)
            throw new EntityNotFoundException(typeof(Department), request.Id);

        dbContext.Departments.Remove(department);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MediatR.Unit.Value;
    }
}