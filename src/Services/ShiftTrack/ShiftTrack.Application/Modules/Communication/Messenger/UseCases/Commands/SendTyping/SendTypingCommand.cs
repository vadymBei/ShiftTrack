using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Communication.Messenger.UseCases.Commands.SendTyping;

public record SendTypingCommand(
    long Id) : IRequest;
