using FluentAssertions;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;
using ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById;
using ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit;
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

        var unit = await Sender.Send(createUnitCommand);

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
            new DepartmentVM()
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