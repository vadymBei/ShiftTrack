using ShiftTrack.Core.Application.Data.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.System.User.Employees.Queries.GetUserPhoto;

public record GetUserPhotoQuery(
    long EmployeeId) : IRequest<DocumentVm>;