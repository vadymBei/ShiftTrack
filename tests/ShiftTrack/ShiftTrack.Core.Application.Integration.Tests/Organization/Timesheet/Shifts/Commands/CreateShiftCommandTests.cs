using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class CreateShiftCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewShiftToDbContext()
    {
        // Arrange
        var command = new CreateShiftCommand(
            "ТС",
            "Тестова",
            "#FFFFF",
            ShiftType.DayOff,
            new TimeSpan(09, 30, 00),
            new TimeSpan(21, 00, 00));
    
        var initialCount = await DbContext.Shifts.CountAsync();

        // Act
        var result = await Mediator.Invoke(command);

        // Assert
        // Перевірка результату команди
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            command.Code,
            command.Description,
            command.Color,
            command.Type,
            command.StartTime,
            command.EndTime,
            WorkHours = command.EndTime - command.StartTime
        });

        // Перевірка стану БД
        var currentCount = await DbContext.Shifts.CountAsync();
        currentCount.Should().Be(initialCount + 1);

        var shiftInDb = await DbContext.Shifts
            .FirstOrDefaultAsync(x => x.Id == result.Id);
    
        shiftInDb.Should().NotBeNull();
        shiftInDb.Should().BeEquivalentTo(new
            {
                result.Id,
                command.Code,
                command.Description,
                command.Color,
                command.Type,
                command.StartTime,
                command.EndTime,
                IsDeleted = false,
                DeletedAt = (DateTime?)null,
                WorkHours = command.EndTime - command.StartTime
            },
            options => options
                .ExcludingMissingMembers()
                .Using<DateTime>(ctx => ctx.Subject.Should()
                    .BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());
    }

    [Fact]
    public async Task Create_WithInvalidTimeRange_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateShiftCommand(
            "ТС",
            "Тестова",
            "#FFFFF",
            ShiftType.DayOff,
            new TimeSpan(21, 00, 00),  // EndTime менше ніж StartTime
            new TimeSpan(09, 30, 00));

        // Act
        Func<Task> act = () => Mediator.Invoke(command);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

}