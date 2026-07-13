using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
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
            new UnitToCreateDto(
                Faker.Address.City(),
                Faker.Address.FullAddress(),
                Faker.Random.Replace("???").ToUpper()));

        // Act
        var newUnit = await Mediator.Send(createUnitCommand);

        // Assert
        var unit = await UnitRepository.GetById(newUnit.Id, CancellationToken.None);

        unit.Should().NotBeNull();
        unit.Should().BeEquivalentTo(
            new UnitVm()
            {
                Id = newUnit.Id,
                Name = createUnitCommand.Data.Name,
                Description = createUnitCommand.Data.Description,
                Code = createUnitCommand.Data.Code,
                FullName = createUnitCommand.Data.Code + " " + createUnitCommand.Data.Name,
            });
    }
}