using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands
{
    public class CreateDepartmentCommandTests : BaseIntegrationTest
    {
        public CreateDepartmentCommandTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_ShouldAdd_NewDepartmentToDatabase()
        {
            // Arrange
            var createDepartmentCommand = new CreateDepartmentCommand(
                "ТЦ Либіль Плаза");

            // Act
            var newDepartment = await Sender.Send(createDepartmentCommand);

            // Assert
            var department = DbContext.Departments.FirstOrDefault(x => x.Id == newDepartment.Id);

            department.Should().NotBeNull();
            department.Should().BeEquivalentTo(new DepartmentVM()
            {
                Id = newDepartment.Id,
                Name = newDepartment.Name
            });
        }
    }
}
