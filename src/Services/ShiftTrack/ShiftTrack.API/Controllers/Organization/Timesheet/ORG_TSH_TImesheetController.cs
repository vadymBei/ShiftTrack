using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Timesheet.UnitTimesheets.Queries.GetTimesheet;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet")]
public class ORG_TSH_TImesheetController : ApiController
{
    [HttpGet]
    public Task<TimesheetVm> GetTimesheet([FromQuery] TimesheetDto request)
        => Mediator.Invoke(new GetTimesheetQuery(request));
}