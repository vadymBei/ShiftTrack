using AutoMapper;
using Kernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.Dtos;
using ShiftTrack.Core.Application.System.User.Common.Interfaces;
using ShiftTrack.Core.Application.System.User.Common.ViewModels;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;

namespace ShiftTrack.Core.Application.System.User.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeVM>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IApplicationDbContext _applicationDbContext;

        public CreateEmployeeCommandHandler(
            IMapper mapper,
            IEmployeeService userService,
            IDepartmentService departmentService,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _userService = userService;
            _departmentService = departmentService;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<EmployeeVM> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeAlreadyExist = await _applicationDbContext.Employees
                .AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (employeeAlreadyExist)
            {
                throw new UserAlreadyExistException(request.PhoneNumber);
            }

            var employee = new Employee()
            {
                Name = request.Name,
                Surname = request.Surname,
                Patronymic = request.Patronymic,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender
            };

            if (request.DepartmentId is not null)
            {
                await _departmentService
                    .GetById(request.DepartmentId, cancellationToken);

                employee.DepartmentId = request.DepartmentId;
            }

            var user = await _userService.RegisterAuthUser(
                new UserToRegisterDto(
                    employee.PhoneNumber,
                    employee.Email,
                    request.Password),
                cancellationToken);

            _applicationDbContext.Employees.Add(employee);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EmployeeVM>(employee);
        }
    }
}
