using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.System.Auth.Account.UseCases.Commands.RemovePhoto;

public class RemovePhotoCommandHandler(
    ICurrentUserService currentUserService,
    IEmployeeRepository employeeRepository,
    IEmployeePhotoStorage photoStorage,
    IUnitOfWork unitOfWork) : IRequestHandler<RemovePhotoCommand>
{
    public async Task Handle(RemovePhotoCommand request, CancellationToken cancellationToken = default)
    {
        var employeeId = currentUserService.Employee.Id;
        var previous = (await employeeRepository.GetPhotoNames([employeeId], cancellationToken))
            .GetValueOrDefault(employeeId);

        if (string.IsNullOrEmpty(previous))
            return;

        await employeeRepository.UpdatePhoto(new UploadEmployeePhotoDto(employeeId, null), cancellationToken);

        await unitOfWork.AfterCommit(() =>
        {
            photoStorage.Delete(previous);
            return Task.CompletedTask;
        });
    }
}
