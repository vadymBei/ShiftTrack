using FluentAssertions;
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
        var firstUnit = await CreateUnitAsync();
        var secondUnit = await CreateUnitAsync();

        var getUnitsQuery = new GetUnitsQuery();

        // Act
        var units = await Mediator.Invoke(getUnitsQuery);

        // Assert
        units.Should().NotBeNull();
        units.Should().Contain(x => x.Id == firstUnit.Id);
        units.Should().Contain(x => x.Id == secondUnit.Id);

        var unit1 = units.First(x => x.Id == firstUnit.Id);
        unit1.Name.Should().Be(firstUnit.Name);
        unit1.Description.Should().Be(firstUnit.Description);
        unit1.Code.Should().Be(firstUnit.Code);

        var unit2 = units.First(x => x.Id == secondUnit.Id);
        unit2.Name.Should().Be(secondUnit.Name);
        unit2.Description.Should().Be(secondUnit.Description);
        unit2.Code.Should().Be(secondUnit.Code);
    }
}