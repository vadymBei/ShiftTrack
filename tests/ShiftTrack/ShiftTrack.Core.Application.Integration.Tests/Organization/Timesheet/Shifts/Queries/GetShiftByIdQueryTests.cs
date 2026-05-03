using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShiftById;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Queries;

public class GetShiftByIdQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetById_ShouldReturnEntityNotFoundException_WhenShiftNotFound()
    {
        // Arrange
        var getByIdQuery = new GetShiftByIdQuery(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(getByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetById_ShouldReturnShift_WhenShiftExists()
    {
        // Arrange
        var createShiftCommand = new CreateShiftCommand(
            new ShiftToCreateDto(
                "ТС1",
                "Тест 1",
                "#FFFFF",
                ShiftType.DayOff,
                new TimeSpan(09, 30, 00),
                new TimeSpan(21, 00, 00)));

        var newShift = await Mediator.Invoke(createShiftCommand);

        var getByIdQuery = new GetShiftByIdQuery(newShift.Id);

        // Act
        var shift = await Mediator.Invoke(getByIdQuery);

        // Assert
        shift.Should().NotBeNull();

        shift.Should().BeEquivalentTo(
            new ShiftVm()
            {
                Id = newShift.Id,
                Code = newShift.Code,
                Description = newShift.Description,
                Color = newShift.Color,
                Type = newShift.Type,
                StartTime = newShift.StartTime,
                EndTime = newShift.EndTime,
                WorkHours = newShift.EndTime - newShift.StartTime
            });
    }
}