using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentsByUnitId;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Queries;

public class GetDepartmentsByUnitIdQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetDepartmentsByUnitId_ShouldReturnEntityNotFoundException_WhenUnitNotFound()
    {
        // Arrange
        var getDepartmentsByUnitIdQuery = new GetDepartmentsByUnitIdQuery(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(getDepartmentsByUnitIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetDepartmentsByUnitId_ShouldReturnOneDepartment_WhenUnitAndDepartmentExists()
    {
        // Arrange
        var departmentsToRemove = DbContext.Departments.ToList();
        DbContext.Departments.RemoveRange(departmentsToRemove);

        var createUnitCommand = new CreateUnitCommand(
           "Хмельницький",
           "Хмельницький регіон",
           "Хм");

        var unit = await Sender.Send(createUnitCommand);

        var createDepartmentCommand = new CreateDepartmentCommand(
            "ТЦ Либіль Плаза",
            unit.Id);

        var department = await Mediator.Invoke(createDepartmentCommand);

        var getDepartmentsByUnitIdQuery = new GetDepartmentsByUnitIdQuery(unit.Id);

        // Act
        var departments = await Mediator.Invoke(getDepartmentsByUnitIdQuery);

        // Assert
        departments.Should().NotBeNull();
        departments.Should().NotBeEmpty();
        departments.FirstOrDefault().Should().BeEquivalentTo(
            new DepartmentVM()
            {
                Id = department.Id,
                Name = department.Name,
                UnitId = department.UnitId,
                Unit = new UnitVM()
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    Code = unit.Code,
                    Description = unit.Description,
                    FullName = unit.FullName
                }
            });
    }
}