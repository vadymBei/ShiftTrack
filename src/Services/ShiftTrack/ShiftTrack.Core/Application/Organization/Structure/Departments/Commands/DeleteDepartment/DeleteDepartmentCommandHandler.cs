using Kernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteDepartmentCommandHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MediatR.Unit> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _dbContext.Departments
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (department == null)
                throw new EntityNotFoundException(typeof(Department), request.Id);

            _dbContext.Departments.Remove(department);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return MediatR.Unit.Value;
        }
    }
}
