using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Interfaces;
using Generators.Pdf.Infrastructure.Features.PuppeteerSharp.Options;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace Generators.Pdf.Infrastructure.Features.PuppeteerSharp;

public class BrowserManager(IOptions<PuppeteerOptions> options) : IBrowserManager
{
    private readonly PuppeteerOptions _options = options.Value;
    private IBrowser? _browser;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public async Task<IBrowser> GetBrowserAsync()
    {
        await _initLock.WaitAsync();
        try
        {
            if (IsBrowserAlive(_browser))
                return _browser!;

            await DisposeBrowserSafeAsync();

            var executablePath = _options.ExecutablePath 
                                 ?? Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");

            var launchOptions = new LaunchOptions
            {
                Headless = true,
                Args =
                [
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage",
                    "--disable-gpu"
                ]
            };

            if (!string.IsNullOrEmpty(executablePath))
            {
                launchOptions.ExecutablePath = executablePath;
            }
            else
            {
                var fetcher = new BrowserFetcher();
                await fetcher.DownloadAsync();
            }

            try
            {
                _browser = await Puppeteer.LaunchAsync(launchOptions);
            }
            catch (Exception ex)
            {
                _browser = null;
                throw new Exception($"Failed to launch browser with path: {launchOptions.ExecutablePath ?? "bundled"}. Error: {ex.Message}", ex);
            }

            return _browser;
        }
        finally
        {
            _initLock.Release();
        }
    }

    private static bool IsBrowserAlive(IBrowser? browser)
        => browser is not null && browser.IsConnected && !browser.IsClosed;

    private async Task DisposeBrowserSafeAsync()
    {
        if (_browser is null)
            return;

        try
        {
            if (!_browser.IsClosed)
                await _browser.CloseAsync();
        }
        catch
        {
            // ignore dispose errors
        }
        finally
        {
            _browser = null;
        }
    }
}