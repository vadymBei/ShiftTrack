namespace ShiftTrack.Application.Features.Booking.Common.Constants;

public static class VacationExceptionsLocalization
{
    public const string CANNOT_DELETE_OTHERS_VACATION = "You can only delete your own vacations.";
    public const string CANNOT_EDIT_OTHERS_VACATION = "You can only edit your own vacations.";
    public const string CANNOT_EDIT_NON_PENDING_VACATION = "You can only edit vacations that are pending approval.";
    public const string CANNOT_VIEW_OTHERS_VACATION = "You can only view vacations where you are the author.";
    public const string CANNOT_REJECT_VACATION = "Regular users cannot reject vacations.";
    public const string CANNOT_APPROVE_VACATION = "You don't have permission to approve vacations.";
    public const string CANNOT_VIEW_VACATION_FROM_OTHER_DEPARTMENT = "You can only view vacations from your department.";
    public const string CANNOT_APPROVE_UNIT_DIRECTOR_APPROVED_VACATION = "Cannot approve vacation that has already been approved by Unit Director.";
    public const string CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_DEPARTMENT = "You can only change vacation status for employees from your department.";
    public const string CANNOT_CHANGE_VACATION_STATUS_FROM_OTHER_UNIT = "You can only change vacation status for employees from your unit.";
    public const string CANNOT_VIEW_VACATION_FROM_OTHER_UNIT = "You can only view vacations from your unit.";


}