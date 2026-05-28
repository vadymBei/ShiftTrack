using ShiftTrack.Application.Modules.Organization.Employees.Interfaces;
using ShiftTrack.Application.Modules.Organization.Payrolls.Constants;
using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;
using ShiftTrack.Application.Modules.Organization.Payrolls.Exceptions;
using ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;
using ShiftTrack.Domain.Common.Extensions;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Entities;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Enums;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.Services;

public class PayrollService(
    IPayrollRepository payrollRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeShiftRepository employeeShiftRepository) : IPayrollService
{
    public async Task MarkPayrollAsPaid(MarkPayrollAsPaidDto dto, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(dto.EmployeeId, cancellationToken);
        
        var payroll = await payrollRepository.Get(
            new GetPayrollDto
            (
                dto.EmployeeId,
                dto.Period
            ),
            cancellationToken);

        var employeeShifts = await employeeShiftRepository.GetEmployeeShifts(
            new EmployeeShiftsFilterDto(
                [dto.EmployeeId],
                new DateTime(dto.Period.Year, dto.Period.Month, 1),
                new DateTime(dto.Period.Year, dto.Period.Month,
                    DateTime.DaysInMonth(dto.Period.Year, dto.Period.Month)),
                true
            ),
            cancellationToken);

        var employeeTotalWorkHours = (int)employeeShifts
            .Where(x => x.Shift.WorkHours is not null)
            .Select(x => x.Shift.WorkHours.Value)
            .Sum().TotalHours;

        if (payroll is null)
        {
            if (employeeShifts.Count() < DateTime.DaysInMonth(dto.Period.Year, dto.Period.Month))
            {
                throw new PayrollException(
                    PayrollExceptionsLocalization.PAYROLL_TIMESHEET_NOT_FILLED,
                    nameof(PayrollExceptionsLocalization.PAYROLL_TIMESHEET_NOT_FILLED));
            }

            payroll = await payrollRepository.Create(
                new Payroll
                {
                    EmployeeId = dto.EmployeeId,
                    Status = PayrollStatus.Paid,
                    PaidAt = DateTime.UtcNow,
                    Year = dto.Period.Year,
                    Month = dto.Period.Month,
                    WorkedHours = employeeTotalWorkHours,
                    HourlyRate = employee.Position.HourlyRate,
                    TotalAmount = employeeTotalWorkHours * employee.Position.HourlyRate
                },
                cancellationToken);
        }
        else
        {
            if (payroll.Status == PayrollStatus.Paid)
            {
                throw new PayrollException(
                    PayrollExceptionsLocalization.PAYROLL_ALREADY_PAID,
                    nameof(PayrollExceptionsLocalization.PAYROLL_ALREADY_PAID));
            }
        }
    }
}