using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Modules.Organization.Payrolls.Dtos;
using ShiftTrack.Application.Modules.Organization.Payrolls.Interfaces;
using ShiftTrack.Domain.Modules.Organization.Payrolls.Entities;
using ShiftTrack.Infrastructure.Common.Interfaces;

namespace ShiftTrack.Infrastructure.Modules.Organization.Payrolls.Repositories;

public class PayrollRepository(
    IApplicationDbContext applicationDbContext) : IPayrollRepository
{
    public async Task<IEnumerable<Payroll>> GetListByPeriod(PayrollsByPeriodDto dto, CancellationToken cancellationToken)
    {
        var payrolls = await applicationDbContext.Payrolls
            .AsNoTracking()
            .Where(p => p.Year == dto.Period.Year 
                        && p.Month == dto.Period.Month 
                        && dto.EmployeeIds.Contains(p.EmployeeId))
            .ToListAsync(cancellationToken);

        return payrolls;
    }

    public async Task<Payroll> Get(GetPayrollDto dto, CancellationToken cancellationToken)
    {
        var payroll = await applicationDbContext.Payrolls
            .FirstOrDefaultAsync(p => p.EmployeeId == dto.EmployeeId 
                                      && p.Year == dto.Period.Year 
                                      && p.Month == dto.Period.Month, cancellationToken);

        return payroll;
    }

    public async Task<Payroll> Create(Payroll payroll, CancellationToken cancellationToken)
    {
        applicationDbContext.Payrolls.Add(payroll);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return payroll;
    }
    
    public async Task Update(Payroll payroll, CancellationToken cancellationToken)
    {
        applicationDbContext.Payrolls.Update(payroll);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}