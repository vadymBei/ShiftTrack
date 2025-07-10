using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitById;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Queries;

public class GetUnitByIdQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetById_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
    {
        // Arrange
        var getUnitByIdQuery = new GetUnitByIdQuery(100);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(getUnitByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetById_ShouldReturnUnit_WhenUnitExists()
    {
        // Arrange
        var createUnitCommand = new CreateUnitCommand(
            "Хмельницький",
            "Хмельницький регіон",
            "Хм");

        var newUnit = await Mediator.Invoke(createUnitCommand);

        var getUnitByIdQuery = new GetUnitByIdQuery(newUnit.Id);

        // Act
        var unit = await Mediator.Invoke(getUnitByIdQuery);

        // Assert
        unit.Should().NotBeNull();

        unit.Should().BeEquivalentTo(
            new UnitVm()
            {
                Id = newUnit.Id,
                Code = newUnit.Code,
                Name = newUnit.Name,
                Description = newUnit.Description,
                FullName = newUnit.Code + " " + newUnit.Name
            });
    }
}