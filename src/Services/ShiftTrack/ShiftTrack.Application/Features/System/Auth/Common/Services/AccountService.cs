using ShiftTrack.Application.Features.Organization.Employees.Common.Interfaces;
using ShiftTrack.Application.Features.System.Auth.Common.Dtos;
using ShiftTrack.Application.Features.System.Auth.Common.Interfaces;
using ShiftTrack.Application.Features.System.User.Common.Dtos;
using ShiftTrack.Application.Features.System.User.Common.Interfaces;
using ShiftTrack.Domain.Features.System.Auth.Models;

namespace ShiftTrack.Application.Features.System.Auth.Common.Services;

public class AccountService(
    IUserRepository userRepository,
    IEmployeeService employeeService) : IAccountService
{
    public Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken)
        => userRepository.RegisterUser(dto, cancellationToken);

    public Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken)
        => userRepository.UpdateUser(dto, cancellationToken);

    public async Task<Token> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetById(dto.EmployeeId, cancellationToken);

        var token = await userRepository.ChangePassword(
            new ChangeUserPasswordDto(
                employee.IntegrationId,
                dto.OldPassword,
                dto.NewPassword,
                dto.ConfirmPassword),
            cancellationToken);

        return token;
    }
}