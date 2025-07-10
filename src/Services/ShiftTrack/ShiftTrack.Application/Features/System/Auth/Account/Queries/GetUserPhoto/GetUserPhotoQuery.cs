using ShiftTrack.Application.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.System.Auth.Account.Queries.GetUserPhoto;

public record GetUserPhotoQuery(
    long EmployeeId) : IRequest<DocumentVm>;