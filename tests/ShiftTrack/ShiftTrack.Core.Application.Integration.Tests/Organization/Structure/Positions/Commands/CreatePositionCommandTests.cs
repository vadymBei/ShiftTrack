using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.CreatePosition;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Commands;

public class CreatePositionCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewPositionToDatabase()
    {
        // Arrange
        var createPositionCommand = new CreatePositionCommand(
            "Адміністратор",
            "Адміністратор магазину");

        // Act
        var newPosition = await Mediator.Invoke(createPositionCommand);

        // Assert
        var position = DbContext.Positions
            .FirstOrDefault(x => x.Id == newPosition.Id);

        position.Should().NotBeNull();

        position.Name.Should().Be(createPositionCommand.Name);
        position.Description.Should().Be(createPositionCommand.Description);
    }
}