using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Domain.System.User;

public class EmployeeServiceStartTests
{
    [Fact]
    public void HireDate_IsTheStart()
    {
        var employee = new Employee { CreatedAt = new DateTime(2026, 10, 21, 9, 30, 0), HireDate = new DateTime(2024, 3, 1) };

        Assert.Equal(new DateTime(2024, 3, 1), employee.ServiceStart);
    }

    [Fact]
    public void WithoutHireDate_TheRegistrationDayIsTheStart()
    {
        var employee = new Employee { CreatedAt = new DateTime(2026, 10, 21, 9, 30, 0) };

        Assert.Equal(new DateTime(2026, 10, 21), employee.ServiceStart);
    }

    [Fact]
    public void RegistrationTime_InUtc_IsTakenAsTheLocalDay()
    {
        var createdAt = new DateTime(2026, 10, 20, 22, 30, 0, DateTimeKind.Utc);

        Assert.Equal(createdAt.ToLocalTime().Date, Employee.ServiceStartOf(null, createdAt));
    }

    [Fact]
    public void TimeOfTheHireDate_IsIgnored()
    {
        Assert.Equal(new DateTime(2024, 3, 1), Employee.ServiceStartOf(new DateTime(2024, 3, 1, 15, 0, 0), DateTime.UtcNow));
    }
}
