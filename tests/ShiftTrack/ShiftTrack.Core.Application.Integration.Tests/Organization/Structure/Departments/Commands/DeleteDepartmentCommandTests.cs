using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.DeleteDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands
{
    public class DeleteDepartmentCommandTests : BaseIntegrationTest
    {
        public DeleteDepartmentCommandTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Delete_ShouldReturnEntityNotFoundException_WhenDepartmentNotFound()
        {
            // Arrange
            var deleteDepartmentCommand = new DeleteDepartmentCommand(1000);

            // Act
            Func<Task> act = async () => await Sender.Send(deleteDepartmentCommand);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task Delete_ShouldRemove_WhenDepartmentExists()
        {
            // Arrange
            var createUnitCommand = new CreateUnitCommand(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм");

            var unit = await Sender.Send(createUnitCommand);

            var createDepartmentCommand = new CreateDepartmentCommand(
                "ТЦ Либіль Плаза",
                unit.Id);

            var newDepartment = await Sender.Send(createDepartmentCommand);

            var deleteDepartmentCommand = new DeleteDepartmentCommand(newDepartment.Id);

            // Act
            await Sender.Send(deleteDepartmentCommand);

            // Assert
            var deletedDepartment = DbContext.Departments.FirstOrDefault(x => x.Id == newDepartment.Id);

            deletedDepartment.Should().BeNull();
        }
    }
}
