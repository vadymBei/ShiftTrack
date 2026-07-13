using FluentAssertions;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByUnitId;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Core.Application.Integration.Tests.Abstractions;
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
        Func<Task> act = async () => await Mediator.Send(getDepartmentsByUnitIdQuery);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task GetDepartmentsByUnitId_ShouldReturnOneDepartment_WhenUnitAndDepartmentExists()
    {
        // Arrange
        var unit = await CreateUnitAsync();
        var department = await CreateDepartmentAsync(unit.Id);

        var getDepartmentsByUnitIdQuery = new GetDepartmentsByUnitIdQuery(unit.Id);

        // Act
        var departments = (await Mediator.Send(getDepartmentsByUnitIdQuery)).ToList();

        // Assert
        departments.Should().NotBeNull();
        departments.Should().NotBeEmpty();
        var foundDepartment = departments.FirstOrDefault(x => x.Id == department.Id);
        foundDepartment.Should().NotBeNull();
        foundDepartment.Should().BeEquivalentTo(
            new DepartmentVm()
            {
                Id = department.Id,
                Name = department.Name,
                UnitId = department.UnitId,
                Unit = new UnitVm()
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