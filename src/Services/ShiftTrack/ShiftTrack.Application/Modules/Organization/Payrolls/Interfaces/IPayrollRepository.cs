using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Entities;

namespace ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;

public interface IPayrollRepository
{
    Task<IEnumerable<Payroll>> GetListByPeriod(PayrollsByPeriodDto dto, CancellationToken cancellationToken);
    Task<Payroll> Get(GetPayrollDto dto, CancellationToken cancellationToken); 
    Task<Payroll> Create(Payroll payroll, CancellationToken cancellationToken); 
    Task Update(Payroll payroll, CancellationToken cancellationToken);
}