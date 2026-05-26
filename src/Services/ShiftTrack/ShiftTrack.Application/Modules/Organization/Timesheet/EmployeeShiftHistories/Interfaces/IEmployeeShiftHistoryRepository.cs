using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Interfaces;

public interface IEmployeeShiftHistoryRepository
{
    Task Create(IEnumerable<EmployeeShiftHistory> histories, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeShiftHistory>> GetByEmployeeShiftId(long employeeShiftId, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeShiftHistory>> GetByEmployeeShiftIds(IEnumerable<long> employeeShiftIds, CancellationToken cancellationToken);
}