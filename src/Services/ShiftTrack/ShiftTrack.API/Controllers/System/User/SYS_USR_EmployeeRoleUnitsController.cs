using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Commands.CreateEmployeeRoleUnit;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Commands.DeleteEmployeeRoleUnit;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitById;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnits.Queries.GetEmployeeRoleUnitsByEmployeeRoleId;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-role-units")]
public class SYS_USR_EmployeeRoleUnitsController: ApiController
{
    [HttpPost]
    public Task<EmployeeRoleUnitVm> CreateEmployeeRoleUnit([FromBody] CreateEmployeeRoleUnitCommand command)
        => Mediator.Invoke(command);

    [HttpGet("{id}")]
    public Task<EmployeeRoleUnitVm> GetEmployeeRoleUnitById(long id)
        => Mediator.Invoke(new GetEmployeeRoleUnitByIdQuery(id)); 
    
    [HttpGet("by-employeeRoleId/{employeeRoleId}")]
    public Task<IEnumerable<EmployeeRoleUnitVm>> GetEmployeeRoleUnitsByEmployeeRoleId(long employeeRoleId)
        => Mediator.Invoke(new GetEmployeeRoleUnitsByEmployeeRoleIdQuery(employeeRoleId));
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRoleUnit(long id)
    {
        await Mediator.Invoke(new DeleteEmployeeRoleUnitCommand(id));
        
        return Ok();
    }
}