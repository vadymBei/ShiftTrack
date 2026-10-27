using Moq;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Strategies;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Booking.Vacations.Strategies;

public class UnitDirectorVacationStrategyTests
{
    private const string Role = "unit.director";

    // Regional director of the whole unit 1, working in department 99 of unit 9.
    private readonly Employee _director = new EmployeeBuilder()
        .WithId(1)
        .WorkingIn(departmentId: 99, unitId: 9)
        .WithRoleInUnit(Role, unitId: 1)
        .Build();

    private readonly Employee _employeeInScope = new EmployeeBuilder().WithId(2).WorkingIn(10, 1).Build();
    private readonly Employee _employeeOutOfScope = new EmployeeBuilder().WithId(3).WorkingIn(20, 2).Build();

    private readonly VacationStrategyFixture _fixture;
    private readonly UnitDirectorVacationStrategy _strategy;

    public UnitDirectorVacationStrategyTests()
    {
        _fixture = new VacationStrategyFixture(_director);

        _strategy = new UnitDirectorVacationStrategy(
            vacationRepository: _fixture.VacationRepository.Object,
            currentUserService: _fixture.CurrentUserService.Object,
            employeeShiftService: _fixture.EmployeeShiftService.Object,
            vacationShiftService: _fixture.VacationShiftService.Object,
            roleScopeService: _fixture.RoleScopeService);
    }

    [Theory]
    [InlineData(VacationStatus.PendingApproval)]
    [InlineData(VacationStatus.ApprovedByDepartmentDirector)]
    public async Task Approve_VacationInScope_ApprovesFinally(VacationStatus status)
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeInScope).InStatus(status).Build());

        await _strategy.ApproveVacation(vacation.Id, CancellationToken.None);

        Assert.Equal(VacationStatus.ApprovedByUnitDirector, vacation.Status);
        _fixture.VerifySaved(Times.Once());
        _fixture.VerifyShiftsSet(Times.Once());
    }

    [Fact]
    public async Task Approve_DraftOfOtherEmployee_IsNotAllowed()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeInScope).AuthoredBy(_employeeInScope.Id)
            .Build());

        Assert.False(_strategy.GetAccess(vacation).CanApprove);

        await Assert.ThrowsAsync<VacationException>(() =>
            _strategy.ApproveVacation(vacation.Id, CancellationToken.None));

        Assert.Equal(VacationStatus.None, vacation.Status);
        _fixture.VerifySaved(Times.Never());
    }

    [Fact]
    public async Task Approve_VacationOutOfScope_Throws()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeOutOfScope)
            .InStatus(VacationStatus.PendingApproval).Build());

        await Assert.ThrowsAsync<VacationException>(() =>
            _strategy.ApproveVacation(vacation.Id, CancellationToken.None));

        _fixture.VerifySaved(Times.Never());
    }

    [Fact]
    public void OwnVacation_OutsideManagedUnit_CanBeApproved()
    {
        // There is no role above the unit director, so they approve their own requests.
        var vacation = new VacationBuilder().Of(_director).AuthoredBy(_director.Id)
            .InStatus(VacationStatus.PendingApproval).Build();

        var access = _strategy.GetAccess(vacation);

        Assert.True(access.CanView);
        Assert.True(access.CanApprove);
    }

    [Fact]
    public async Task Reject_FinallyApprovedVacation_RestoresShifts()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeInScope)
            .InStatus(VacationStatus.ApprovedByUnitDirector).Build());

        await _strategy.RejectVacation(vacation.Id, CancellationToken.None);

        Assert.Equal(VacationStatus.Rejected, vacation.Status);
        _fixture.VerifySaved(Times.Once());
        _fixture.EmployeeShiftService.Verify(x => x.RestorePreviousEmployeeShifts(
            It.IsAny<RestoreEmployeeShiftsDto>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Reject_RejectedVacation_Throws()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeInScope)
            .InStatus(VacationStatus.Rejected).Build());

        await Assert.ThrowsAsync<VacationException>(() =>
            _strategy.RejectVacation(vacation.Id, CancellationToken.None));

        _fixture.VerifySaved(Times.Never());
    }

    [Fact]
    public void AccessFilter_UsesRoleBindingsAndOwnVacations()
    {
        var filter = _strategy.GetAccessFilter();

        Assert.Equal(new long[] { 1 }, filter.UnitIds);
        Assert.Empty(filter.DepartmentIds);
        Assert.Equal(_director.Id, filter.OwnerId);
    }
}
