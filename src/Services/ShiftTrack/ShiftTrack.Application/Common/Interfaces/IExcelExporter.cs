namespace ShiftTrack.Application.Common.Interfaces;

public interface IExcelExporter<in T>
{
    byte[] Export(T data);
}