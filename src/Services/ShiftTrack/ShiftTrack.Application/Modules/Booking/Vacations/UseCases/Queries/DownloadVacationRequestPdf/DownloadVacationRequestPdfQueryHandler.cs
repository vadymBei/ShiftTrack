using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Queries.DownloadVacationRequestPdf;

public class DownloadVacationRequestPdfQueryHandler(
    IVacationService vacationService,
    IEmployeeRepository employeeRepository,
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

        var unitDirector = await GetUnitDirector(vacation.Employee.DepartmentId, cancellationToken);

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

    /// <summary>Regional director whose role bindings cover the employee's department.</summary>
    private async Task<Employee> GetUnitDirector(long? departmentId, CancellationToken cancellationToken)
    {
        if (departmentId is null)
            return null;

        var unitDirectorIds = await employeeRepository.GetIdsByRoleScope(
            DefaultRolesCatalog.UNIT_DIRECTOR, [departmentId.Value], cancellationToken);

        if (unitDirectorIds.Count == 0)
            return null;

        return await employeeRepository.GetById(unitDirectorIds.First(), cancellationToken);
    }
}