using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;
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