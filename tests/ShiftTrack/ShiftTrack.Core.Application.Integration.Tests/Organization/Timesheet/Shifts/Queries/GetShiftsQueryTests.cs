using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Queries.GetShifts;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Queries;

public class GetShiftsQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetShifts_ShouldReturnShifts_WhenShiftsExist()
    {
        // Arrange
        var shift = await CreateShiftAsync();

        var query = new GetShiftsQuery();

        // Act
        var shifts = await Mediator.Send(query);

        // Assert
        shifts.Should().NotBeNull();
        shifts.Should().Contain(x => x.Id == shift.Id);
        
        var foundShift = shifts.First(x => x.Id == shift.Id);
        foundShift.Code.Should().Be(shift.Code);
        foundShift.Description.Should().Be(shift.Description);
    }
}
