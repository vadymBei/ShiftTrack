using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition;

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
        var newPosition = await Sender.Send(createPositionCommand);

        // Assert
        var position = DbContext.Positions
            .FirstOrDefault(x => x.Id == newPosition.Id);

        position.Should().NotBeNull();

        position.Name.Should().Be(createPositionCommand.Name);
        position.Description.Should().Be(createPositionCommand.Description);
    }
}