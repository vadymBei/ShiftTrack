namespace ShiftTrack.Application.Common.Interfaces;

public interface IExcelFormatter<in T>
{
    byte[] Format(T data);
}