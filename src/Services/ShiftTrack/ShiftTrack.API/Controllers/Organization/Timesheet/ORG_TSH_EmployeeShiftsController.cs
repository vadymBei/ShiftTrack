using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Timesheet.EmployeeShifts.Commands.CreateEmployeeShifts;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet/employee-shifts")]
public class ORG_TSH_EmployeeShiftsController : ApiController
{
    [HttpPost("list")]
    public Task<IEnumerable<EmployeeShiftVm>> CreateEmployeeShifts([FromBody] IEnumerable<EmployeeShiftToCreateDto> request)
        =>  Mediator.Invoke(new CreateEmployeeShiftsCommand(request));
}