using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Constants;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Dtos;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Events;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Exceptions;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.Interfaces;
using ShiftTrack.Application.Modules.Booking.BusinessTrips.ViewModels;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Interfaces;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Booking.BusinessTrips.Services;

public class BusinessTripService(
    ICurrentUserService currentUserService,
    IEmployeeRepository employeeRepository,
    IEmployeeRoleChecker employeeRoleChecker,
    IDepartmentRepository departmentRepository,
    IBusinessTripRepository businessTripRepository,
    IMediator mediator) : IBusinessTripService
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

        await mediator.Publish(new BusinessTripCreatedEvent(businessTrip.Id), cancellationToken);

        return businessTrip;
    }

    public async Task<BusinessTrip> Update(BusinessTripToUpdateDto dto, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(dto.Id, cancellationToken);

        if (!(await GetPermissions(businessTrip, cancellationToken)).CanEdit)
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_UPDATE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_UPDATE_BUSINESS_TRIP));
        }

        var updatedEvent = new BusinessTripUpdatedEvent(
            businessTrip.Id,
            (businessTrip.Participants ?? []).Select(x => x.Id).ToList(),
            (businessTrip.Locations ?? []).OrderBy(x => x.Id).Select(x => x.LocationIntegrationId).ToList(),
            businessTrip.StartDate,
            businessTrip.EndDate);

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

        await mediator.Publish(updatedEvent, cancellationToken);

        return businessTrip;
    }

    public async Task Reject(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!(await GetPermissions(businessTrip, cancellationToken)).CanReject)
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_REJECT_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_REJECT_BUSINESS_TRIP));
        }

        var oldStatus = businessTrip.Status;

        businessTrip.Status = BusinessTripStatus.Rejected;

        await businessTripRepository.Update(businessTrip, cancellationToken);

        await PublishStatusChanged(businessTrip.Id, oldStatus, businessTrip.Status, cancellationToken);
    }

    public async Task<IEnumerable<BusinessTrip>> GetFiltered(BusinessTripFilterDto filter,
        CancellationToken cancellationToken)
    {
        var businessTrips = await businessTripRepository
            .GetFiltered(filter, cancellationToken);

        var context = await GetAccessContext(cancellationToken);

        return businessTrips
            .Where(x => CanView(x, context))
            .ToList();
    }

    public async Task<BusinessTrip> GetById(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!CanView(businessTrip, await GetAccessContext(cancellationToken)))
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

        if (!(await GetPermissions(businessTrip, cancellationToken)).CanDelete)
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_DELETE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_DELETE_BUSINESS_TRIP));
        }

        var deletedEvent = new BusinessTripDeletedEvent(
            businessTrip.Id,
            businessTrip.AuthorId,
            (businessTrip.Participants ?? []).Select(x => x.Id).ToList(),
            (businessTrip.Locations ?? []).OrderBy(x => x.Id).Select(x => x.LocationIntegrationId).ToList(),
            businessTrip.StartDate,
            businessTrip.EndDate);

        await businessTripRepository.Delete(id, cancellationToken);

        await mediator.Publish(deletedEvent, cancellationToken);
    }

    public async Task Approve(long id, CancellationToken cancellationToken)
    {
        var businessTrip = await businessTripRepository.GetById(id, cancellationToken);

        if (!(await GetPermissions(businessTrip, cancellationToken)).CanApprove)
        {
            throw new BusinessTripException(
                BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP,
                nameof(BusinessTripExceptionsLocalization.CAN_NOT_APPROVE_BUSINESS_TRIP));
        }

        var oldStatus = businessTrip.Status;

        businessTrip.Status = BusinessTripStatus.Approved;

        await businessTripRepository.Update(businessTrip, cancellationToken);

        await PublishStatusChanged(businessTrip.Id, oldStatus, businessTrip.Status, cancellationToken);
    }

    private async Task PublishStatusChanged(long businessTripId, BusinessTripStatus oldStatus,
        BusinessTripStatus newStatus, CancellationToken cancellationToken)
    {
        if (oldStatus == newStatus)
            return;

        await mediator.Publish(
            new BusinessTripStatusChangedEvent(businessTripId, oldStatus, newStatus),
            cancellationToken);
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

    public async Task<BusinessTripPermissionsVm> GetPermissions(BusinessTrip businessTrip,
        CancellationToken cancellationToken)
    {
        return BuildPermissions(businessTrip, await GetAccessContext(cancellationToken));
    }

    public async Task<IReadOnlyDictionary<long, BusinessTripPermissionsVm>> GetPermissions(
        IReadOnlyCollection<BusinessTrip> businessTrips, CancellationToken cancellationToken)
    {
        var context = await GetAccessContext(cancellationToken);

        return businessTrips
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, x => BuildPermissions(x.First(), context));
    }

    /// <summary>
    /// Single source of truth for business trip access: used both to validate actions
    /// and to tell the client which actions to show.
    /// </summary>
    private BusinessTripPermissionsVm BuildPermissions(BusinessTrip businessTrip, AccessContext context)
    {
        var canView = CanView(businessTrip, context);
        var canManage = CanManage(businessTrip, context);
        var canChangeStatus = CanChangeStatus(businessTrip, context);

        return new BusinessTripPermissionsVm
        {
            CanView = canView,
            CanEdit = canManage && businessTrip.Status != BusinessTripStatus.Approved,
            CanApprove = canChangeStatus && businessTrip.Status != BusinessTripStatus.Approved,
            CanReject = canChangeStatus && businessTrip.Status != BusinessTripStatus.Rejected,
            CanDelete = canManage,
            CanDownload = canView && businessTrip.Status == BusinessTripStatus.Approved
        };
    }

    private bool CanView(BusinessTrip businessTrip, AccessContext context)
    {
        if (context.IsSysAdmin || IsAuthor(businessTrip) || IsParticipant(businessTrip))
            return true;

        return context.IsUnitDirector && IsInUnit(businessTrip, context.UnitDepartmentIds);
    }

    private bool CanManage(BusinessTrip businessTrip, AccessContext context)
    {
        if (context.IsSysAdmin)
            return true;

        if (context.IsUnitDirector
            && (IsAuthor(businessTrip) || IsInUnit(businessTrip, context.UnitDepartmentIds)))
            return true;

        return IsAuthor(businessTrip)
               && businessTrip.Status is BusinessTripStatus.PendingApproval or BusinessTripStatus.Rejected;
    }

    private static bool CanChangeStatus(BusinessTrip businessTrip, AccessContext context)
    {
        return context.IsSysAdmin
               || (context.IsUnitDirector && IsInUnit(businessTrip, context.UnitDepartmentIds));
    }

    private async Task<AccessContext> GetAccessContext(CancellationToken cancellationToken)
    {
        return new AccessContext(
            employeeRoleChecker.HasCurrentUserSysAdminRole(),
            employeeRoleChecker.HasCurrentUserUnitDirectorRole(),
            await GetCurrentUnitDepartmentIds(cancellationToken));
    }

    private sealed record AccessContext(
        bool IsSysAdmin,
        bool IsUnitDirector,
        IReadOnlySet<long> UnitDepartmentIds);

    private async Task<IReadOnlySet<long>> GetCurrentUnitDepartmentIds(CancellationToken cancellationToken)
    {
        var unitId = currentUserService.Employee.Department?.UnitId;

        if (unitId is null || !employeeRoleChecker.HasCurrentUserUnitDirectorRole())
            return new HashSet<long>();

        var departments = await departmentRepository.GetDepartmentsByUnitId(unitId.Value, cancellationToken);

        return departments.Select(x => x.Id).ToHashSet();
    }
}