using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Queries
{
    public class GetDepartmentByIdQueryTests : BaseIntegrationTest
    {
        public GetDepartmentByIdQueryTests(
            IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetById_ShouldReturnEntityNotFoundException_WhenDepartmentNotFound()
        {
            // Arrange
            var getDepartmentByIdQuery = new GetDepartmentByIdQuery(1000);

            // Act
            Func<Task> act = async () => await Sender.Send(getDepartmentByIdQuery);

            // Assert
            await act.Should()
                .ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetById_ShouldReturnDepartment_WhenDepartmentExists()
        {
            // Arrange
            var createDepartmentCommand = new CreateDepartmentCommand(
                "ТЦ Либіль Плаза");

            var newDepartment = await Sender.Send(createDepartmentCommand);

            var getDepartmentByIdQuery = new GetDepartmentByIdQuery(newDepartment.Id);

            // Act
            var department = await Sender.Send(getDepartmentByIdQuery);

            // Assert
            department.Should().NotBeNull();
            department.Should().BeEquivalentTo(
                new DepartmentVM()
                {
                    Id = newDepartment.Id,
                    Name = newDepartment.Name
                });
        }
    }
}
