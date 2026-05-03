using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.DeleteShift;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class DeleteShiftCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_ShouldReturnEntityNotFoundException_WhenShiftNotFound()
    {
        // Arrange
        var nonExistentId = 1000;
        var command = new DeleteShiftCommand(nonExistentId);

        // Act
        Func<Task> act = () => Mediator.Invoke(command);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Delete_ShouldMarkAsDeleted_WhenShiftExists()
    {
        // Arrange
        var createCommand = new CreateShiftCommand(
            new ShiftToCreateDto(
                "ТС3",
                "Тест 3",
                "#FFFFF",
                ShiftType.DayOff,
                new TimeSpan(09, 30, 00),
                new TimeSpan(21, 00, 00)));

        var shift = await Mediator.Invoke(createCommand);

        var deleteCommand = new DeleteShiftCommand(shift.Id);

        // Act
        await Mediator.Invoke(deleteCommand);

        // Assert
        var deletedShift = await DbContext.Shifts
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == shift.Id);

        deletedShift.Should().NotBeNull();
        deletedShift.Should().BeEquivalentTo(new
            {
                Id = shift.Id,
                IsDeleted = true,
                DeletedAt = deletedShift.DeletedAt
            },
            options => options
                .ExcludingMissingMembers()
                .Using<DateTime>(ctx => ctx.Subject.Should()
                    .BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());
    }
}