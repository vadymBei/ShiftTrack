using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.DeleteShift;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.Exceptions;

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
        Func<Task> act = () => Mediator.Send(command);

        // Assert
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Delete_ShouldMarkAsDeleted_WhenShiftExists()
    {
        // Arrange
        var shift = await CreateShiftAsync();

        var deleteCommand = new DeleteShiftCommand(shift.Id);

        // Act
        await Mediator.Send(deleteCommand);

        // Assert
        Func<Task> act = async () => await ShiftRepository.GetById(shift.Id, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}