using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.UpdateUnit;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Commands;

public class UpdateUnitCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Update_ShouldReturnUpdatedUnit_WhenUnitExists()
    {
        // Arrange
        var createUnitCommand = new CreateUnitCommand(
            "Хмельницький",
            "Хмельницький регіон",
            "Хм");

        var newUnit = await Mediator.Invoke(createUnitCommand);

        var updateUnitCommand = new UpdateUnitCommand(
            newUnit.Id,
            "Хмельницький оновлено",
            "Хмельницький регіон оновлено",
            "Хмо");

        // Act
        var updatedUnit = await Mediator.Invoke(updateUnitCommand);

        // Assert
        updatedUnit.Should().NotBeNull();

        updatedUnit.Should().BeEquivalentTo(
            new UnitVm()
            {
                Id = updateUnitCommand.Id,
                Name = updateUnitCommand.Name,
                Description = updateUnitCommand.Description,
                Code = updateUnitCommand.Code,
                FullName = updateUnitCommand.Code + " " + updateUnitCommand.Name
            });
    }

    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
    {
        // Arrange
        var updateUnitCommand = new UpdateUnitCommand(
            1000,
            "Хмельницький оновлено",
            "Хмельницький регіон оновлено",
            "Хмо");

        // Act
        Func<Task> act = async () => await Mediator.Invoke(updateUnitCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}