namespace ShiftTrack.Application.Common.Interfaces;

public interface IPdfGenerator
{
    Task<Stream> Generate<T>(IPdfFormatter<T> formatter, T data, CancellationToken cancellationToken);
}