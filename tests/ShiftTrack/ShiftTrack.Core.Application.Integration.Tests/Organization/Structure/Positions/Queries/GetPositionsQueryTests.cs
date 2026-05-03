using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
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
        var positionsToRemove = DbContext.Positions.ToList();
        DbContext.Positions.RemoveRange(positionsToRemove);

        var createPositionCommand = new CreatePositionCommand(
            new PositionToCreateDto(
                "Адміністратор",
                "Адміністратор магазину",
                150));

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
        positions.Should().Contain(x => x.HourlyRate == newPosition.HourlyRate);
    }
}