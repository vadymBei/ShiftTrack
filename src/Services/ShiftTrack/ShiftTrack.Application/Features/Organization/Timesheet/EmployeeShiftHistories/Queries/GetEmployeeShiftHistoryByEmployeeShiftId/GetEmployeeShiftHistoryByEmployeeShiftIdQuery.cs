using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShiftHistories.Queries.GetEmployeeShiftHistoryByEmployeeShiftId;

public record GetEmployeeShiftHistoryByEmployeeShiftIdQuery(
    long EmployeeShiftId) : IRequest<IEnumerable<EmployeeShiftHistoryVm>>;