using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.UseCases.Commands.CreateEmployeeShifts;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Timesheet;

[Authorize]
[Route("api/shift-track/timesheet/employee-shifts")]
public class ORG_TSH_EmployeeShiftsController : ApiController
{
    [HttpPost("list")]
    public Task<IEnumerable<EmployeeShiftVm>> CreateEmployeeShifts([FromBody] IEnumerable<EmployeeShiftToCreateDto> request)
        =>  Mediator.Send(new CreateEmployeeShiftsCommand(request));
}