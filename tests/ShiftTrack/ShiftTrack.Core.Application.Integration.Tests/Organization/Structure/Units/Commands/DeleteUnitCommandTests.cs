using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Units.Commands
{
    public class DeleteUnitCommandTests : BaseIntegrationTest
    {
        public DeleteUnitCommandTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Delete_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
        {
            // Arrange
            var deleteUnitCommand = new DeleteUnitCommand(1000);

            // Act
            Func<Task> act = async () => await Sender.Send(deleteUnitCommand);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task Delete_ShouldRemove_WhenUnitExists()
        {
            // Arrange
            var createUnitCommand = new CreateUnitCommand(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм");

            var newUnit = await Sender.Send(createUnitCommand);

            var deleteUnitCommand = new DeleteUnitCommand(newUnit.Id);

            // Act 
            await Sender.Send(deleteUnitCommand);

            // Assert
            var deletedUnit = DbContext.Units.FirstOrDefault(x => x.Id == newUnit.Id);

            deletedUnit.Should().BeNull();
        }        
    }
}
