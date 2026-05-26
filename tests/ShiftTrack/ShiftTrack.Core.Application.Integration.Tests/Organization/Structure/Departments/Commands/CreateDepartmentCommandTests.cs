using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.CreateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands;

public class CreateDepartmentCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewDepartmentToDatabase()
    {
        // Arrange
        var unit = await CreateUnitAsync();

        var createDepartmentCommand = new CreateDepartmentCommand(
            new DepartmentToCreateDto(
                Faker.Company.CompanyName(),
                unit.Id));

        // Act
        var newDepartment = await Mediator.Invoke(createDepartmentCommand);

        // Assert
        var department = await DepartmentRepository.GetById(newDepartment.Id, CancellationToken.None);

        department.Should().NotBeNull();
        department.Should().BeEquivalentTo(
            new DepartmentVm()
            {
                Id = newDepartment.Id,
                Name = createDepartmentCommand.Data.Name,
                UnitId = unit.Id,
                Unit = unit
            });
    }
}