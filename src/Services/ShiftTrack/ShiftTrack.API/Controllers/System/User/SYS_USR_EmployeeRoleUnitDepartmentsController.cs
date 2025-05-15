using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.CreateEmployeeRoleUnitDepartment;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Commands.DeleteEmployeeRoleUnitDepartment;
using ShiftTrack.Core.Application.System.User.EmployeeRoleUnitDepartments.Queries.GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.System.User;

[Authorize]
[Route("api/shift-track/system/user/employee-role-unit-departments")]
public class SYS_USR_EmployeeRoleUnitDepartmentsController : ApiController
{
    [HttpPost]
    public Task<EmployeeRoleUnitDepartmentVm> CreateEmployeeRoleUnitDepartment([FromBody] CreateEmployeeRoleUnitDepartmentCommand command)
        => Mediator.Invoke(command);
    
    [HttpGet("by-employeeRoleUnitId/{employeeRoleUnitId}")]
    public Task<IEnumerable<EmployeeRoleUnitDepartmentVm>> GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitId(long employeeRoleUnitId)
        => Mediator.Invoke(new GetEmployeeRoleUnitDepartmentsByEmployeeRoleUnitIdQuery(employeeRoleUnitId));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeRoleUnitDepartment(long id)
    {
        await Mediator.Invoke(new DeleteEmployeeRoleUnitDepartmentCommand(id));
        
        return Ok();
    }
}