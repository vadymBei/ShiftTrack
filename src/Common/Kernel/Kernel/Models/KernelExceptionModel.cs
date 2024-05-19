using System.Net;

namespace Kernel.Models
{
    public class KernelExceptionModel
    {
        public HttpStatusCode Code { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
    }
}
