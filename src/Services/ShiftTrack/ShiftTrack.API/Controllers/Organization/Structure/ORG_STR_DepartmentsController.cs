using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.DeleteDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;
using ShiftTrack.Kernel.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Structure
{
    [Authorize]
    [Route("api/shift-track/organization/structure/departments")]
    public class ORG_STR_DepartmentsController : ApiController
    {
        [HttpPost]
        public async Task<DepartmentVM> Create(CreateDepartmentCommand command)
            => await Mediator.Send(command);

        [HttpPut]
        public async Task<DepartmentVM> UpdateDepartment(UpdateDepartmentCommand command)
            => await Mediator.Send(command);

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            await Mediator.Send(new DeleteDepartmentCommand(id));

            return Ok();
        }

        [HttpGet("by-unitId/{unitId}")]
        public async Task<IEnumerable<DepartmentVM>> GetDepartments(long unitId)
            => await Mediator.Send(new GetDepartmentsByUnitIdQuery(unitId));

        [HttpGet("by-id/{id}")]
        public async Task<DepartmentVM> GetDepartmentById(long id)
            => await Mediator.Send(new GetDepartmentByIdQuery(id));
    }
}
