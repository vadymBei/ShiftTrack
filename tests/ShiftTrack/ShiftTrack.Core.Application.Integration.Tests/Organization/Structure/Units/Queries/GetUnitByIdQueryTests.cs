using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnitById;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
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
        Func<Task> act = async () => await Mediator.Send(getUnitByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetById_ShouldReturnUnit_WhenUnitExists()
    {
        // Arrange
        var newUnit = await CreateUnitAsync();

        var getUnitByIdQuery = new GetUnitByIdQuery(newUnit.Id);

        // Act
        var unit = await Mediator.Send(getUnitByIdQuery);

        // Assert
        unit.Should().NotBeNull();

        unit.Should().BeEquivalentTo(
            new UnitVm()
            {
                Id = newUnit.Id,
                Code = newUnit.Code,
                Name = newUnit.Name,
                Description = newUnit.Description,
                FullName = newUnit.FullName
            });
    }
}