using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.DeleteUnit;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Commands;

public class DeleteUnitCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
    {
        // Arrange
        var deleteUnitCommand = new DeleteUnitCommand(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(deleteUnitCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Delete_ShouldRemove_WhenUnitExists()
    {
        // Arrange
        var createUnitCommand = new CreateUnitCommand(
            new UnitToCreateDto(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм"));

        var newUnit = await Mediator.Invoke(createUnitCommand);

        var deleteUnitCommand = new DeleteUnitCommand(newUnit.Id);

        // Act 
        await Mediator.Invoke(deleteUnitCommand);

        // Assert
        var deletedUnit = DbContext.Units.FirstOrDefault(x => x.Id == newUnit.Id);

        deletedUnit.Should().BeNull();
    }
}