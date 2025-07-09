namespace ShiftTrack.Domain.Common.Interfaces;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    long? CreatedById { get; set; }
    
    DateTime? ModifiedAt { get; set; }
    long? ModifiedById { get; set; }
}