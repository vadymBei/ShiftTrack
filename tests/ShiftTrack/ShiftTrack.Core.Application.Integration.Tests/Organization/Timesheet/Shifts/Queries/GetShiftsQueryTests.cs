using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Queries.GetShifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Queries
{
    public class GetShiftsQueryTests : BaseIntegrationTest
    {
        public GetShiftsQueryTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetShifts_ShouldReturnOneShift_WhenShiftExists()
        {
            // Arrange
            var shiftsToDelete = DbContext.Shifts.ToList();
            DbContext.Shifts.RemoveRange(shiftsToDelete);

            var createShiftCommand = new CreateShiftCommand(
               "ВХ",
               "Вихідний",
               "#FFFFF",
               ShiftType.DayOff);

            var newShift = await Sender.Send(createShiftCommand);

            var getShiftsQuery = new GetShiftsQuery();

            // Act
            var shifts = await Sender.Send(getShiftsQuery);

            // Assert
            shifts.Should().NotBeNullOrEmpty();

            shifts.Should().HaveCount(1);

            shifts.Should().Contain(x => x.Id == newShift.Id);
            shifts.Should().Contain(x => x.Code ==  newShift.Code);
            shifts.Should().Contain(x => x.Description ==  newShift.Description);
            shifts.Should().Contain(x => x.Color ==  newShift.Color);
            shifts.Should().Contain(x => x.Type ==  newShift.Type);
        }
    }
}
