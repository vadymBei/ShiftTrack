using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands
{
    public class CreateDepartmentCommandTests : BaseIntegrationTest
    {
        public CreateDepartmentCommandTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
        {
            // Arrange
            var createDepartmentCommand = new CreateDepartmentCommand(
                "ТЦ Либіль Плаза",
                1000);

            // Act
            Func<Task> act = async () => await Sender.Send(createDepartmentCommand);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task Create_ShouldAdd_NewDepartmentToDatabase()
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

            // Act
            var newDepartment = await Sender.Send(createDepartmentCommand);

            // Assert
            var department = DbContext.Departments.FirstOrDefault(x => x.Id == newDepartment.Id);

            department.Should().NotBeNull();
            department.UnitId.Should().Be(unit.Id);
            department.Should().BeEquivalentTo(new DepartmentVM()
            {
                Id = newDepartment.Id,
                Name = newDepartment.Name
            });
        }
    }
}
