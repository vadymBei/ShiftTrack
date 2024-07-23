using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Queries.GetPositions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Queries
{
    public class GetPositionsQueryTests : BaseIntegrationTest
    {
        public GetPositionsQueryTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetPositions_ShouldReturnOnePosition_WhenPositionExists()
        {
            // Arrange
            var positionsToRemove = DbContext.Positions.ToList();
            DbContext.Positions.RemoveRange(positionsToRemove);

            var createPositionCommand = new CreatePositionCommand(
                "Адміністратор",
                "Адміністратор магазину");

            var newPosition = await Sender.Send(createPositionCommand);

            var getPositionsQuery = new GetPositionsQuery();

            // Act
            var positions = await Sender.Send(getPositionsQuery);

            // Assert
            positions.Should().NotBeNull();
            positions.Should().HaveCount(1);

            positions.Should().Contain(x => x.Id == newPosition.Id);
            positions.Should().Contain(x => x.Name == newPosition.Name);
            positions.Should().Contain(x => x.Description == newPosition.Description);
        }
    }
}
