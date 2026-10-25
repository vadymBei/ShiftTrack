using ShiftTrack.Application.Modules.Booking.Vacations.Constants;
using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Application.Modules.Booking.Vacations.Exceptions;
using ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;
using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.Booking.Vacations.Models;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Services;

public class VacationAccrualService(
    IEmployeeRepository employeeRepository,
    IVacationRepository vacationRepository,
    IVacationAccrualRepository vacationAccrualRepository,
    IVacationAccrualCalculator vacationAccrualCalculator) : IVacationAccrualService
{
    public async Task<int> RecalculateAll(CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var employees = await employeeRepository.GetServiceStarts(cancellationToken);

        var excludedPeriods = (await vacationRepository.GetApproved(
                VacationLaw.NotCountedInServiceTypes, null, cancellationToken))
            .ToLookup(x => x.EmployeeId, x => new DateRangeDto(x.StartDate, x.EndDate));

        var usedDays = (await vacationRepository.GetApproved(
                VacationLaw.AnnualVacationTypes, null, cancellationToken))
            .Where(x => x.StartDate.Date <= today)
            .GroupBy(x => x.EmployeeId)
            .ToDictionary(x => x.Key, x => x.Sum(v => v.DaysCount));

        var accruals = new Dictionary<long, IReadOnlyList<VacationAccrual>>();
        var balances = new Dictionary<long, int>();

        foreach (var employee in employees)
        {
            var workingYears = vacationAccrualCalculator.Calculate(
                employee.ServiceStart, today, excludedPeriods[employee.EmployeeId].ToList());

            accruals[employee.EmployeeId] = workingYears
                .Select(x => new VacationAccrual
                {
                    EmployeeId = employee.EmployeeId,
                    WorkingYear = x.WorkingYear,
                    PeriodStart = x.PeriodStart,
                    PeriodEnd = x.PeriodEnd,
                    Days = x.Days,
                    IsFullEntitlement = x.IsFullEntitlement
                })
                .ToList();

            var accrued = workingYears.Sum(x => x.Days);
            var used = usedDays.GetValueOrDefault(employee.EmployeeId);

            balances[employee.EmployeeId] = (int)Math.Floor(Math.Max(accrued - used, 0));
        }

        await vacationAccrualRepository.ReplaceForEmployees(accruals, cancellationToken);
        await employeeRepository.UpdateVacationDaysBalances(balances, cancellationToken);

        return employees.Count;
    }

    public async Task<VacationBalance> GetBalance(long employeeId, CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var accruals = await vacationAccrualRepository.GetByEmployeeId(employeeId, cancellationToken);

        IReadOnlyList<WorkingYearAccrualDto> workingYears = accruals.Count > 0
            ? accruals
                .Select(x => new WorkingYearAccrualDto(
                    x.WorkingYear, x.PeriodStart, x.PeriodEnd, x.Days, x.IsFullEntitlement,
                    x.PeriodEnd.Date >= today))
                .ToList()
            : await CalculateNow(employeeId, today, cancellationToken);

        var annualVacations = await vacationRepository.GetApproved(
            VacationLaw.AnnualVacationTypes, employeeId, cancellationToken);

        var used = annualVacations.Where(x => x.StartDate.Date <= today).Sum(x => x.DaysCount);
        var planned = annualVacations.Where(x => x.StartDate.Date > today).Sum(x => x.DaysCount);
        var total = workingYears.Sum(x => x.Days);
        var currentYear = workingYears.OrderByDescending(x => x.WorkingYear).FirstOrDefault();

        return new VacationBalance
        {
            AnnualDays = VacationLaw.AnnualBasicVacationDays,
            TotalDays = total,
            UsedDays = used,
            PlannedDays = planned,
            AvailableDays = Math.Max(total - used - planned, 0),
            WorkingYearStart = currentYear?.PeriodStart,
            WorkingYearEnd = currentYear?.PeriodEnd,
            AnnualVacationAvailableFrom = await GetAnnualVacationAvailableFrom(employeeId, cancellationToken)
        };
    }

    public async Task<DateTime> GetAnnualVacationAvailableFrom(long employeeId, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(employeeId, cancellationToken);

        var excludedPeriods = await GetExcludedPeriods(employeeId, cancellationToken);

        return vacationAccrualCalculator.GetAnnualVacationAvailableFrom(employee.CreatedAt, excludedPeriods);
    }

    public async Task EnsureCanTakeVacation(long employeeId, VacationType type, DateTime startDate,
        CancellationToken cancellationToken)
    {
        if (!VacationLaw.AnnualVacationTypes.Contains(type))
            return;

        var availableFrom = await GetAnnualVacationAvailableFrom(employeeId, cancellationToken);

        if (startDate.Date >= availableFrom)
            return;

        throw new VacationException(
            string.Format(VacationExceptionsLocalization.ANNUAL_VACATION_BEFORE_SIX_MONTHS, availableFrom),
            nameof(VacationExceptionsLocalization.ANNUAL_VACATION_BEFORE_SIX_MONTHS));
    }

    private async Task<List<DateRangeDto>> GetExcludedPeriods(long employeeId, CancellationToken cancellationToken)
    {
        return (await vacationRepository.GetApproved(
                VacationLaw.NotCountedInServiceTypes, employeeId, cancellationToken))
            .Select(x => new DateRangeDto(x.StartDate, x.EndDate))
            .ToList();
    }

    /// <summary>For employees the job hasn't processed yet (e.g. registered today).</summary>
    private async Task<IReadOnlyList<WorkingYearAccrualDto>> CalculateNow(long employeeId, DateTime today,
        CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(employeeId, cancellationToken);

        var excludedPeriods = await GetExcludedPeriods(employeeId, cancellationToken);

        return vacationAccrualCalculator.Calculate(employee.CreatedAt, today, excludedPeriods);
    }
}
