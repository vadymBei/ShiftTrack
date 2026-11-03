using AutoMapper;
using Moq;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Dtos;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.UseCases.Commands.UpdateEmployee;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;
using ShiftTrack.Domain.Modules.System.User.Employees.Enums;

namespace ShiftTrack.Core.Unit.Tests.Application.Organization.Employees;

public class UpdateEmployeeCommandHandlerTests
{
    private const long EmployeeId = 5;

    private readonly Employee _stored = new()
    {
        Id = EmployeeId,
        Name = "Олена",
        Surname = "Коваль",
        CreatedAt = new DateTime(2026, 10, 21),
        HireDate = new DateTime(2025, 1, 10)
    };

    private readonly Mock<IEmployeeService> _employeeService = new();
    private readonly Mock<IVacationAccrualService> _accruals = new();
    private readonly UpdateEmployeeCommandHandler _handler;

    public UpdateEmployeeCommandHandlerTests()
    {
        var repository = new Mock<IEmployeeRepository>();
        repository.Setup(x => x.GetById(EmployeeId, It.IsAny<CancellationToken>())).ReturnsAsync(_stored);

        _employeeService
            .Setup(x => x.Update(It.IsAny<EmployeeToUpdateDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((EmployeeToUpdateDto dto, CancellationToken _) =>
                new Employee { Id = dto.Id, CreatedAt = _stored.CreatedAt, HireDate = dto.HireDate });

        _handler = new UpdateEmployeeCommandHandler(
            new Mock<IMapper>().Object, _employeeService.Object, repository.Object, _accruals.Object);
    }

    [Fact]
    public async Task ChangedHireDate_RecalculatesTheVacationDays()
    {
        await _handler.Handle(Command(new DateTime(2024, 6, 3)), CancellationToken.None);

        _employeeService.Verify(x => x.Update(It.Is<EmployeeToUpdateDto>(d => d.HireDate == new DateTime(2024, 6, 3)),
            It.IsAny<CancellationToken>()), Times.Once);
        _accruals.Verify(x => x.Recalculate(EmployeeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SameHireDate_DoesNotRecalculate()
    {
        await _handler.Handle(Command(new DateTime(2025, 1, 10)), CancellationToken.None);

        _accruals.Verify(x => x.Recalculate(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ClearedHireDate_CountsFromTheRegistration()
    {
        await _handler.Handle(Command(null), CancellationToken.None);

        _accruals.Verify(x => x.Recalculate(EmployeeId, It.IsAny<CancellationToken>()), Times.Once);
    }

    private static UpdateEmployeeCommand Command(DateTime? hireDate) =>
        new(EmployeeId, "Олена", "Коваль", "", null, null, null, EmployeeGender.Female, hireDate);
}
