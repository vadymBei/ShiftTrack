using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.Booking.Vacations.Exceptions;

namespace ShiftTrack.Core.Unit.Tests.Domain.Booking.Vacations;

public class VacationStatusTests
{
    private const VacationApprovalLevel Department = VacationApprovalLevel.DepartmentDirector;
    private const VacationApprovalLevel Unit = VacationApprovalLevel.UnitDirector;

    [Fact]
    public void NewVacation_IsDraft()
    {
        var vacation = new VacationBuilder().Build();

        Assert.Equal(VacationStatus.None, vacation.Status);
        Assert.True(vacation.CanSubmit);
    }

    [Fact]
    public void Submit_Draft_MovesToPendingApproval()
    {
        var vacation = new VacationBuilder().Build();

        vacation.Submit();

        Assert.Equal(VacationStatus.PendingApproval, vacation.Status);
    }

    [Theory]
    [InlineData(VacationStatus.PendingApproval)]
    [InlineData(VacationStatus.ApprovedByDepartmentDirector)]
    [InlineData(VacationStatus.ApprovedByUnitDirector)]
    [InlineData(VacationStatus.Rejected)]
    public void Submit_NotDraft_IsBlocked(VacationStatus status)
    {
        var vacation = new VacationBuilder().InStatus(status).Build();

        Assert.False(vacation.CanSubmit);
        Assert.Throws<VacationStatusTransitionException>(vacation.Submit);
        Assert.Equal(status, vacation.Status);
    }

    [Theory]
    [InlineData(VacationStatus.PendingApproval, Department, VacationStatus.ApprovedByDepartmentDirector)]
    [InlineData(VacationStatus.PendingApproval, Unit, VacationStatus.ApprovedByUnitDirector)]
    [InlineData(VacationStatus.ApprovedByDepartmentDirector, Unit, VacationStatus.ApprovedByUnitDirector)]
    public void Approve_AllowedTransition_ChangesStatus(VacationStatus from, VacationApprovalLevel level,
        VacationStatus expected)
    {
        var vacation = new VacationBuilder().InStatus(from).Build();

        Assert.True(vacation.CanApprove(level));

        vacation.Approve(level);

        Assert.Equal(expected, vacation.Status);
    }

    [Theory]
    [InlineData(VacationStatus.None, Department)]
    [InlineData(VacationStatus.None, Unit)]
    [InlineData(VacationStatus.ApprovedByDepartmentDirector, Department)]
    [InlineData(VacationStatus.ApprovedByUnitDirector, Department)]
    [InlineData(VacationStatus.ApprovedByUnitDirector, Unit)]
    [InlineData(VacationStatus.Rejected, Department)]
    [InlineData(VacationStatus.Rejected, Unit)]
    public void Approve_NotAllowedTransition_IsBlocked(VacationStatus from, VacationApprovalLevel level)
    {
        var vacation = new VacationBuilder().InStatus(from).Build();

        Assert.False(vacation.CanApprove(level));
        Assert.Throws<VacationStatusTransitionException>(() => vacation.Approve(level));
        Assert.Equal(from, vacation.Status);
    }

    [Theory]
    [InlineData(VacationStatus.PendingApproval, Department)]
    [InlineData(VacationStatus.PendingApproval, Unit)]
    [InlineData(VacationStatus.ApprovedByDepartmentDirector, Department)]
    [InlineData(VacationStatus.ApprovedByDepartmentDirector, Unit)]
    [InlineData(VacationStatus.ApprovedByUnitDirector, Unit)]
    public void Reject_AllowedTransition_MovesToRejected(VacationStatus from, VacationApprovalLevel level)
    {
        var vacation = new VacationBuilder().InStatus(from).Build();

        Assert.True(vacation.CanReject(level));

        vacation.Reject(level);

        Assert.Equal(VacationStatus.Rejected, vacation.Status);
    }

    [Theory]
    [InlineData(VacationStatus.None, Department)]
    [InlineData(VacationStatus.None, Unit)]
    [InlineData(VacationStatus.ApprovedByUnitDirector, Department)]
    [InlineData(VacationStatus.Rejected, Department)]
    [InlineData(VacationStatus.Rejected, Unit)]
    public void Reject_NotAllowedTransition_IsBlocked(VacationStatus from, VacationApprovalLevel level)
    {
        var vacation = new VacationBuilder().InStatus(from).Build();

        Assert.False(vacation.CanReject(level));
        Assert.Throws<VacationStatusTransitionException>(() => vacation.Reject(level));
        Assert.Equal(from, vacation.Status);
    }
}
