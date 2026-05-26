namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;

public record PositionToUpdateDto(
    long Id,
    string Name,
    string Description,
    decimal HourlyRate);