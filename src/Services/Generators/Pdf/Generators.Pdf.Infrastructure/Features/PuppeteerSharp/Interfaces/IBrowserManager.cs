using PuppeteerSharp;

namespace Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Interfaces;

public interface IBrowserManager
{
    Task<IBrowser> GetBrowserAsync();
}