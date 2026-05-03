using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.ApproveVacation;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.CreateVacation;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.DeleteVacation;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.RejectVacation;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.UpdateVacation;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.DownloadVacationRequestPdf;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacationById;
using ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.GetVacations;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Kernel.CQRS.Controllers;

namespace ShiftTrack.API.Controllers.Booking.Vacations;

[Authorize]
[Route("api/shift-track/booking/vacations")]
public class BKG_VCN_VacationsController : ApiController
{
    [HttpPost]
    public Task<VacationVm> CreateVacation(CreateVacationCommand command)
        => Mediator.Invoke(command);

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVacation(long id)
    {
        await Mediator.Invoke(new DeleteVacationCommand(id));

        return Ok();
    }

    [HttpGet]
    public Task<IEnumerable<VacationVm>> GetVacations([FromQuery] VacationsFilterDto filter)
        => Mediator.Invoke(new GetVacationsQuery(filter));

    [HttpGet("{id}")]
    public Task<VacationVm> GetVacationById(long id)
        => Mediator.Invoke(new GetVacationByIdQuery(id));

    [HttpPut]
    public Task<VacationVm> UpdateVacation([FromBody] UpdateVacationCommand command)
        => Mediator.Invoke(command);

    [HttpPut("approve/{id}")]
    public Task<VacationVm> ApproveVacation(long id)
        => Mediator.Invoke(new ApproveVacationCommand(id));

    [HttpPut("reject/{id}")]
    public Task<VacationVm> RejectVacation(long id)
        => Mediator.Invoke(new RejectVacationCommand(id));

    [HttpGet("download/request/pdf/{id}")]
    public async Task<FileResult> DownloadVacationRequestPdfQuery(long id)
    {
        var documentVm = await Mediator.Invoke(new DownloadVacationRequestPdfQuery(id));

        return File(documentVm.StreamContent, documentVm.MimeType, documentVm.Name);
    }
}