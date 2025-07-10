using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

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
        var newUnit = await Mediator.Invoke(createUnitCommand);

        // Assert
        var unit = DbContext.Units
            .FirstOrDefault(x => x.Id == newUnit.Id);

        unit.Should().NotBeNull();
        unit.Should().BeEquivalentTo(
            new UnitVm()
            {
                Id = newUnit.Id,
                Name = createUnitCommand.Name,
                Description = createUnitCommand.Description,
                Code = createUnitCommand.Code,
                FullName = createUnitCommand.Code + " " + createUnitCommand.Name,
            });
    }
}