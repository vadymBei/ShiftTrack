using Kernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.Services
{
    public class PositionService : IPositionService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public PositionService(
            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Position> GetById(object id, CancellationToken cancellationToken)
        {
            var position = await _applicationDbContext.Positions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == (long)id, cancellationToken);

            if (position is null)
            {
                throw new EntityNotFoundException(typeof(Position), (long)id);
            }

            return position;
        }
    }
}
