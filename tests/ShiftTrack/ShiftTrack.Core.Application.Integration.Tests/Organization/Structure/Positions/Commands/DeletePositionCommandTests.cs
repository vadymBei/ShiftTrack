using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.DeletePosition;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Commands;

public class DeletePositionCommandTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_ShouldRemove_WhenPositionExists()
    {
        // Arrange
        var position = await CreatePositionAsync();

        var deletePositionCommand = new DeletePositionCommand(position.Id);

        // Act
        await Mediator.Send(deletePositionCommand);

        // Assert
        Func<Task> act = async () => await PositionRepository.GetById(position.Id, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Delete_ShouldReturnEntityNotFoundException_WhenPositionNotFound()
    {
        // Arrange
        var deletePositionCommand = new DeletePositionCommand(1000);

        // Act
        Func<Task> act = async () => await Mediator.Send(deletePositionCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}