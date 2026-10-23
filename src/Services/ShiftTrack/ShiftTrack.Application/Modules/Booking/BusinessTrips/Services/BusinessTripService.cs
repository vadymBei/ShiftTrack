using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Constants;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Exceptions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Services;

public class BusinessTripService(
    ICurrentUserService currentUserService,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleChecker employeeRoleChecker,
    IDepartmentRepository departmentRepository,
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

        if (!await CanManage(businessTrip, cancellationToken))
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

        if (!await CanChangeStatus(businessTrip, cancellationToken))
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

        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
        {
            return businessTrips;
        }

        var unitDepartmentIds = await GetCurrentUnitDepartmentIds(cancellationToken);

        return businessTrips
            .Where(x => CanView(x, unitDepartmentIds))
            .ToList();
    }

    public async Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!employeeRoleChecker.HasCurrentUserSysAdminRole()
            && !CanView(businessTrip, await GetCurrentUnitDepartmentIds(cancellationToken)))
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_VIEW_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_VIEW_BUSINESS_TRIP));
        }

        return businessTrip;
    }

    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!await CanManage(businessTrip, cancellationToken))
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

        if (!await CanChangeStatus(businessTrip, cancellationToken))
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP));
        }

        businessTrip.Status = BusinessTripStatus.Approved;

        await businessTripRepository.Update(businessTrip, cancellationToken);
    }

    public Task<IEnumerable<BusinessTrip>> GetMy(CancellationToken cancellationToken)
    {
        return businessTripRepository.GetByParticipantOrAuthor(currentUserService.Employee.Id, cancellationToken);
    }

    public async Task<IEnumerable<BusinessTrip>> GetPendingApprovals(int take, CancellationToken cancellationToken)
    {
        var filter = await GetApprovalFilter(cancellationToken);

        if (filter is null)
            return [];

        return await businessTripRepository.GetForApproval(filter, take, cancellationToken);
    }

    public async Task<int> CountPendingApprovals(CancellationToken cancellationToken)
    {
        var filter = await GetApprovalFilter(cancellationToken);

        if (filter is null)
            return 0;

        return await businessTripRepository.CountForApproval(filter, cancellationToken);
    }

    private async Task<BusinessTripApprovalFilterDto> GetApprovalFilter(CancellationToken cancellationToken)
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return new BusinessTripApprovalFilterDto(null, null);

        if (!employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return null;

        var unitDepartmentIds = await GetCurrentUnitDepartmentIds(cancellationToken);

        if (unitDepartmentIds.Count == 0)
            return null;

        return new BusinessTripApprovalFilterDto(
            unitDepartmentIds.ToList(),
            null);
    }

    private bool IsAuthor(BusinessTrip businessTrip)
        => businessTrip.AuthorId == currentUserService.Employee.Id;

    private bool IsParticipant(BusinessTrip businessTrip)
        => businessTrip.Participants.Any(p => p.Id == currentUserService.Employee.Id);

    private static bool IsInUnit(BusinessTrip businessTrip, IReadOnlySet<long> unitDepartmentIds)
        => businessTrip.Participants.Any(p => p.DepartmentId is { } departmentId
                                              && unitDepartmentIds.Contains(departmentId));

    private bool CanView(BusinessTrip businessTrip, IReadOnlySet<long> unitDepartmentIds)
    {
        if (IsAuthor(businessTrip) || IsParticipant(businessTrip))
            return true;

        return employeeRoleChecker.HasCurrentUserUnitDirectorRole()
               && IsInUnit(businessTrip, unitDepartmentIds);
    }

    private async Task<bool> CanManage(BusinessTrip businessTrip, CancellationToken cancellationToken)
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return true;

        if (employeeRoleChecker.HasCurrentUserUnitDirectorRole()
            && (IsAuthor(businessTrip)
                || IsInUnit(businessTrip, await GetCurrentUnitDepartmentIds(cancellationToken))))
            return true;

        return IsAuthor(businessTrip)
               && businessTrip.Status is BusinessTripStatus.PendingApproval or BusinessTripStatus.Rejected;
    }

    private async Task<bool> CanChangeStatus(BusinessTrip businessTrip, CancellationToken cancellationToken)
    {
        if (employeeRoleChecker.HasCurrentUserSysAdminRole())
            return true;

        return employeeRoleChecker.HasCurrentUserUnitDirectorRole()
               && IsInUnit(businessTrip, await GetCurrentUnitDepartmentIds(cancellationToken));
    }

    private async Task<IReadOnlySet<long>> GetCurrentUnitDepartmentIds(CancellationToken cancellationToken)
    {
        var unitId = currentUserService.Employee.Department?.UnitId;

        if (unitId is null || !employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return new HashSet<long>();

        var departments = await departmentRepository.GetDepartmentsByUnitId(unitId.Value, cancellationToken);

        return departments.Select(x => x.Id).ToHashSet();
    }
}