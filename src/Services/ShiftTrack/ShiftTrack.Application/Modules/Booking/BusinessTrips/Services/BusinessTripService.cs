using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Constants;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Exceptions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Services;

public class BusinessTripService(
    ICurrentUserService currentUserService,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleChecker employeeRoleChecker,
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

    public async Task<BusinessTrip> Update(BusinessTripToUpdateDto dto, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(dto.Id, cancellationToken);

        if (businessTrip.AuthorId == currentUserService.Employee.Id
            && businessTrip.Status != BusinessTripStatus.PendingApproval
            && businessTrip.Status != BusinessTripStatus.Rejected
            && !employeeRoleChecker.HasCurrentUserSysAdminRole()
            && !employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_UPDATE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_UPDATE_BUSINESS_TRIP));
        }

        businessTrip.StartDate = dto.StartDate;
        businessTrip.EndDate = dto.EndDate;
        businessTrip.Description = dto.Description;
        businessTrip.EstimatedBudget = dto.EstimatedBudget;

        businessTrip.Participants.Clear();
        var employees = await employeeRepository.GetListByIds(dto.EmployeeIds, cancellationToken);
        businessTrip.Participants = employees.ToList();

        businessTrip.Locations.Clear();
        businessTrip.Locations = dto.LocationIntegrationIds
            .Select(integrationId =>
                new BusinessTripLocation
                {
                    LocationIntegrationId = integrationId
                })
            .ToList();

        await businessTripRepository.Update(businessTrip, cancellationToken);

        businessTrip = await businessTripRepository.GetById(businessTrip.Id, cancellationToken);

        return businessTrip;
    }

    public async Task Reject(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!CanUpdateBusinessTripStatus(businessTrip))
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP));
        }
        
        businessTrip.Status = BusinessTripStatus.Rejected;

        await businessTripRepository.Update(businessTrip, cancellationToken);
    }

    public async Task<IEnumerable<BusinessTrip>> GetFiltered(BusinessTripFilterDto filter,
        CancellationToken cancellationToken)
    {
        var businessTrips = await businessTripRepository
            .GetFiltered(filter, cancellationToken);

        if (!employeeRoleChecker.HasCurrentUserSysAdminRole()
            && !employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            businessTrips = businessTrips.Where(x => x.AuthorId == currentUserService.Employee.Id
                                                     || x.Participants.Any(p =>
                                                         p.Id == currentUserService.Employee.Id));
        }

        return businessTrips;
    }

    public Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken)
    {
        return businessTripRepository.GetById(id, cancellationToken);
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (businessTrip.AuthorId == currentUserService.Employee.Id
            && businessTrip.Status != BusinessTripStatus.PendingApproval
            && businessTrip.Status != BusinessTripStatus.Rejected
            && !employeeRoleChecker.HasCurrentUserSysAdminRole()
            && !employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_DELETE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_DELETE_BUSINESS_TRIP));
        }

        await businessTripRepository.Delete(id, cancellationToken);
    }

    public async Task Approve(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!CanUpdateBusinessTripStatus(businessTrip))
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP));
        }
        
        businessTrip.Status = BusinessTripStatus.Approved;

        await businessTripRepository.Update(businessTrip, cancellationToken);
    }
    
    private bool CanUpdateBusinessTripStatus(BusinessTrip businessTrip)
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return true;

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole())
        {
            var isAuthor = businessTrip.AuthorId == currentUserService.Employee.Id;
            var isInSameDepartment = businessTrip.Participants
                .Any(p => p.DepartmentId == currentUserService.Employee.DepartmentId);
        
            return isAuthor || isInSameDepartment;
        }

        return false;
    }
}