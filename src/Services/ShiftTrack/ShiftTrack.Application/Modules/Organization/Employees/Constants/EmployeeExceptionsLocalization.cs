namespace ShiftTrack.Application.Modules.Organization.Employees.Constants;

public static class EmployeeExceptionsLocalization
{
    public const string NO_PERMISSION_TO_EDIT_EMPLOYEES = "You don't have permission to edit employees.";
    public const string EMPLOYEE_OUT_OF_SCOPE = "You can only edit employees of your unit or employees without a department.";
    public const string DEPARTMENT_OUT_OF_SCOPE = "You can only move employees to departments of your unit.";
}
