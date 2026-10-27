using Moq;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Strategies;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Booking.Vacations.Strategies;

public class DepartmentDirectorVacationStrategyTests
{
    private const string Role = "department.director";

    // Store director bound to departments 10 and 11 of unit 1, working in department 10.
    private readonly Employee _director = new EmployeeBuilder()
        .WithId(1)
        .WorkingIn(departmentId: 10, unitId: 1)
        .WithRoleInDepartments(Role, 1, 10, 11)
        .Build();

    private readonly Employee _employeeInScope = new EmployeeBuilder().WithId(2).WorkingIn(11, 1).Build();
    private readonly Employee _employeeOutOfScope = new EmployeeBuilder().WithId(3).WorkingIn(12, 1).Build();

    private readonly VacationStrategyFixture _fixture;
    private readonly DepartmentDirectorVacationStrategy _strategy;

    public DepartmentDirectorVacationStrategyTests()
    {
        _fixture = new VacationStrategyFixture(_director);

        _strategy = new DepartmentDirectorVacationStrategy(
            currentUserService: _fixture.CurrentUserService.Object,
            vacationRepository: _fixture.VacationRepository.Object,
            employeeShiftService: _fixture.EmployeeShiftService.Object,
            vacationShiftService: _fixture.VacationShiftService.Object,
            roleScopeService: _fixture.RoleScopeService);
    }

    [Fact]
    public void PendingVacationInScope_CanBeApprovedAndRejected()
    {
        var vacation = new VacationBuilder().Of(_employeeInScope).InStatus(VacationStatus.PendingApproval).Build();

        var access = _strategy.GetAccess(vacation);

        Assert.True(access.CanView);
        Assert.True(access.CanApprove);
        Assert.True(access.CanReject);
        Assert.False(access.ApproveSubmitsForApproval);
    }

    [Fact]
    public void VacationOutOfScope_IsNotAccessible()
    {
        var vacation = new VacationBuilder().Of(_employeeOutOfScope).InStatus(VacationStatus.PendingApproval).Build();

        var access = _strategy.GetAccess(vacation);

        Assert.False(access.CanView);
        Assert.False(access.CanApprove);
        Assert.False(access.CanReject);
    }

    [Fact]
    public void OwnPendingVacation_IsVisibleButCannotBeApproved()
    {
        var vacation = new VacationBuilder().Of(_director).AuthoredBy(_director.Id)
            .InStatus(VacationStatus.PendingApproval).Build();

        var access = _strategy.GetAccess(vacation);

        Assert.True(access.CanView);
        Assert.False(access.CanApprove);
    }

    [Fact]
    public void OwnDraft_IsSubmittedForApproval()
    {
        var vacation = new VacationBuilder().Of(_director).AuthoredBy(_director.Id).Build();

        Assert.True(_strategy.GetAccess(vacation).ApproveSubmitsForApproval);
    }

    [Fact]
    public async Task Approve_PendingVacationInScope_ApprovesAndSetsShifts()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeInScope)
            .InStatus(VacationStatus.PendingApproval).Build());

        await _strategy.ApproveVacation(vacation.Id, CancellationToken.None);

        Assert.Equal(VacationStatus.ApprovedByDepartmentDirector, vacation.Status);
        _fixture.VerifySaved(Times.Once());
        _fixture.VerifyShiftsSet(Times.Once());
    }

    [Fact]
    public async Task Approve_OwnDraft_OnlySubmits()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_director).AuthoredBy(_director.Id).Build());

        await _strategy.ApproveVacation(vacation.Id, CancellationToken.None);

        Assert.Equal(VacationStatus.PendingApproval, vacation.Status);
        _fixture.VerifySaved(Times.Once());
        _fixture.VerifyShiftsSet(Times.Never());
    }

    [Fact]
    public async Task Approve_VacationOutOfScope_Throws()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeOutOfScope)
            .InStatus(VacationStatus.PendingApproval).Build());

        await Assert.ThrowsAsync<VacationException>(() =>
            _strategy.ApproveVacation(vacation.Id, CancellationToken.None));

        Assert.Equal(VacationStatus.PendingApproval, vacation.Status);
        _fixture.VerifySaved(Times.Never());
    }

    [Fact]
    public async Task Approve_AlreadyApprovedByDepartmentDirector_Throws()
    {
        var vacation = _fixture.Stored(new VacationBuilder().Of(_employeeInScope)
            .InStatus(VacationStatus.ApprovedByDepartmentDirector).Build());

        await Assert.ThrowsAsync<VacationException>(() =>
            _strategy.ApproveVacation(vacation.Id, CancellationToken.None));

        _fixture.VerifySaved(Times.Never());
    }

    [Fact]
    public void AccessFilter_UsesRoleBindingsAndOwnVacations()
    {
        var filter = _strategy.GetAccessFilter();

        Assert.Empty(filter.UnitIds);
        Assert.Equal(new long[] { 10, 11 }, filter.DepartmentIds.OrderBy(x => x));
        Assert.Equal(_director.Id, filter.OwnerId);
    }

    [Fact]
    public void ApprovalFilter_ExcludesOwnVacations()
    {
        var filter = _strategy.GetApprovalFilter();

        Assert.Equal(new[] { VacationStatus.PendingApproval }, filter.Statuses);
        Assert.Equal(_director.Id, filter.ExcludeEmployeeId);
    }

    [Fact]
    public void ApprovalFilter_WithoutBindings_IsNull()
    {
        var director = new EmployeeBuilder().WithId(1).WorkingIn(10, 1).WithRoleWithoutBindings(Role).Build();
        var fixture = new VacationStrategyFixture(director);
        var strategy = new DepartmentDirectorVacationStrategy(
            currentUserService: fixture.CurrentUserService.Object,
            vacationRepository: fixture.VacationRepository.Object,
            employeeShiftService: fixture.EmployeeShiftService.Object,
            vacationShiftService: fixture.VacationShiftService.Object,
            roleScopeService: fixture.RoleScopeService);

        Assert.Null(strategy.GetApprovalFilter());
    }
}
