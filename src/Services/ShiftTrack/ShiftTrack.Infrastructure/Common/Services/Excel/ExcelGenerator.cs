using ShiftTrack.Application.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Common.Services.Excel;

public class ExcelGenerator : IExcelGenerator
{
    public byte[] Generate<T>(IExcelFormatter<T> formatter, T data)
    {
        return formatter.Format(data);
    }
}