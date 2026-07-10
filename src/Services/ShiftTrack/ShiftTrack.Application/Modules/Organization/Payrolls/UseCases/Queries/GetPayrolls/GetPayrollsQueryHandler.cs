using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Employees.ViewModels;
using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;
using ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;
using ShiftTrack.Application.Modules.Organization.Payrolls.ViewModels;
using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.ViewModels;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;
using ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.UseCases.Queries.GetPayrolls;

public class GetPayrollsQueryHandler(
    IMapper mapper,
    IPayrollRepository payrollRepository,
    IUnitTimesheetService unitTimesheetService) : IRequestHandler<GetPayrollsQuery, PayrollSummaryVm>
{
    public async Task<PayrollSummaryVm> Handle(GetPayrollsQuery request, CancellationToken cancellationToken = default)
    {
        var timesheet = await unitTimesheetService.GetTimesheet(
            new UnitTimesheetDto(
                request.Period,
                request.DepartmentId),
            cancellationToken);

        var payrolls = await payrollRepository.GetListByPeriod(
            new PayrollsByPeriodDto(
                request.Period,
                timesheet.EmployeeTimesheets
                    .Select(e => e.Employee.Id)
                    .ToHashSet()),
            cancellationToken);

        var payrollDict = payrolls
            .ToDictionary(x => x.EmployeeId);

        var employeePayrolls = timesheet.EmployeeTimesheets
            .Select(et =>
            {
                payrollDict.TryGetValue(et.Employee.Id, out var payroll);
                var hourlyRate = et.Employee.Position?.HourlyRate ?? 0;

                return new PayrollVm
                {
                    EmployeeId = et.Employee.Id,
                    Employee = mapper.Map<EmployeeVm>(et.Employee),
                    WorkedHours = et.TotalWorkHours,
                    HourlyRate = hourlyRate,
                    TotalAmount = et.TotalWorkHours * hourlyRate,
                    Status = payroll?.Status ?? PayrollStatus.Pending
                };
            })
            .ToList();

        return new PayrollSummaryVm
        {
            Payrolls = employeePayrolls,
            TotalEmployees = employeePayrolls.Count,
            TotalAmount = employeePayrolls.Sum(p => p.TotalAmount),
            TotalWorkedHours = employeePayrolls.Sum(p => p.WorkedHours)
        };
    }
}