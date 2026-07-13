using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.System.User.Common.ViewModels;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Commands.CreateEmployeeRoleUnitDepartment;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Commands.DeleteEmployeeRoleUnitDepartment;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnitDepartments.UseCases.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-role-unit-departments")]
public class SYS_USR_EmployeeRoleUnitDepartmentsController : ApiController
{
    [HttpPost]
    public Task<EmployeeRoleUnitDepartmentVm> CreateEmployeeRoleUnitDepartment([FromBody] CreateEmployeeRoleUnitDepartmentCommand command)
        => Mediator.Send(command);
    
    [HttpGet("by-employeeRoleUnitId/{employeeRoleUnitId}")]
    public Task<IEnumerable<EmployeeRoleUnitDepartmentVm>> GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId(long employeeRoleUnitId)
        => Mediator.Send(new GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery(employeeRoleUnitId));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRoleUnitDepartment(long id)
    {
        await Mediator.Send(new DeleteEmployeeRoleUnitDepartmentCommand(id));
        
        return Ok();
    }
}