namespace ShiftTrack.Domain.Modules.Communication.Notifications.Enums;

public enum NotificationType
{
    None = 0,

    VacationPendingApproval = 100,
    VacationApproved = 101,
    VacationFinallyApproved = 102,
    VacationRejected = 103,
    VacationCreatedForYou = 104,
    VacationDeleted = 105,

    BusinessTripPendingApproval = 200,
    BusinessTripAdded = 201,
    BusinessTripApproved = 202,
    BusinessTripRejected = 203,
    BusinessTripChanged = 204,
    BusinessTripRemoved = 205,

    ShiftChanged = 300,
    ScheduleUpdated = 301,

    PayrollPaid = 400,

    RoleChanged = 500,
    EmployeeRegistered = 501,

    Mention = 600
}
