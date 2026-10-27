using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Builders;

public class VacationBuilder
{
    private readonly Vacation _vacation = new()
    {
        Id = 100,
        Type = VacationType.YearMainVacation,
        StartDate = new DateTime(2026, 11, 2),
        EndDate = new DateTime(2026, 11, 15),
        DaysCount = 14
    };

    private VacationStatus _status = VacationStatus.None;

    public VacationBuilder Of(Employee employee)
    {
        _vacation.EmployeeId = employee.Id;
        _vacation.Employee = employee;
        return this;
    }

    public VacationBuilder AuthoredBy(long authorId)
    {
        _vacation.AuthorId = authorId;
        return this;
    }

    public VacationBuilder InStatus(VacationStatus status)
    {
        _status = status;
        return this;
    }

    /// <summary>The status is reached through the state machine, as in the application.</summary>
    public Vacation Build()
    {
        switch (_status)
        {
            case VacationStatus.None:
                break;
            case VacationStatus.PendingApproval:
                _vacation.Submit();
                break;
            case VacationStatus.ApprovedByDepartmentDirector:
                _vacation.Submit();
                _vacation.Approve(VacationApprovalLevel.DepartmentDirector);
                break;
            case VacationStatus.ApprovedByUnitDirector:
                _vacation.Submit();
                _vacation.Approve(VacationApprovalLevel.UnitDirector);
                break;
            case VacationStatus.Rejected:
                _vacation.Submit();
                _vacation.Reject(VacationApprovalLevel.DepartmentDirector);
                break;
        }

        return _vacation;
    }
}
