using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ShiftTrack.Infrastructure.Persistence.Conventions;

/// <summary>
/// Points in time ("...At": CreatedAt, ModifiedAt, DeletedAt, ReadAt, PaidAt...) are stored in UTC.
/// The "timestamp without time zone" columns lose DateTimeKind, so values read back are marked as UTC
/// and the API serializes them with "Z". Calendar dates (StartDate, EndDate, Date, DateOfBirth)
/// are not affected.
/// </summary>
public static class UtcDateTimeConvention
{
    private const string PointInTimeSuffix = "At";

    private static readonly ValueConverter<DateTime, DateTime> UtcConverter = new(
        value => value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value,
        value => DateTime.SpecifyKind(value, DateTimeKind.Utc));

    public static void ApplyUtcDateTimeConvention(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var isDateTime = property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?);

                if (isDateTime && property.Name.EndsWith(PointInTimeSuffix, StringComparison.Ordinal))
                {
                    property.SetValueConverter(UtcConverter);
                }
            }
        }
    }
}
