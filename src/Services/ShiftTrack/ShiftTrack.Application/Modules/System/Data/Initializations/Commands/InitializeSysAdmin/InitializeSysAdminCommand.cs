using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Data.Initializations.Commands.InitializeSysAdmin;

public sealed record InitializeSysAdminCommand() : IRequest;