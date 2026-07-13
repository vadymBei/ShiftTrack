namespace ShiftTrack.Kernel.CQRS.Interfaces;

/// <summary>
/// Маркерний інтерфейс для повідомлень (публікація до багатьох обробників).
/// На відміну від <see cref="IRequest"/>, notification не повертає відповідь
/// і може мати нуль або більше обробників.
/// </summary>
public interface INotification;