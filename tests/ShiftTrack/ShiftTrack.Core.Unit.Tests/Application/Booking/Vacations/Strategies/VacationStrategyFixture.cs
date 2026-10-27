using Moq;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Modules.System.User.RoleScopes.Services;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Booking.Vacations.Strategies;

/// <summary>Mocks shared by the vacation strategy tests; the role scope is real.</summary>
public class VacationStrategyFixture
{
    public Mock<IVacationRepository> VacationRepository { get; } = new();
    public Mock<ICurrentUserService> CurrentUserService { get; } = new();
    public Mock<IEmployeeShiftService> EmployeeShiftService { get; } = new();
    public Mock<IVacationShiftService> VacationShiftService { get; } = new();

    public IRoleScopeService RoleScopeService { get; }

    public VacationStrategyFixture(Employee currentEmployee)
    {
        CurrentUserService.Setup(x => x.Employee).Returns(currentEmployee);

        // Only the current employee's bindings are used, no departments are loaded from the database.
        RoleScopeService = new RoleScopeService(CurrentUserService.Object, departmentRepository: null);
    }

    public Vacation Stored(Vacation vacation)
    {
        VacationRepository
            .Setup(x => x.GetForUpdate(vacation.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vacation);

        return vacation;
    }

    public void VerifySaved(Times times) =>
        VacationRepository.Verify(x => x.Save(It.IsAny<CancellationToken>()), times);

    public void VerifyShiftsSet(Times times) =>
        VacationShiftService.Verify(x => x.SetVacationShifts(It.IsAny<long>(), It.IsAny<CancellationToken>()), times);
}
