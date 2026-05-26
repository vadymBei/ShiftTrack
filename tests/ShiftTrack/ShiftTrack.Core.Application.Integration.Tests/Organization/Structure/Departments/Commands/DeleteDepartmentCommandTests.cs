using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.DeleteDepartment;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands;

public class DeleteDepartmentCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Delete_ShouldReturnEntityNotFoundException_WhenDepartmentNotFound()
    {
        // Arrange
        var deleteDepartmentCommand = new DeleteDepartmentCommand(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(deleteDepartmentCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Delete_ShouldRemove_WhenDepartmentExists()
    {
        // Arrange
        var unit = await CreateUnitAsync();
        var department = await CreateDepartmentAsync(unit.Id);

        var deleteDepartmentCommand = new DeleteDepartmentCommand(department.Id);

        // Act
        await Mediator.Invoke(deleteDepartmentCommand);

        // Assert
        Func<Task> act = async () => await DepartmentRepository.GetById(department.Id, CancellationToken.None);
        await act.Should().ThrowAsync<EntityNotFoundException>();
    }
}