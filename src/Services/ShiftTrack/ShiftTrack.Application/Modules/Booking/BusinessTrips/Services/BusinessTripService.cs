using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Services;

public class BusinessTripService(
    IEmployeeRepository employeeRepository,
    IBusinessTripRepository businessTripRepository) : IBusinessTripService
{
    public async Task<BusinessTrip> Create(BusinessTripToCreateDto dto, CancellationToken cancellationToken)
    {
        var employees = await employeeRepository
            .GetListByIds(dto.EmployeeIds, cancellationToken);

        var businessTrip = new BusinessTrip
        {
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Description = dto.Description,
            EstimatedBudget = dto.EstimatedBudget,
            Status = BusinessTripStatus.PendingApproval,
            Participants = employees.ToList(),
            Locations = dto.LocationIntegrationIds
                .Select(integrationId =>
                    new BusinessTripLocation
                    {
                        LocationIntegrationId = integrationId
                    })
                .ToList()
        };

        businessTrip = await businessTripRepository
            .Create(businessTrip, cancellationToken);

        return businessTrip;
    }
}