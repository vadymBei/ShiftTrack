using Moq;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Booking.Vacations.Services;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.System.User.Employees.Entities;

namespace ShiftTrack.Core.Unit.Tests.Application.Booking.Vacations;

public class VacationAccrualServiceTests
{
    private const long EmployeeId = 5;

    private readonly VacationAccrualService _service;
    private readonly Employee _employee = new() { Id = EmployeeId, CreatedAt = new DateTime(2026, 10, 21) };

    public VacationAccrualServiceTests()
    {
        var employeeRepository = new Mock<IEmployeeRepository>();
        employeeRepository
            .Setup(x => x.GetById(EmployeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_employee);

        var vacationRepository = new Mock<IVacationRepository>();
        vacationRepository
            .Setup(x => x.GetApproved(It.IsAny<IReadOnlyCollection<VacationType>>(), EmployeeId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Vacation>());

        _service = new VacationAccrualService(
            employeeRepository.Object,
            vacationRepository.Object,
            new Mock<IVacationAccrualRepository>().Object,
            new VacationAccrualCalculator());
    }

    [Fact]
    public async Task AnnualVacation_BeforeSixMonths_IsNotAllowed()
    {
        var exception = await Assert.ThrowsAsync<VacationException>(() => _service.EnsureCanTakeVacation(
            EmployeeId, VacationType.YearMainVacation, new DateTime(2027, 4, 20), CancellationToken.None));

        Assert.Equal("BCK_VCN_ANNUAL_VACATION_BEFORE_SIX_MONTHS", exception.ErrorType);
        Assert.Contains("21.04.2027", exception.ErrorMessage);
    }

    [Fact]
    public async Task AnnualVacation_AfterSixMonths_IsAllowed()
    {
        await _service.EnsureCanTakeVacation(
            EmployeeId, VacationType.YearMainVacation, new DateTime(2027, 4, 21), CancellationToken.None);
    }

    [Theory]
    [InlineData(VacationType.BonusVacation)]
    [InlineData(VacationType.VacationWithoutSalaryByFamily)]
    [InlineData(VacationType.VacationWithoutSalaryByPregnancy)]
    public async Task OtherVacationTypes_AreNotLimitedBySixMonths(VacationType type)
    {
        await _service.EnsureCanTakeVacation(EmployeeId, type, new DateTime(2026, 10, 22), CancellationToken.None);
    }

    [Fact]
    public async Task HireDate_CountsInsteadOfTheRegistration()
    {
        // Hired long before the account was registered.
        _employee.HireDate = new DateTime(2025, 1, 10);

        await _service.EnsureCanTakeVacation(
            EmployeeId, VacationType.YearMainVacation, new DateTime(2026, 11, 2), CancellationToken.None);

        Assert.Equal(new DateTime(2025, 7, 10), await _service.GetAnnualVacationAvailableFrom(EmployeeId, CancellationToken.None));
    }

    [Fact]
    public async Task HireDate_AfterTheRegistration_MovesTheSixMonths()
    {
        _employee.HireDate = new DateTime(2026, 12, 1);

        var exception = await Assert.ThrowsAsync<VacationException>(() => _service.EnsureCanTakeVacation(
            EmployeeId, VacationType.YearMainVacation, new DateTime(2027, 5, 1), CancellationToken.None));

        Assert.Contains("01.06.2027", exception.ErrorMessage);
    }
}
