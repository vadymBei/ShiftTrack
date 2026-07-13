using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Commands;

public class CreatePositionCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewPositionToDatabase()
    {
        // Arrange
        var createPositionCommand = new CreatePositionCommand(
            new PositionToCreateDto(
                Faker.Name.JobTitle(),
                Faker.Name.JobDescriptor(),
                Faker.Random.Decimal(100, 500)));

        // Act
        var newPosition = await Mediator.Send(createPositionCommand);

        // Assert
        var position = await PositionRepository.GetById(newPosition.Id, CancellationToken.None);

        position.Should().NotBeNull();
        position.Name.Should().Be(createPositionCommand.Data.Name);
        position.Description.Should().Be(createPositionCommand.Data.Description);
        position.HourlyRate.Should().Be(createPositionCommand.Data.HourlyRate);
    }
}