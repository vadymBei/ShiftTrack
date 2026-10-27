using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Communication.Notifications.Services;

public static class NotificationTextFormatter
{
    public const string BusinessTripFallbackRoute = "Відрядження";

    private static readonly string[] MonthsGenitive =
    [
        "січня", "лютого", "березня", "квітня", "травня", "червня",
        "липня", "серпня", "вересня", "жовтня", "листопада", "грудня"
    ];

    public static string Period(DateTime start, DateTime end)
    {
        if (start.Date == end.Date)
            return Day(start);

        if (start.Year == end.Year && start.Month == end.Month)
            return $"{start.Day}–{end.Day} {MonthsGenitive[end.Month - 1]}";

        return $"{Day(start)} – {Day(end)}";
    }

    public static string VacationType(VacationType type) => type switch
    {
        Domain.Modules.Booking.Vacations.Enums.VacationType.YearMainVacation => "Основна щорічна відпустка",
        Domain.Modules.Booking.Vacations.Enums.VacationType.BonusVacation => "Бонусна відпустка",
        Domain.Modules.Booking.Vacations.Enums.VacationType.VacationWithoutSalaryByFamily => "Відпустка без збереження з/п за сімейними обставинами",
        Domain.Modules.Booking.Vacations.Enums.VacationType.VacationWithoutSalaryByPregnancy => "Відпустка у зв’язку з вагітністю та пологами",
        Domain.Modules.Booking.Vacations.Enums.VacationType.VacationWithoutSalaryByChild3Years => "Відпустка для догляду за дитиною до 3 років",
        Domain.Modules.Booking.Vacations.Enums.VacationType.VacationWithoutSalaryByChild6Years => "Відпустка для догляду за дитиною до 6 років",
        _ => "Відпустка"
    };

    public static string Participants(IReadOnlyList<string> fullNames)
    {
        if (fullNames.Count == 0)
            return "Без учасників";

        return fullNames.Count == 1
            ? fullNames[0]
            : $"{fullNames[0]} +{fullNames.Count - 1}";
    }

    private static string Day(DateTime date) => $"{date.Day} {MonthsGenitive[date.Month - 1]}";
}
