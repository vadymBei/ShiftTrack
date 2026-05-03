using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositionById;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Queries;

public class GetPositionByIdQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetById_ShouldReturnPosition_WhenPositionExists()
    {
        // Arrange
        var createPositionCommand = new CreatePositionCommand(
            new PositionToCreateDto(
                "Адміністратор",
                "Адміністратор магазину",
                150));

        var newPosition = await Mediator.Invoke(createPositionCommand);

        var getPositionByIdQuery = new GetPositionByIdQuery(newPosition.Id);

        // Act
        var position = await Mediator.Invoke(getPositionByIdQuery);

        // Assert
        position.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturnEntityNotFoundException_WhenPositionNotFound()
    {
        // Arrange
        var getPositionByIdQuery = new GetPositionByIdQuery(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(getPositionByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}