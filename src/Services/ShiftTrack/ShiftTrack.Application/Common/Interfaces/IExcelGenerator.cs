namespace ShiftTrack.Application.Common.Interfaces;

public interface IExcelGenerator
{
    byte[] Generate<T>(IExcelFormatter<T> formatter, T data);
}