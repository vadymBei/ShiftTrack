using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Kernel.Exceptions;

using UnitEntity = ShiftTrack.Core.Domain.Organization.Structure.Entities.Unit;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit
{
    public class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteUnitCommandHandler(
            IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
        {
            var unit = await _dbContext.Units
                .Include(x => x.Departments)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (unit == null)
                throw new EntityNotFoundException(typeof(UnitEntity), request.Id);

            _dbContext.Departments.RemoveRange(unit.Departments);
            _dbContext.Units.Remove(unit);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
