using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.ExportUnitTimesheet;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.UseCases.Queries.GetUnitTimesheet;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet")]
public class ORG_TSH_TimesheetController : ApiController
{
    [HttpGet]
    public Task<UnitTimesheetVm> GetTimesheet([FromQuery] UnitTimesheetDto request)
        => Mediator.Invoke(new GetTimesheetQuery(request));

    [HttpGet("export")]
    public async Task<IActionResult> ExportTimesheet([FromQuery] ExportUnitTimesheetQuery query)
    {
        var document = await Mediator.Invoke(query);
        
        return File(document.Content, document.MimeType, document.Name);
    }
}