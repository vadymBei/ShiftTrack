using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.DeletePosition;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Commands;

public class DeletePositionCommandTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_ShouldRemove_WhenPositionExists()
    {
        // Arrange
        var createPositionCommand = new CreatePositionCommand(
            "Адміністратор",
            "Адміністратор магазину");

        var newPosition = await Mediator.Invoke(createPositionCommand);

        var deletePositionCommand = new DeletePositionCommand(newPosition.Id);

        // Act
        await Mediator.Invoke(deletePositionCommand);

        // Assert
        var deletedPosition = DbContext.Positions.FirstOrDefault(x => x.Id == newPosition.Id);

        deletedPosition.Should().BeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnEntityNotFoundException_WhenPositionNotFound()
    {
        // Arrange
        var deletePositionCommand = new DeletePositionCommand(1000);

        // Act
        Func<Task> act = async () =>  await Mediator.Invoke(deletePositionCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}