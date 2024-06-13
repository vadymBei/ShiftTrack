using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Common.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IAuthenticationRepository _authenticationRepository;

        public EmployeeService(
            IApplicationDbContext applicationDbContext,
            IAuthenticationRepository authenticationRepository)
        {
            _applicationDbContext = applicationDbContext;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<Employee> GetById(object id, CancellationToken cancellationToken)
        {
            long employeeId = (long)id;

            var employee = await _applicationDbContext.Employees
                .AsNoTracking()
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == employeeId, cancellationToken);

            if (employee == null)
            {
                throw new EntityNotFoundException(typeof(Employee), employeeId);
            }

            return employee;
        }

        public Task<Authentication.Models.User> RegisterAuthUser(UserToRegisterDto dto, CancellationToken cancellationToken)
        {
            return _authenticationRepository.RegisterUser(dto, cancellationToken);
        }

        public Task<Authentication.Models.User> UpdateAuthUser(UserToUpdateDto dto, CancellationToken cancellationToken)
        {
            return _authenticationRepository.UpdateUser(dto, cancellationToken);
        }
    }
}
