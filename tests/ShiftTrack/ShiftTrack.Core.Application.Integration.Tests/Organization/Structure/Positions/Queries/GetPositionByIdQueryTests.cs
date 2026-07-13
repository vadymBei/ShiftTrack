using FluentAssertions;
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
        var newPosition = await CreatePositionAsync();

        var getPositionByIdQuery = new GetPositionByIdQuery(newPosition.Id);

        // Act
        var position = await Mediator.Send(getPositionByIdQuery);

        // Assert
        position.Should().NotBeNull();
        position.Id.Should().Be(newPosition.Id);
        position.Name.Should().Be(newPosition.Name);
    }

    [Fact]
    public async Task GetById_ShouldReturnEntityNotFoundException_WhenPositionNotFound()
    {
        // Arrange
        var getPositionByIdQuery = new GetPositionByIdQuery(1000);

        // Act
        Func<Task> act = async () => await Mediator.Send(getPositionByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}