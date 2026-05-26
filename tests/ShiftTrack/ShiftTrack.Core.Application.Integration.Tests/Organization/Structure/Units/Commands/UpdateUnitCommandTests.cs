using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.UpdateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
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
        var newUnit = await CreateUnitAsync();

        var updateUnitCommand = new UpdateUnitCommand(
            new UnitToUpdateDto(
                newUnit.Id,
                Faker.Address.City(),
                Faker.Address.FullAddress(),
                Faker.Address.CountryCode()));

        // Act
        var updatedUnit = await Mediator.Invoke(updateUnitCommand);

        // Assert
        updatedUnit.Should().NotBeNull();

        updatedUnit.Should().BeEquivalentTo(
            new UnitVm()
            {
                Id = updateUnitCommand.Data.Id,
                Name = updateUnitCommand.Data.Name,
                Description = updateUnitCommand.Data.Description,
                Code = updateUnitCommand.Data.Code,
                FullName = updateUnitCommand.Data.Code + " " + updateUnitCommand.Data.Name
            });
    }

    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
    {
        // Arrange
        var updateUnitCommand = new UpdateUnitCommand(
            new UnitToUpdateDto(
                1000,
                Faker.Address.City(),
                Faker.Address.FullAddress(),
                Faker.Address.CountryCode()));

        // Act
        Func<Task> act = async () => await Mediator.Invoke(updateUnitCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}