using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.UpdatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Queries.GetPositionById;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Positions.Commands;

public class UpdatePositionCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Update_ShouldReturnUpdatedPosition_WhenPositionExists()
    {
        // Arrange
        var position = await CreatePositionAsync();

        var updatePositionCommand = new UpdatePositionCommand(
            new PositionToUpdateDto(
                position.Id,
                Faker.Name.JobTitle(),
                Faker.Name.JobDescriptor(),
                Faker.Random.Decimal(100, 500)));

        // Act
        var updatedPosition = await Mediator.Send(updatePositionCommand);

        // Assert
        updatedPosition.Should().NotBeNull();
        updatedPosition.Should().BeEquivalentTo(
            new PositionVm()
            {
                Id = position.Id,
                Name = updatePositionCommand.Data.Name,
                Description = updatePositionCommand.Data.Description,
                HourlyRate = updatePositionCommand.Data.HourlyRate
            });
    }

    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenPositionNotExists()
    {
        // Arrange
        var updatePositionCommand = new UpdatePositionCommand(
            new PositionToUpdateDto(
                1000,
                Faker.Name.JobTitle(),
                Faker.Name.JobDescriptor(),
                150));

        // Act
        Func<Task> act = async () => await Mediator.Send(updatePositionCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }
}