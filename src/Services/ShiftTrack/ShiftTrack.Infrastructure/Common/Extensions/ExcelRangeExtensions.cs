using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ShiftTrack.Infrastructure.Common.Extensions;

public static class ExcelRangeExtensions
{
    public static void SetRangeBorder(this ExcelRange range)
    {
        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
    }
}