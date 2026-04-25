using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Common.Interfaces;

public interface IShiftService : IEntityServiceBase<Shift>
{
    Task<IEnumerable<Shift>> GetShiftsByIds(IEnumerable<long> shiftIds, CancellationToken cancellationToken);
    Task<Shift> GetShiftByCode(string code, CancellationToken cancellationToken);
}