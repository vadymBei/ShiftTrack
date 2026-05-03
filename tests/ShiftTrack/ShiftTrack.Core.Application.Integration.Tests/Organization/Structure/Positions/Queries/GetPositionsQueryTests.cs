using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Queries;

public class GetPositionsQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetPositions_ShouldReturnOnePosition_WhenPositionsExists()
    {
        // Arrange
        var newPosition = await CreatePositionAsync();

        var getPositionsQuery = new GetPositionsQuery();

        // Act
        var positions = await Mediator.Invoke(getPositionsQuery);

        // Assert
        positions.Should().NotBeNull();
        positions.Should().Contain(x => x.Id == newPosition.Id);

        var position = positions.First(x => x.Id == newPosition.Id);
        position.Name.Should().Be(newPosition.Name);
        position.Description.Should().Be(newPosition.Description);
        position.HourlyRate.Should().Be(newPosition.HourlyRate);
    }
}