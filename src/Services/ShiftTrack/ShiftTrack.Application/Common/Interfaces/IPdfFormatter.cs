namespace ShiftTrack.Application.Common.Interfaces;

public interface IPdfFormatter<in T>
{
    string Format(T data);
}