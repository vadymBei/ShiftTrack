using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Commands
{
    public class UpdateUnitCommandTests : BaseIntegrationTest
    {
        public UpdateUnitCommandTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Update_ShoulReturnUpdatedUnit_WhenUnitExists()
        {
            // Arrange
            var createUnitCommand = new CreateUnitCommand(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм");

            var newUnit = await Sender.Send(createUnitCommand);

            var updateUnitCommand = new UpdateUnitCommand(
                newUnit.Id,
                "Хмельницький оновлено",
                "Хмельницький регіон оновлено",
                "Хмо");

            // Act
            var updatedUnit = await Sender.Send(updateUnitCommand);

            // Assert
            updatedUnit.Should().NotBeNull();

            updatedUnit.Should().BeEquivalentTo(
                new UnitVM()
                {
                    Id = updateUnitCommand.Id,
                    Name = updateUnitCommand.Name,
                    Description = updateUnitCommand.Description,
                    Code = updateUnitCommand.Code,
                    FullName = updateUnitCommand.Code + " " + updateUnitCommand.Name,
                    Departments = new List<DepartmentVM>()
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
            Func<Task> act = async () => await Sender.Send(updateUnitCommand);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }
    }
}
