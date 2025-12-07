using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShiftHistories.Queries.
    GetEmployeeShiftHistoryByEmployeeShiftId;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet/employee-shift-history")]
public class ORG_TSH_EmployeeShiftHistoryController : ApiController
{
    [HttpGet("by-employeeShiftId/{employeeShiftId}")]
    public Task<IEnumerable<EmployeeShiftHistoryVm>> GetByEmployeeShiftId(long employeeShiftId)
        => Mediator.Invoke(new GetEmployeeShiftHistoryByEmployeeShiftIdQuery(employeeShiftId));
}