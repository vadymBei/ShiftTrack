namespace ShiftTrack.Application.Common.Interfaces;

public interface IPdfExporter<in T>
{
    Task<Stream> Export(T data, CancellationToken cancellationToken);
}