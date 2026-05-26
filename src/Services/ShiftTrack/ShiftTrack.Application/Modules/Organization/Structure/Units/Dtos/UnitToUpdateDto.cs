namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;

public record UnitToUpdateDto(
    long Id,
    string Name,
    string Description,
    string Code);