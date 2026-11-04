namespace ShiftTrack.Application.Modules.Organization.Employees.Constants;

public static class EmployeeExceptionsLocalization
{
    public const string NO_PERMISSION_TO_EDIT_EMPLOYEES = "You don't have permission to edit employees.";
    public const string EMPLOYEE_OUT_OF_SCOPE = "You can only edit employees of your unit or employees without a department.";
    public const string DEPARTMENT_OUT_OF_SCOPE = "You can only move employees to departments of your unit.";
    public const string PHOTO_REQUIRED = "Choose a photo.";
    public const string PHOTO_TOO_LARGE = "The photo must be at most 5 MB.";
    public const string PHOTO_TYPE_NOT_ALLOWED = "The photo must be a JPEG, PNG or WebP image.";
    public const string PHOTO_NOT_FOUND = "The employee has no photo.";
    public const string PHOTO_STORAGE_UNAVAILABLE = "The photo can't be saved now: the storage is unavailable.";
    public const string TOO_MANY_EMPLOYEES = "Too many employees in one request.";
}
