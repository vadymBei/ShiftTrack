using ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShiftHistories.Queries.GetEmployeeShiftHistoryByEmployeeShiftId;

public record GetEmployeeShiftHistoryByEmployeeShiftIdQuery(
    long EmployeeShiftId) : IRequest<IEnumerable<EmployeeShiftHistoryVm>>;