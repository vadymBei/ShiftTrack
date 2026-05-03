using AutoMapper;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.ViewModels;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Vacations.UseCases.Commands.CreateVacation;

public class CreateVacationCommandHandler(
    IMapper mapper,
    IVacationRepository vacationRepository,
    IEmployeeRepository employeeRepository) : IRequestHandler<CreateVacationCommand, VacationVm>
{
    public async Task<VacationVm> Handle(CreateVacationCommand request, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository
            .GetById(request.EmployeeId, cancellationToken);

        var vacation = await vacationRepository.Create(
            new VacationToCreateDto(
                request.StartDate,
                request.EndDate,
                request.Comment,
                employee.Id,
                request.Type,
                employee.VacationDaysBalance),
            cancellationToken);

        vacation = await vacationRepository.GetById(vacation.Id, cancellationToken);

        return mapper.Map<VacationVm>(vacation);
    }
}