using Kernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Common.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ShiftService(
            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Shift> GetById(object id, CancellationToken cancellationToken)
        {
            var shift = await _applicationDbContext.Shifts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == (long)id, cancellationToken);

            if (shift == null)
            {
                throw new EntityNotFoundException(typeof(Shift), (long)id);
            }

            return shift;
        }
    }
}
