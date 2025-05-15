using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.UpdatePosition;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositionById;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Commands;

public class UpdatePositionCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Update_ShouldReturnUpdatedPosition_WhenPositionExists()
    {
        // Arrange
        var createPositionCommand = new CreatePositionCommand(
            "Адміністратор",
            "Адміністратор магазину");

        var newPosition = await Mediator.Invoke(createPositionCommand);

        var getPositionByIdQuery = new GetPositionByIdQuery(newPosition.Id);

        var position = await Mediator.Invoke(getPositionByIdQuery);

        var updatePositionCommand = new UpdatePositionCommand(
            position.Id,
            "Адміністратор оновлений",
            "Адміністратор магазину оновлений");

        // Act
        var updatedPosition = await Mediator.Invoke(updatePositionCommand);

        // Assert
        updatedPosition.Should().NotBeNull();

        updatedPosition.Should().BeEquivalentTo(
            new PositionVM()
            {
                Id = position.Id,
                Name = updatePositionCommand.Name,
                Description = updatePositionCommand.Description
            });
    }

    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenPositionNotExists()
    {
        // Arrange
        var updatePositionCommand = new UpdatePositionCommand(
            1000,
            "Тест",
            "Тест оновлення посади з помилкою");

        // Act
        Func<Task> act = async () => await Mediator.Invoke(updatePositionCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}