using Microsoft.EntityFrameworkCore;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.Tokens.Models;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Models;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Common.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _applicationDbContext;

        public EmployeeService(
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            IApplicationDbContext applicationDbContext)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Token> ChangePassword(ChangeEmployeePasswordDto dto, CancellationToken cancellationToken)
        {
            var employee = await GetById(dto.EmployeeId, cancellationToken);

            var token = await _userRepository.ChangePassword(
                new ChangeUserPasswordDto(
                    employee.IntegrationId,
                    dto.OldPassword,
                    dto.NewPassword,
                    dto.ConfirmPassword),
                cancellationToken);

            return token;
        }

        public async Task<Employee> GetById(object id, CancellationToken cancellationToken)
        {
            long employeeId = (long)id;

            var employee = await _applicationDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Department)
                    .ThenInclude(x => x.Unit)
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);

            if (employee == null)
            {
                throw new EntityNotFoundException(typeof(Employee), employeeId);
            }

            return employee;
        }

        public async Task<CurrentUser> GetCurrentUser(CancellationToken cancellationToken)
        {
            var currentUser = _currentUserService.User;

            var employee = await _applicationDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Department)
                    .ThenInclude(x => x.Unit)
                .Include(x => x.Position)
                .FirstOrDefaultAsync(x => x.IntegrationId == currentUser.Id, cancellationToken);

            if (employee == null)
            {
                return new CurrentUser();
            }

            return new CurrentUser
            {
                Employee = employee,
                Roles = currentUser.Roles
            };
        }

        public Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken)
        {
            return _userRepository.RegisterUser(dto, cancellationToken);
        }

        public Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken)
        {
            return _userRepository.UpdateUser(dto, cancellationToken);
        }
    }
}
