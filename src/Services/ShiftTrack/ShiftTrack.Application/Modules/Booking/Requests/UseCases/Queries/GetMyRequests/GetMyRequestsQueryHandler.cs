using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.Requests.Enums;
using ShiftTrack.Application.Modules.Booking.Requests.ViewModels;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.Requests.UseCases.Queries.GetMyRequests;

public class GetMyRequestsQueryHandler(
    IVacationService vacationService,
    IBusinessTripService businessTripService,
    IBusinessTripRouteService businessTripRouteService) : IRequestHandler<GetMyRequestsQuery, MyBookingRequestsVm>
{
    public async Task<MyBookingRequestsVm> Handle(GetMyRequestsQuery request, CancellationToken cancellationToken = default)
    {
        var take = Math.Clamp(request.Take, 1, 50);
        var today = DateTime.Today;

        var vacations = (await vacationService.GetMy(cancellationToken)).ToList();
        var businessTrips = (await businessTripService.GetMy(cancellationToken)).ToList();
        var routes = await businessTripRouteService.GetRoutes(businessTrips, cancellationToken);

        var vacationItems = vacations.Select(x => new
        {
            IsApproved = x.Status is VacationStatus.ApprovedByDepartmentDirector or VacationStatus.ApprovedByUnitDirector,
            Vm = new BookingRequestVm
            {
                Id = x.Id,
                Kind = BookingRequestKind.Vacation,
                VacationType = x.Type,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status.ToString(),
                CreatedAt = x.CreatedAt
            }
        });

        var businessTripItems = businessTrips.Select(x => new
        {
            IsApproved = x.Status is BusinessTripStatus.Approved,
            Vm = new BookingRequestVm
            {
                Id = x.Id,
                Kind = BookingRequestKind.BusinessTrip,
                Route = routes.TryGetValue(x.Id, out var route) ? route : null,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status.ToString(),
                CreatedAt = x.CreatedAt
            }
        });

        var all = vacationItems.Concat(businessTripItems).ToList();

        return new MyBookingRequestsVm
        {
            UpcomingAbsence = all
                .Where(x => x.IsApproved && x.Vm.EndDate.Date >= today)
                .OrderBy(x => x.Vm.StartDate)
                .Select(x => x.Vm)
                .FirstOrDefault(),
            Items = all
                .Select(x => x.Vm)
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .ToList()
        };
    }
}
