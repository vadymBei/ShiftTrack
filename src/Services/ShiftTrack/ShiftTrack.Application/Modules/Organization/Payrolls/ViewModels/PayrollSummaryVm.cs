namespace ShiftTrack.Application.Modules.Organization.Payrolls.ViewModels;

public class PayrollSummaryVm
{
    public decimal TotalAmount { get; set; }
    public int TotalEmployees { get; set; }
    public int TotalWorkedHours { get; set; }
    public IEnumerable<PayrollVm> Payrolls { get; set; }
}