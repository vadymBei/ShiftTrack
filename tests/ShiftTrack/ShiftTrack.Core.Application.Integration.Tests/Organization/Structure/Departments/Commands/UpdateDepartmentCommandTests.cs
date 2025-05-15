using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
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
            1000,
            "ТЦ Либідь плаза оновлений");

        // Act
        Func<Task> act = async () => await Mediator.Invoke(updateDepartmentCommand);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedDepartment_WhenDepartmentExists()
    {
        // Arrange
        var createUnitCommand = new CreateUnitCommand(
            "Хмельницький",
            "Хмельницький регіон",
            "Хм");

        var unit = await Mediator.Invoke(createUnitCommand);

        var createDepartmentCommand = new CreateDepartmentCommand(
            "ТЦ Либіль Плаза", 
            unit.Id);

        var newDepartment = await Mediator.Invoke(createDepartmentCommand);

        var updateDepartmentCommand = new UpdateDepartmentCommand(
            newDepartment.Id,
            "Либіль Плаза");

        // Act
        var updatedDepartment = await Mediator.Invoke(updateDepartmentCommand);

        // Assert
        updatedDepartment.Should().NotBeNull();
        updatedDepartment.Should().BeEquivalentTo(
            new DepartmentVM()
            {
                Id = updateDepartmentCommand.Id,
                Name = updateDepartmentCommand.Name,
                UnitId = newDepartment.UnitId,
                Unit = new UnitVM()
                {
                    Id = newDepartment.Unit.Id,
                    Name = newDepartment.Unit.Name,
                    Code = newDepartment.Unit.Code,
                    Description = newDepartment.Unit.Description,
                    FullName = newDepartment.Unit.FullName
                }
            });
    }
}