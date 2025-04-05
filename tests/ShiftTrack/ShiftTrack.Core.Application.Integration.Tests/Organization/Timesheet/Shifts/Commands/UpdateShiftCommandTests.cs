using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.UpdateShift;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class UpdateShiftCommandTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenShiftNotFound()
    {
        // Arrange
        var updateShiftCommand = new UpdateShiftCommand(
            1000,
            "ВХ",
            "Вихідний",
            "#FFFFF",
            ShiftType.DayOff,
            new TimeSpan(09, 30, 00),
            new TimeSpan(21, 00, 00));

        // Act
        Func<Task> act = async () => await Sender.Send(updateShiftCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldUpdate_WhenShiftExists()
    {
        // Arrange
        var createShiftCommand = new CreateShiftCommand(
            "ВХ",
            "Вихідний",
            "#FFFFF",
            ShiftType.DayOff,
            new TimeSpan(09, 30, 00),
            new TimeSpan(21, 00, 00));

        var newShift = await Sender.Send(createShiftCommand);

        var updateShiftCommand = new UpdateShiftCommand(
            newShift.Id,
            "Р",
            "Робоча зміна 9 год 30 хв",
            "#FFF200",
            ShiftType.Workday,
            new TimeSpan(10, 00, 00),
            new TimeSpan(19, 00, 00));

        // Act
        var updatedShift = await Sender.Send(updateShiftCommand);

        // Assert
        updatedShift.Should().NotBeNull();

        updatedShift.Should().BeEquivalentTo(
            new ShiftVM()
            {
                Id = updateShiftCommand.Id,
                Code = updateShiftCommand.Code,
                Description = updateShiftCommand.Description,
                Color = updateShiftCommand.Color,
                Type = updateShiftCommand.Type,
                StartTime = updateShiftCommand.StartTime,
                EndTime = updateShiftCommand.EndTime,
                WorkHours = updateShiftCommand.EndTime - updateShiftCommand.StartTime
            });
    }
}