using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;
using ShiftTrack.Application.Modules.Organization.Payrolls.UseCases.Commands.MarkPayrollAsPaid;
using ShiftTrack.Application.Modules.Organization.Payrolls.UseCases.Queries.GetPayrolls;
using ShiftTrack.Application.Modules.Organization.Payrolls.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Organization.Payrolls;

[Authorize]
[Route("api/shift-track/organization/payrolls")]
public class ORG_PRL_PayrollsController : ApiController
{
    [HttpPut("mark-as-paid")]
    public async Task<IActionResult> MarkPayrollAsPaid([FromBody] MarkPayrollAsPaidDto request)
    {
        await Mediator.Send(new MarkPayrollAsPaidCommand(request));

        return Ok();
    }

    [HttpGet("by-period")]
    public async Task<PayrollSummaryVm> GetPayrollsByPeriod(DateTime period, long departmentId)
        => await Mediator.Send(new GetPayrollsQuery(period, departmentId));
}