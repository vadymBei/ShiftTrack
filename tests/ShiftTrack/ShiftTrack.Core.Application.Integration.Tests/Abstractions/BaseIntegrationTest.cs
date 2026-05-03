using Bogus;
using Microsoft.Extensions.DependencyInjection;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Commands.CreateDepartment;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.CreatePosition;
using ShiftTrack.Application.Modules.Organization.Structure.Positions.ViewModels;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Dtos;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.UseCases.Commands.CreateShift;
using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Integration.Tests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _serviceScope;
    protected readonly IMediator Mediator;
    protected readonly Faker Faker = new("uk");

    protected readonly IDepartmentRepository DepartmentRepository;
    protected readonly IUnitRepository UnitRepository;
    protected readonly IPositionRepository PositionRepository;
    protected readonly IEmployeeRepository EmployeeRepository;
    protected readonly IShiftRepository ShiftRepository;

    protected BaseIntegrationTest(
        IntegrationTestWebAppFactory factory)
    {
        _serviceScope = factory.Services.CreateScope();

        Mediator = _serviceScope.ServiceProvider.GetRequiredService<IMediator>();

        DepartmentRepository = _serviceScope.ServiceProvider.GetRequiredService<IDepartmentRepository>();
        UnitRepository = _serviceScope.ServiceProvider.GetRequiredService<IUnitRepository>();
        PositionRepository = _serviceScope.ServiceProvider.GetRequiredService<IPositionRepository>();
        EmployeeRepository = _serviceScope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
        ShiftRepository = _serviceScope.ServiceProvider.GetRequiredService<IShiftRepository>();
    }

    protected async Task<UnitVm> CreateUnitAsync(UnitToCreateDto dto = null)
    {
        dto ??= new UnitToCreateDto(
            Faker.Address.City(),
            Faker.Address.FullAddress(),
            Faker.Random.Replace("???").ToUpper());

        return await Mediator.Invoke(new CreateUnitCommand(dto));
    }

    protected async Task<DepartmentVm> CreateDepartmentAsync(long unitId, DepartmentToCreateDto dto = null)
    {
        dto ??= new DepartmentToCreateDto(
            Faker.Company.CompanyName(),
            unitId);

        return await Mediator.Invoke(new CreateDepartmentCommand(dto));
    }

    protected async Task<PositionVm> CreatePositionAsync(PositionToCreateDto dto = null)
    {
        dto ??= new PositionToCreateDto(
            Faker.Name.JobTitle(),
            Faker.Name.JobDescriptor(),
            Faker.Random.Decimal(100, 500));

        return await Mediator.Invoke(new CreatePositionCommand(dto));
    }

    protected async Task<ShiftVm> CreateShiftAsync(ShiftToCreateDto dto = null)
    {
        dto ??= new ShiftToCreateDto(
            Faker.Random.Replace("??").ToUpper(),
            Faker.Commerce.ProductName(),
            Faker.Internet.Color(),
            ShiftType.Workday,
            new TimeSpan(08, 00, 00),
            new TimeSpan(20, 00, 00));

        return await Mediator.Invoke(new CreateShiftCommand(dto));
    }
}