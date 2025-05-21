using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class DeleteShiftCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_ShouldReturnEntityNotFoundException_WhenShiftNotFound()
    {
        // Arrange
        var deleteShiftCommand = new DeleteShiftCommand(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(deleteShiftCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Delete_ShouldDelete_WhenShiftExists()
    {
        // Arrange
        var createShiftCommand = new CreateShiftCommand(
            "ВХ",
            "Вихідний",
            "#FFFFF",
            ShiftType.DayOff,
            new TimeSpan(09, 30, 00),
            new TimeSpan(21, 00, 00));

        var shift = await Mediator.Invoke(createShiftCommand);

        var deleteShiftCommand = new DeleteShiftCommand(shift.Id);

        // Act
        await Mediator.Invoke(deleteShiftCommand);

        // Assert
        var deletedShift = DbContext.Shifts.FirstOrDefault(x => x.Id == shift.Id);

        deletedShift.Should().BeNull();
    }
}