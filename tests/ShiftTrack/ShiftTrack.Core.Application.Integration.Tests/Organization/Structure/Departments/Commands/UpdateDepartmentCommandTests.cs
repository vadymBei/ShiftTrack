using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.CreateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.UpdateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
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
                "ТЦ Либідь плаза оновлений"));

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
            new UnitToCreateDto(
                "Хмельницький",
                "Хмельницький регіон",
                "Хм"));

        var unit = await Mediator.Invoke(createUnitCommand);

        var createDepartmentCommand = new CreateDepartmentCommand(
            new DepartmentToCreateDto(
                "ТЦ Либіль Плаза",
                unit.Id));

        var newDepartment = await Mediator.Invoke(createDepartmentCommand);

        var updateDepartmentCommand = new UpdateDepartmentCommand(
            new DepartmentToUpdateDto(
                newDepartment.Id,
                "Либіль Плаза"));

        // Act
        var updatedDepartment = await Mediator.Invoke(updateDepartmentCommand);

        // Assert
        updatedDepartment.Should().NotBeNull();
        updatedDepartment.Should().BeEquivalentTo(
            new DepartmentVm()
            {
                Id = updateDepartmentCommand.Data.Id,
                Name = updateDepartmentCommand.Data.Name,
                UnitId = newDepartment.UnitId,
                Unit = new UnitVm()
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