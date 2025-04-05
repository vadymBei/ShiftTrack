using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Commands;

public class CreateUnitCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewUnitToDatabase()
    {
        // Arrange
        var createUnitCommand = new CreateUnitCommand(
            "Хмельницький",
            "Хмельницький регіон",
            "Хм");

        // Act
        var newUnit = await Sender.Send(createUnitCommand);

        // Assert
        var unit = DbContext.Units
            .FirstOrDefault(x => x.Id == newUnit.Id);

        unit.Should().NotBeNull();
        unit.Should().BeEquivalentTo(
            new UnitVM
            {
                Id = 1,
                Name = createUnitCommand.Name,
                Description = createUnitCommand.Description,
                Code = createUnitCommand.Code,
                FullName = createUnitCommand.Code + " " + createUnitCommand.Name,
            });
    }
}