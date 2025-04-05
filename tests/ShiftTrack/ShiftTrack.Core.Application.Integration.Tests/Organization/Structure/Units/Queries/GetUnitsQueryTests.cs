using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Queries;

public class GetUnitsQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetAll_ShouldReturnUnits_WhenUnitsExists()
    {
        // Arrange
        var unitsToDelete = DbContext.Units.ToList();
        DbContext.Units.RemoveRange(unitsToDelete);

        var firstUnitCreateCommand = new CreateUnitCommand(
            "Хмельницький",
            "Хмельницький регіон",
            "Хм");

        await Sender.Send(firstUnitCreateCommand);

        var secondUnitCreateCommand = new CreateUnitCommand(
            "Львів",
            "Львівський регіон",
            "Лв");

        await Sender.Send(secondUnitCreateCommand);
            
        var getUnitsQuery = new GetUnitsQuery();

        // Act
        var units = await Sender.Send(getUnitsQuery);

        // Assert
        units.Should().NotBeNull();
        units.Should().HaveCount(2);

        units.Should().Contain(x => x.Name == "Хмельницький");
        units.Should().Contain(x => x.Description == "Хмельницький регіон");
        units.Should().Contain(x => x.Code == "Хм");

        units.Should().Contain(x => x.Name == "Львів");
        units.Should().Contain(x => x.Description == "Львівський регіон");
        units.Should().Contain(x => x.Code == "Лв");
    }
}