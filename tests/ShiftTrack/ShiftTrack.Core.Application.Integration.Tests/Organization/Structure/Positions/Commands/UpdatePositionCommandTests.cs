using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.UpdatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositionById;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
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
            new PositionToCreateDto(
                "Адміністратор",
                "Адміністратор магазину",
                150));

        var newPosition = await Mediator.Invoke(createPositionCommand);

        var getPositionByIdQuery = new GetPositionByIdQuery(newPosition.Id);

        var position = await Mediator.Invoke(getPositionByIdQuery);

        var updatePositionCommand = new UpdatePositionCommand(
            new PositionToUpdateDto(
                position.Id,
                "Адміністратор оновлений",
                "Адміністратор магазину оновлений",
                170));

        // Act
        var updatedPosition = await Mediator.Invoke(updatePositionCommand);

        // Assert
        updatedPosition.Should().NotBeNull();

        updatedPosition.Should().BeEquivalentTo(
            new PositionVm()
            {
                Id = position.Id,
                Name = updatePositionCommand.Data.Name,
                Description = updatePositionCommand.Data.Description,
                HourlyRate = updatePositionCommand.Data.HourlyRate
            });
    }

    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenPositionNotExists()
    {
        // Arrange
        var updatePositionCommand = new UpdatePositionCommand(
            new PositionToUpdateDto(
                1000,
                "Тест",
                "Тест оновлення посади з помилкою",
                150));

        // Act
        Func<Task> act = async () => await Mediator.Invoke(updatePositionCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}