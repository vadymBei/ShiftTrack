namespace ShiftTrack.Application.Modules.Organization.Employees.Constants;

public static class EmployeePhotoConstants
{
    public const long MaxSize = 5 * 1024 * 1024;

    /// <summary>How many employees one request for photo versions may ask about.</summary>
    public const int MaxEmployeesPerRequest = 500;
}
