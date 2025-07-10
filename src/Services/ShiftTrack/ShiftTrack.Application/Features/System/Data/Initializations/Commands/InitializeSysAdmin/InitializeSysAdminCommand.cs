using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Data.Initializations.Commands.InitializeSysAdmin;

public sealed record InitializeSysAdminCommand() : IRequest;