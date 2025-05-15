using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Commands;

public class CreateDepartmentCommandTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Create_ShouldAdd_NewDepartmentToDatabase()
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

        // Act
        var newDepartment = await Mediator.Invoke(createDepartmentCommand);

        // Assert
        var department = DbContext.Departments
            .Include(x => x.Unit)
            .FirstOrDefault(x => x.Id == newDepartment.Id);

        department.Should().NotBeNull();
        department.Should().BeEquivalentTo(new DepartmentVM()
        {
            Id = newDepartment.Id,
            Name = newDepartment.Name,
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