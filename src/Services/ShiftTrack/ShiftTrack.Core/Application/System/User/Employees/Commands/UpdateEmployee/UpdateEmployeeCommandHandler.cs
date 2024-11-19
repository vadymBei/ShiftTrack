using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Authentication.Interfaces;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeVM>
    {
        private readonly IMapper _mapper;
        private readonly IUnitService _unitService;
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _applicationDbContext;

        public UpdateEmployeeCommandHandler(
            IMapper mapper,
            IUnitService unitService,
            IPositionService positionService,
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            ICurrentUserService currentUserService,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _unitService = unitService;
            _positionService = positionService;
            _employeeService = employeeService;
            _departmentService = departmentService;
            _currentUserService = currentUserService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<EmployeeVM> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _applicationDbContext.Employees
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (employee == null)
            {
                throw new EntityNotFoundException(typeof(Employee), request.Id);
            }

            if (employee.PhoneNumber != request.PhoneNumber
                || employee.Email != request.Email)
            {
                var updatedUser = await _employeeService.UpdateAuthUser(
                     new UserToUpdateDto(
                         _currentUserService.User.Id,
                         request.Email,
                         request.PhoneNumber)
                     , cancellationToken);
            }

            employee.Name = request.Name;
            employee.Surname = request.Surname;
            employee.Patronymic = request.Patronymic;
            employee.Email = request.Email;
            employee.PhoneNumber = request.PhoneNumber;
            employee.DateOfBirth = request.DateOfBirth;
            employee.Gender = request.Gender;

            if (request.DepartmentId is not null)
            {
                await _departmentService
                    .GetById(request.DepartmentId, cancellationToken);

                employee.DepartmentId = request.DepartmentId;
            }
            else
            {
                employee.DepartmentId = null;
            }

            if (request.PositionId is not null)
            {
                await _positionService
                    .GetById(request.PositionId, cancellationToken);

                employee.PositionId = request.PositionId;
            }
            else
            {
                employee.PositionId = null;
            }

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            employee = await _employeeService
                .GetById(request.Id, cancellationToken);

            return _mapper.Map<EmployeeVM>(employee);
        }
    }
}
