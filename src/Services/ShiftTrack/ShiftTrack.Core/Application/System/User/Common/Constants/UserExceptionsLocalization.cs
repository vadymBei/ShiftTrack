namespace ShiftTrack.Core.Application.System.User.Common.Constants;

public static class UserExceptionsLocalization
{
    public const string EMPLOYEE_ROLE_ALREADY_EXISTS = "Employee role already exists.";
    public const string EMPLOYEE_ROLE_UNIT_ALREADY_EXISTS = "Employee role unit already exists.";
    public const string EMPLOYEE_ROLE_UNIT_DEPARTMENT_ALREADY_EXISTS = "Employee role unit department already exists.";
    public const string CREATE_ROLE_UNIT_OUT_OF_SCOPE = "You cannot assign roles to employees from another unit.";
    public const string CREATE_ROLE_UNIT_DEPARTMENT_OUT_OF_SCOPE = "You cannot assign roles to employees from another department.";
    public const string UNIT_DIRECTOR_INVALID_ROLE = "You can only assign DEPARTMENT_ADMIN or DEPARTMENT_STYLIST or DEPARTMENT_DIRECTOR roles.";
    public const string DEPARTMENT_DIRECTOR_INVALID_ROLE = "You can only assign DEPARTMENT_ADMIN or DEPARTMENT_STYLIST roles";
    public const string CANNOT_ASSIGN_GLOBAL_SCOPE = "You cannot assign roles globally.";
    public const string DELETE_ROLE_WRONG_UNIT = "You can only delete roles from employees in your unit";
    public const string DELETE_ROLE_WRONG_DEPARTMENT = "You can only delete roles from employees in your department";

}