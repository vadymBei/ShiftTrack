namespace ShiftTrack.Core.Application.System.User.Common.Constants;

public static class UserExceptionsLocalization
{
    public const string EMPLOYEE_ROLE_ALREADY_EXISTS = "Employee role already exists.";
    public const string EMPLOYEE_ROLE_UNIT_ALREADY_EXISTS = "Employee role unit already exists.";
    public const string EMPLOYEE_ROLE_UNIT_DEPARTMENT_ALREADY_EXISTS = "Employee role unit department already exists.";
    public const string UNIT_DIRECTOR_OUT_OF_SCOPE = "You cannot assign roles to employees from another unit.";
    public const string UNIT_DIRECTOR_INVALID_ROLE = "You can only assign DEPARTMENT_ADMIN and DEPARTMENT_STYLIST roles.";
    public const string UNIT_DIRECTOR_GLOBAL_SCOPE = "You cannot assign roles globally.";
    public const string DELETE_ROLE_WRONG_UNIT = "You can only delete roles from employees in your unit";

}