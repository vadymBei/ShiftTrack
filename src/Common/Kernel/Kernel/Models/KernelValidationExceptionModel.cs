namespace Kernel.Models
{
    public class KernelValidationExceptionModel : KernelExceptionModel
    {
        public IDictionary<string, string[]> ValidationErrors { get; set; }
    }
}
