using FluentAssertions;
using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentById;
using ShiftTrack.Application.Features.Organization.Structure.Units.Commands.CreateUnit;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Integration.Tests.Organization.Structure.Departments.Queries;

public class GetDepartmentByIdQueryTests(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task GetById_ShouldReturnEntityNotFoundException_WhenDepartmentNotFound()
    {
        // Arrange
        var getDepartmentByIdQuery = new GetDepartmentByIdQuery(1000);

        // Act
        Func<Task> act = async () => await Mediator.Invoke(getDepartmentByIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetById_ShouldReturnDepartment_WhenDepartmentExists()
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

        var getDepartmentByIdQuery = new GetDepartmentByIdQuery(newDepartment.Id);

        // Act
        var department = await Mediator.Invoke(getDepartmentByIdQuery);

        // Assert
        department.Should().NotBeNull();
        department.Should().BeEquivalentTo(
            new DepartmentVm()
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