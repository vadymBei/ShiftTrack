using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.UpdateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class UpdateShiftCommandTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenShiftNotFound()
    {
        // Arrange
        var updateShiftCommand = new UpdateShiftCommand(
            new ShiftToUpdateDto(
                1000,
                Faker.Random.Replace("??"),
                Faker.Commerce.ProductName(),
                Faker.Internet.Color(),
                Faker.PickRandom<ShiftType>(),
                new TimeSpan(09, 30, 00),
                new TimeSpan(21, 00, 00)));

        // Act
        Func<Task> act = async () => await Mediator.Invoke(updateShiftCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldUpdate_WhenShiftExists()
    {
        // Arrange
        var newShift = await CreateShiftAsync();

        var updateShiftCommand = new UpdateShiftCommand(
            new ShiftToUpdateDto(
                newShift.Id,
                Faker.Random.Replace("??"),
                Faker.Commerce.ProductName(),
                Faker.Internet.Color(),
                Faker.PickRandom<ShiftType>(),
                new TimeSpan(10, 00, 00),
                new TimeSpan(19, 00, 00)));

        // Act
        var updatedShift = await Mediator.Invoke(updateShiftCommand);

        // Assert
        updatedShift.Should().NotBeNull();

        updatedShift.Should().BeEquivalentTo(
            new ShiftVm()
            {
                Id = updateShiftCommand.Data.Id,
                Code = updateShiftCommand.Data.Code,
                Description = updateShiftCommand.Data.Description,
                Color = updateShiftCommand.Data.Color,
                Type = updateShiftCommand.Data.Type,
                StartTime = updateShiftCommand.Data.StartTime,
                EndTime = updateShiftCommand.Data.EndTime,
                WorkHours = updateShiftCommand.Data.EndTime - updateShiftCommand.Data.StartTime
            });
    }
}