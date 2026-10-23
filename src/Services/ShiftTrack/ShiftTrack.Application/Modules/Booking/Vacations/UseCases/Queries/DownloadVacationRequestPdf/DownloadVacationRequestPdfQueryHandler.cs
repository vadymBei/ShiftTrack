using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoleUnits.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.DownloadVacationRequestPdf;

public class DownloadVacationRequestPdfQueryHandler(
    IVacationService vacationService,
    IEmployeeRoleUnitRepository employeeRoleUnitRepository,
    IPdfExporter<VacationRequestData> vacationRequestExporter)
    : IRequestHandler<DownloadVacationRequestPdfQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(DownloadVacationRequestPdfQuery request,
        CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService
            .GetById(request.VacationId, cancellationToken);

        if (vacation.Status is not VacationStatus.ApprovedByUnitDirector)
        {
            throw new VacationException(
                VacationExceptionsLocalization.CANNOT_DOWNLOAD_NOT_APPROVED_VACATION,
                nameof(VacationExceptionsLocalization.CANNOT_DOWNLOAD_NOT_APPROVED_VACATION));
        }

        var employeeRoleUnits = await employeeRoleUnitRepository
            .GetListByUnitId((long)vacation.Employee.Department.UnitId, cancellationToken);

        var unitDirector = employeeRoleUnits
            .FirstOrDefault(x => x.EmployeeRole.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR)
            ?.EmployeeRole?.Employee;

        var streamContent = await vacationRequestExporter.Export(
            new VacationRequestData()
            {
                UnitDirector = unitDirector,
                Vacation = vacation
            },
            cancellationToken);

        return new DocumentVm()
        {
            StreamContent = streamContent,
            Extension = ".pdf",
            Name = $"{vacation.Employee.FullName} vacation request"
        };
    }
}