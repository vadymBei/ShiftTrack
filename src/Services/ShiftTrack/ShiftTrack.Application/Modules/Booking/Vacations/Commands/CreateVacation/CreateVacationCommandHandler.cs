using AutoMapper;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Common.ViewModels;
using ShiftTrack.Application.Modules.Organization.Employees.Common.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Commands.CreateVacation;

public class CreateVacationCommandHandler(
    IMapper mapper,
    IEmployeeService employeeService,
    IVacationService vacationService,
    IApplicationDbContext applicationDbContext) : IRequestHandler<CreateVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(CreateVacationCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await employeeService
            .GetById(request.EmployeeId, cancellationToken);

        var vacation = new Vacation()
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            EmployeeId = employee.Id,
            Comment = request.Comment,
            Type = request.Type,
            Status = VacationStatus.None,
            DaysBalanceAtCreation = employee.VacationDaysBalance,
            DaysCount = (request.EndDate - request.StartDate).Days + 1
        };
        
        await applicationDbContext.Vacations.AddAsync(vacation, cancellationToken);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        vacation = await vacationService.GetById(vacation.Id, cancellationToken);
        
        return mapper.Map<VacationVm>(vacation);
    }
}