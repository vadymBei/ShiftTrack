using ShiftTrack.Core.Unit.Tests.Builders;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Models;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Domain.System.User;

public class RoleScopeTests
{
    private const string UnitDirector = "unit.director";
    private const string DepartmentDirector = "department.director";

    private static Employee WorkingIn(long departmentId, long unitId) =>
        new EmployeeBuilder().WithId(500).WorkingIn(departmentId, unitId).Build();

    [Fact]
    public void UnitBindingWithoutDepartments_CoversWholeUnit()
    {
        var director = new EmployeeBuilder().WithRoleInUnit(UnitDirector, unitId: 1).Build();

        var scope = RoleScope.Of(director, UnitDirector);

        Assert.True(scope.Covers(WorkingIn(10, 1)));
        Assert.True(scope.Covers(WorkingIn(11, 1)));
        Assert.False(scope.Covers(WorkingIn(20, 2)));
    }

    [Fact]
    public void UnitBindingWithDepartments_CoversOnlyThoseDepartments()
    {
        var director = new EmployeeBuilder().WithRoleInDepartments(DepartmentDirector, 1, 10, 11).Build();

        var scope = RoleScope.Of(director, DepartmentDirector);

        Assert.True(scope.Covers(WorkingIn(10, 1)));
        Assert.True(scope.Covers(WorkingIn(11, 1)));
        Assert.False(scope.Covers(WorkingIn(12, 1)));
        Assert.True(scope.IsBoundTo(1));
        Assert.False(scope.IsBoundTo(2));
    }

    [Fact]
    public void OwnDepartment_DoesNotExtendScope()
    {
        var director = new EmployeeBuilder()
            .WorkingIn(departmentId: 99, unitId: 9)
            .WithRoleInUnit(UnitDirector, unitId: 1)
            .Build();

        var scope = RoleScope.Of(director, UnitDirector);

        Assert.False(scope.Covers(WorkingIn(99, 9)));
    }

    [Fact]
    public void BindingsOfOtherRoles_AreIgnored()
    {
        var employee = new EmployeeBuilder().WithRoleInUnit(DepartmentDirector, unitId: 1).Build();

        var scope = RoleScope.Of(employee, UnitDirector);

        Assert.True(scope.IsEmpty);
        Assert.False(scope.Covers(WorkingIn(10, 1)));
    }

    [Fact]
    public void RoleWithoutBindings_IsEmpty()
    {
        var director = new EmployeeBuilder().WithRoleWithoutBindings(UnitDirector).Build();

        Assert.True(RoleScope.Of(director, UnitDirector).IsEmpty);
    }

    [Fact]
    public void EmployeeWithoutLoadedRoles_IsEmpty()
    {
        Assert.True(RoleScope.Of(new Employee(), UnitDirector).IsEmpty);
        Assert.True(RoleScope.Of(null, UnitDirector).IsEmpty);
    }

    [Fact]
    public void SeveralBindings_AreCombined()
    {
        var director = new EmployeeBuilder()
            .WithRoleInUnit(UnitDirector, unitId: 1)
            .WithRoleInDepartments(UnitDirector, 2, 20)
            .Build();

        var scope = RoleScope.Of(director, UnitDirector);

        Assert.Equal([1], scope.UnitIds);
        Assert.Equal([20], scope.DepartmentIds);
        Assert.True(scope.Covers(WorkingIn(10, 1)));
        Assert.True(scope.Covers(WorkingIn(20, 2)));
        Assert.False(scope.Covers(WorkingIn(21, 2)));
    }
}
