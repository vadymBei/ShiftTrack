using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Application.Modules.Communication.Messenger.Services;

/// <summary>Texts of the system messages about changes of a group, with the verb in the actor's gender.</summary>
public static class SystemMessageTexts
{
    public static string Created(Employee actor, string title) =>
        $"{actor?.FullName} {Verb(actor, "створив", "створила", "створив(-ла)")} групу «{title}»";

    public static string Renamed(Employee actor, string title) =>
        $"{actor?.FullName} {Verb(actor, "змінив", "змінила", "змінив(-ла)")} назву групи на «{title}»";

    public static string Added(Employee actor, IEnumerable<Employee> employees) =>
        $"{actor?.FullName} {Verb(actor, "додав", "додала", "додав(-ла)")} до групи: {Names(employees)}";

    public static string Removed(Employee actor, Employee employee) =>
        $"{actor?.FullName} {Verb(actor, "видалив", "видалила", "видалив(-ла)")} з групи: {employee?.FullName}";

    public static string Left(Employee actor) =>
        $"{actor?.FullName} {Verb(actor, "вийшов", "вийшла", "вийшов(-ла)")} з групи";

    public static string NewOwner(Employee owner) =>
        $"{owner?.FullName} тепер {Verb(owner, "власник", "власниця", "власник(-ця)")} групи";

    /// <summary>The title of the notification about a mention.</summary>
    public static string Mentioned(Employee actor, string title) =>
        $"{actor?.FullName ?? "Колега"} {Verb(actor, "згадав", "згадала", "згадав(-ла)")} вас у «{title}»";

    private static string Names(IEnumerable<Employee> employees) =>
        string.Join(", ", employees.Select(x => x.FullName));

    private static string Verb(Employee employee, string male, string female, string unknown) =>
        employee?.Gender switch
        {
            EmployeeGender.Male => male,
            EmployeeGender.Female => female,
            _ => unknown
        };
}
