using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;
using ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Timesheet.Shifts.Commands
{
    public class DeleteShiftCommandTests : BaseIntegrationTest
    {
        public DeleteShiftCommandTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Delete_ShouldReturnEntityNotFoundException_WhenShiftNotFound()
        {
            // Arrange
            var deleteShiftCommand = new DeleteShiftCommand(1000);

            // Act
            Func<Task> act = async () => await Sender.Send(deleteShiftCommand);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task Delete_ShouldDelete_WhenShiftExists()
        {
            // Arrange
            var createShiftCommand = new CreateShiftCommand(
                "ВХ",
                "Вихідний",
                "#FFFFF",
                ShiftType.DayOff);

            var shift = await Sender.Send(createShiftCommand);

            var deleteShiftCommand = new DeleteShiftCommand(shift.Id);

            // Act
            await Sender.Send(deleteShiftCommand);

            // Assert
            var deletedShift = DbContext.Shifts.FirstOrDefault(x => x.Id == shift.Id);

            deletedShift.Should().BeNull();
        }
    }
}
