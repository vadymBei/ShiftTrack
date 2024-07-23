using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Core.Application.System.Auth.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Application.System.User.Employees.Commands.ChangePassword;
using ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateEmployee;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployeeById;
using ShiftTrack.Core.Application.System.User.Employees.Queries.GetEmployees;
using ShiftTrack.Kernel.Controllers;

namespace ShiftTrack.API.Controllers.System.User
{
    [Authorize]
    [Route("api/shift-track/system/user/employees")]
    public class SYS_USR_EmployeesController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<EmployeeVM> CreateEmployee(CreateEmployeeCommand command)
            => await Mediator.Send(command);

        [HttpPut]
        public async Task<EmployeeVM> UpdateEmployee(UpdateEmployeeCommand command)
            => await Mediator.Send(command);

        [HttpGet]
        public async Task<IEnumerable<EmployeeVM>> GetEmployees([FromQuery] GetEmployeesQuery query)
            => await Mediator.Send(query);

        [HttpGet("{id}")]
        public async Task<EmployeeVM> GetEmployeeById(long id)
            => await Mediator.Send(new GetEmployeeByIdQuery(id));

        [HttpPost("change-password")]
        public async Task<TokenVM> ChangePassword(ChangeEmployeePasswordDto commandData)
            => await Mediator.Send(new ChangePasswordCommand(commandData));
    }
}
