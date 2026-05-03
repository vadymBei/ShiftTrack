using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands;

public class CreateShiftCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewShiftToDatabase()
    {
        // Arrange
        var command = new CreateShiftCommand(
            new ShiftToCreateDto(
                Faker.Random.Replace("??").ToUpper(),
                Faker.Commerce.ProductName(),
                Faker.Internet.Color(),
                ShiftType.Workday,
                new TimeSpan(09, 30, 00),
                new TimeSpan(21, 00, 00)));

        // Act
        var result = await Mediator.Invoke(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new
        {
            command.Data.Code,
            command.Data.Description,
            command.Data.Color,
            command.Data.Type,
            command.Data.StartTime,
            command.Data.EndTime,
            WorkHours = command.Data.EndTime - command.Data.StartTime
        });

        var shiftInDb = await ShiftRepository.GetById(result.Id, CancellationToken.None);

        shiftInDb.Should().NotBeNull();
        shiftInDb.Should().BeEquivalentTo(new
            {
                result.Id,
                command.Data.Code,
                command.Data.Description,
                command.Data.Color,
                command.Data.Type,
                command.Data.StartTime,
                command.Data.EndTime,
                IsDeleted = false,
                DeletedAt = (DateTime?)null,
                WorkHours = command.Data.EndTime - command.Data.StartTime
            },
            options => options
                .ExcludingMissingMembers()
                .Using<DateTime>(ctx => ctx.Subject.Should()
                    .BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());
    }

    [Fact]
    public async Task Create_WithInvalidTimeRange_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateShiftCommand(
            new ShiftToCreateDto(
                Faker.Random.Replace("??").ToUpper(),
                Faker.Commerce.ProductName(),
                Faker.Internet.Color(),
                ShiftType.Workday,
                new TimeSpan(21, 00, 00),
                new TimeSpan(09, 30, 00)));

        // Act
        Func<Task> act = () => Mediator.Invoke(command);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}