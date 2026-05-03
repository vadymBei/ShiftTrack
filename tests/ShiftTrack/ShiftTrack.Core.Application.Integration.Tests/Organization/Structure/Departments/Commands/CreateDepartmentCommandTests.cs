using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.CreateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands;

public class CreateDepartmentCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewDepartmentToDatabase()
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

        // Act
        var newDepartment = await Mediator.Invoke(createDepartmentCommand);

        // Assert
        var department = DbContext.Departments
            .Include(x => x.Unit)
            .FirstOrDefault(x => x.Id == newDepartment.Id);

        department.Should().NotBeNull();
        department.Should().BeEquivalentTo(new DepartmentVm()
        {
            Id = newDepartment.Id,
            Name = newDepartment.Name,
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