using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShiftById;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
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
        Func<Task> act = async () => await Mediator.Send(getByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetById_ShouldReturnShift_WhenShiftExists()
    {
        // Arrange
        var newShift = await CreateShiftAsync();

        var getByIdQuery = new GetShiftByIdQuery(newShift.Id);

        // Act
        var shift = await Mediator.Send(getByIdQuery);

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
                WorkHours = newShift.WorkHours
            });
    }
}