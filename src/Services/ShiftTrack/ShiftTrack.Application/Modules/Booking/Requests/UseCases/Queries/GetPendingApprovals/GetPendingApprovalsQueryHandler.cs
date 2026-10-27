using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.Requests.Enums;
using ShiftTrack.Application.Modules.Booking.Requests.ViewModels;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetPendingApprovals;

public class GetPendingApprovalsQueryHandler(
    IVacationService vacationService,
    IBusinessTripService businessTripService,
    IBusinessTripRouteService businessTripRouteService) : IRequestHandler<GetPendingApprovalsQuery, IEnumerable<PendingApprovalVm>>
{
    public async Task<IEnumerable<PendingApprovalVm>> Handle(GetPendingApprovalsQuery request,
        CancellationToken cancellationToken = default)
    {
        var take = Math.Clamp(request.Take, 1, 50);

        var vacations = await vacationService.GetPendingApprovals(take, cancellationToken);
        var businessTrips = (await businessTripService.GetPendingApprovals(take, cancellationToken)).ToList();
        var routes = await businessTripRouteService.GetRoutes(businessTrips, cancellationToken);

        var vacationItems = vacations.Select(x => new PendingApprovalVm
        {
            Id = x.Id,
            Kind = BookingRequestKind.Vacation,
            VacationType = x.Type,
            Employee = ToEmployeeVm(x.Employee),
            ParticipantsCount = 1,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            CreatedAt = x.CreatedAt
        });

        var businessTripItems = businessTrips.Select(x => new PendingApprovalVm
        {
            Id = x.Id,
            Kind = BookingRequestKind.BusinessTrip,
            Route = routes.TryGetValue(x.Id, out var route) ? route : null,
            Employee = ToEmployeeVm(x.Participants?
                .OrderBy(p => p.Surname)
                .ThenBy(p => p.Name)
                .FirstOrDefault()),
            ParticipantsCount = x.Participants?.Count ?? 0,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            CreatedAt = x.CreatedAt
        });

        return vacationItems
            .Concat(businessTripItems)
            .OrderBy(x => x.CreatedAt)
            .Take(take)
            .ToList();
    }

    private static PendingApprovalEmployeeVm ToEmployeeVm(Employee employee)
    {
        if (employee is null)
            return null;

        return new PendingApprovalEmployeeVm
        {
            Id = employee.Id,
            FullName = employee.FullName,
            PositionName = employee.Position?.Name
        };
    }
}
