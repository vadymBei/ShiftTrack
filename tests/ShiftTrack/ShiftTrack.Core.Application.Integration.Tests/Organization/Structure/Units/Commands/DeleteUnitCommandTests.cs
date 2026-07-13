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
        Func<Task> act = async () => await Mediator.Send(deleteUnitCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Delete_ShouldRemove_WhenUnitExists()
    {
        // Arrange
        var unit = await CreateUnitAsync();

        var deleteUnitCommand = new DeleteUnitCommand(unit.Id);

        // Act 
        await Mediator.Send(deleteUnitCommand);

        // Assert
        Func<Task> act = async () => await UnitRepository.GetById(unit.Id, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}