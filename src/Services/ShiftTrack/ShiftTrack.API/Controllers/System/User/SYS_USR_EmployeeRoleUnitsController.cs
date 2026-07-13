using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Commands.CreateEmployeeRoleUnit;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Commands.DeleteEmployeeRoleUnit;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitById;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.UseCases.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-role-units")]
public class SYS_USR_EmployeeRoleUnitsController: ApiController
{
    [HttpPost]
    public Task<EmployeeRoleUnitVm> CreateEmployeeRoleUnit([FromBody] CreateEmployeeRoleUnitCommand command)
        => Mediator.Send(command);

    [HttpGet("{id}")]
    public Task<EmployeeRoleUnitVm> GetEmployeeRoleUnitById(long id)
        => Mediator.Send(new GetEmployeeRoleUnitByIdQuery(id)); 
    
    [HttpGet("by-employeeRoleId/{employeeRoleId}")]
    public Task<IEnumerable<EmployeeRoleUnitVm>> GetEmployeeRoleUnitsByEmployeeRoleId(long employeeRoleId)
        => Mediator.Send(new GetEmployeeRoleUnitsByEmployeeRoleIdQuery(employeeRoleId));
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRoleUnit(long id)
    {
        await Mediator.Send(new DeleteEmployeeRoleUnitCommand(id));
        
        return Ok();
    }
}