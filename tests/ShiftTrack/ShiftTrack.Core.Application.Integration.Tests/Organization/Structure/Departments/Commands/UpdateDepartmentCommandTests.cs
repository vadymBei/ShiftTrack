using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.UpdateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands;

public class UpdateDepartmentCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Update_ShouldReturnEntityNotFoundException_WhenDepartmentNotFound()
    {
        // Arrange
        var updateDepartmentCommand = new UpdateDepartmentCommand(
            new DepartmentToUpdateDto(
                1000,
                Faker.Company.CompanyName()));

        // Act
        Func<Task> act = async () => await Mediator.Send(updateDepartmentCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedDepartment_WhenDepartmentExists()
    {
        // Arrange
        var unit = await CreateUnitAsync();
        var department = await CreateDepartmentAsync(unit.Id);

        var updateDepartmentCommand = new UpdateDepartmentCommand(
            new DepartmentToUpdateDto(
                department.Id,
                Faker.Company.CompanyName()));

        // Act
        var updatedDepartment = await Mediator.Send(updateDepartmentCommand);

        // Assert
        updatedDepartment.Should().NotBeNull();
        updatedDepartment.Should().BeEquivalentTo(
            new DepartmentVm()
            {
                Id = updateDepartmentCommand.Data.Id,
                Name = updateDepartmentCommand.Data.Name,
                UnitId = unit.Id,
                Unit = unit
            });
    }
}