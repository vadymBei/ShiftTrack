using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Exceptions;
using ShiftTrack.Application.Modules.Organization.Employees.Services;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.System.User.EmployeeRoles.Services;
using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Application.Modules.System.User.Roles.Constants;
using ShiftTrack.Domain.Modules.Organization.Structure.Entities;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Models;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Organization.Employees;

public class EmployeeEditPermissionTests
{
    private const long Unit = 1;
    private const long OtherUnit = 2;

    private readonly Mock<IDepartmentRepository> _departments = new();

    public EmployeeEditPermissionTests()
    {
        _departments
            .Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((long id, CancellationToken _) => new Department { Id = id, UnitId = id >= 20 ? OtherUnit : Unit });
    }

    [Fact]
    public async Task SysAdmin_EditsAnyone()
    {
        var permission = For(new EmployeeBuilder().WithRoleWithoutBindings(DefaultRolesCatalog.SYS_ADMIN).Build());

        await permission.EnsureCanEdit(Worker(departmentId: 20, unitId: OtherUnit), newDepartmentId: 21, CancellationToken.None);
    }

    [Fact]
    public async Task EmployeeWithoutRole_CannotEdit()
    {
        var permission = For(new EmployeeBuilder().WorkingIn(10, Unit).Build());

        var exception = await Assert.ThrowsAsync<EmployeeException>(() =>
            permission.EnsureCanEdit(Worker(10, Unit), 10, CancellationToken.None));

        Assert.Equal("ORG_EMP_NO_PERMISSION_TO_EDIT_EMPLOYEES", exception.ErrorType);
        Assert.Equal(HttpStatusCode.Forbidden, exception.Code);
    }

    [Fact]
    public async Task DepartmentDirector_CannotEdit()
    {
        var permission = For(new EmployeeBuilder().WithRoleInDepartments(DefaultRolesCatalog.DEPARTMENT_DIRECTOR, Unit, 10).Build());

        var exception = await Assert.ThrowsAsync<EmployeeException>(() =>
            permission.EnsureCanEdit(Worker(10, Unit), 10, CancellationToken.None));

        Assert.Equal("ORG_EMP_NO_PERMISSION_TO_EDIT_EMPLOYEES", exception.ErrorType);
    }

    [Fact]
    public async Task UnitDirector_EditsTheirUnit_AndMovesWithinIt()
    {
        var permission = For(UnitDirector());

        await permission.EnsureCanEdit(Worker(10, Unit), 11, CancellationToken.None);
    }

    [Fact]
    public async Task UnitDirector_AssignsAnEmployeeWithoutDepartment()
    {
        var permission = For(UnitDirector());

        await permission.EnsureCanEdit(new Employee { Id = 7 }, 10, CancellationToken.None);
    }

    [Fact]
    public async Task UnitDirector_CannotEditAnotherUnit()
    {
        var permission = For(UnitDirector());

        var exception = await Assert.ThrowsAsync<EmployeeException>(() =>
            permission.EnsureCanEdit(Worker(20, OtherUnit), 20, CancellationToken.None));

        Assert.Equal("ORG_EMP_EMPLOYEE_OUT_OF_SCOPE", exception.ErrorType);
    }

    [Fact]
    public async Task UnitDirector_CannotMoveToAnotherUnit()
    {
        var permission = For(UnitDirector());

        var exception = await Assert.ThrowsAsync<EmployeeException>(() =>
            permission.EnsureCanEdit(Worker(10, Unit), 20, CancellationToken.None));

        Assert.Equal("ORG_EMP_DEPARTMENT_OUT_OF_SCOPE", exception.ErrorType);
    }

    [Fact]
    public async Task UnitDirector_BoundToSomeDepartments_OnlyThose()
    {
        var permission = For(new EmployeeBuilder()
            .WithRoleInDepartments(DefaultRolesCatalog.UNIT_DIRECTOR, Unit, 10)
            .Build());

        await permission.EnsureCanEdit(Worker(10, Unit), null, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<EmployeeException>(() =>
            permission.EnsureCanEdit(Worker(11, Unit), null, CancellationToken.None));
        Assert.Equal("ORG_EMP_EMPLOYEE_OUT_OF_SCOPE", exception.ErrorType);
    }

    private static Employee UnitDirector() =>
        new EmployeeBuilder().WithId(99).WithRoleInUnit(DefaultRolesCatalog.UNIT_DIRECTOR, Unit).Build();

    private static Employee Worker(long departmentId, long unitId) =>
        new EmployeeBuilder().WithId(5).WorkingIn(departmentId, unitId).Build();

    private EmployeeEditPermission For(Employee currentUser)
    {
        var current = new Mock<ICurrentUserService>();
        current.Setup(x => x.Employee).Returns(currentUser);

        var scopes = new Mock<IRoleScopeService>();
        scopes.Setup(x => x.GetCurrentScope(It.IsAny<string>()))
            .Returns((string role) => RoleScope.Of(currentUser, role));

        return new EmployeeEditPermission(current.Object, new EmployeeRoleChecker(current.Object), scopes.Object,
            _departments.Object, NullLogger<EmployeeEditPermission>.Instance);
    }
}
