using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Queries.GetPositions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Queries;

public class GetPositionsQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetPositions_ShouldReturnOnePosition_WhenPositionsExists()
    {
        // Arrange
        var positionsToRemove = DbContext.Positions.ToList();
        DbContext.Positions.RemoveRange(positionsToRemove);

        var createPositionCommand = new CreatePositionCommand(
            "Адміністратор",
            "Адміністратор магазину");

        var newPosition = await Mediator.Invoke(createPositionCommand);

        var getPositionsQuery = new GetPositionsQuery();

        // Act
        var positions = await Mediator.Invoke(getPositionsQuery);

        // Assert
        positions.Should().NotBeNull();
        positions.Should().HaveCount(1);

        positions.Should().Contain(x => x.Id == newPosition.Id);
        positions.Should().Contain(x => x.Name == newPosition.Name);
        positions.Should().Contain(x => x.Description == newPosition.Description);
    }
}