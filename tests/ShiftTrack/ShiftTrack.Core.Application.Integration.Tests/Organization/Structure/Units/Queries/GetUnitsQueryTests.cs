using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnits;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

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
            new UnitToCreateDto(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм"));

        await Mediator.Invoke(firstUnitCreateCommand);

        var secondUnitCreateCommand = new CreateUnitCommand(
            new UnitToCreateDto(
                "Львів",
                "Львівський регіон",
                "Лв"));

        await Mediator.Invoke(secondUnitCreateCommand);

        var getUnitsQuery = new GetUnitsQuery();

        // Act
        var units = await Mediator.Invoke(getUnitsQuery);

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