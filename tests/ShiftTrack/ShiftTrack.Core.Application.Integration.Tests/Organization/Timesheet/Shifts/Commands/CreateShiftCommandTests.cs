using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class CreateShiftCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewShiftToDbContext()
    {
        // Arrange
        var createShiftCommand = new CreateShiftCommand(
            "ВХ",
            "Вихідний",
            "#FFFFF",
            ShiftType.DayOff,
            new TimeSpan(09, 30, 00),
            new TimeSpan(21, 00, 00));

        // Act
        var newShift = await Mediator.Invoke(createShiftCommand);

        // Assert
        var shift = DbContext.Shifts.FirstOrDefault(x => x.Id == newShift.Id);

        shift.Should().NotBeNull();

        shift.Should().BeEquivalentTo(
            new Shift()
            {
                Id = newShift.Id,
                Code = newShift.Code,
                Description = newShift.Description,
                Color = newShift.Color,
                Type = newShift.Type,
                StartTime = newShift.StartTime,
                EndTime = newShift.EndTime,
                IsDeleted = false,
                DeletedAt = null,
                WorkHours = newShift.EndTime - newShift.StartTime
            });
    }
}