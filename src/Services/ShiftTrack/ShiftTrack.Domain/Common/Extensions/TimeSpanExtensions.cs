namespace ShiftTrack.Domain.Common.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan Sum(this IEnumerable<TimeSpan> source)
    {
        return source.Aggregate(TimeSpan.Zero, (subtotal, time) => subtotal.Add(time));
    }

    public static TimeSpan Sum(this IEnumerable<TimeSpan?> source)
    {
        return source.Aggregate(TimeSpan.Zero, (subtotal, time) => subtotal.Add(time.Value));
    }
}