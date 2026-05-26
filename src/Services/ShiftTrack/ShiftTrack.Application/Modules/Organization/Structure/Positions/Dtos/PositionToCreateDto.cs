namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;

public record PositionToCreateDto(
    string Name,
    string Description,
    decimal HourlyRate);