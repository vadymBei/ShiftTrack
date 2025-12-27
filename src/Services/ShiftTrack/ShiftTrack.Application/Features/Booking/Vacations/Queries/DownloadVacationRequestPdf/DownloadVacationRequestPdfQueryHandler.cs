using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Constants;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Booking.Vacations.Queries.DownloadVacationRequestPdf;

public class DownloadVacationRequestPdfQueryHandler(
    IPdfGenerator pdfGenerator,
    IVacationService vacationService,
    IEmployeeRoleService employeeRoleService,
    IPdfFormatter<VacationRequestData> vacationRequestFormatter) : IRequestHandler<DownloadVacationRequestPdfQuery, DocumentVm>
{
    public async Task<DocumentVm> Handle(DownloadVacationRequestPdfQuery request, CancellationToken cancellationToken = default)
    {
        var vacation = await vacationService
            .GetById(request.VacationId, cancellationToken);

        var employeeRoleUnits = await employeeRoleService
            .GetEmployeeRoleUnitsByUnitId((long)vacation.Employee.Department.UnitId, cancellationToken);
        
        var unitDirector = employeeRoleUnits
            .FirstOrDefault(x => x.EmployeeRole.Role.Name == DefaultRolesCatalog.UNIT_DIRECTOR)
            ?.EmployeeRole?.Employee;
        
        var streamContent = await pdfGenerator.Generate(
            vacationRequestFormatter,
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