using Data.WebClient.Models;

namespace Data.WebClient.Interfaces
{
    public interface IWebClient
    {
        WebClientConfiguration Configuration { get; }

        HttpClient HttpClient { get; }

        Task<T> Get<T>(string endpoint, CancellationToken cancellationToken) where T : class;

        Task Get(string endpoint, CancellationToken cancellationToken);

        Task<T> Post<T>(string endpoint, CancellationToken cancellationToken) where T : class;

        Task Post(string endpoint, CancellationToken cancellationToken);

        Task<T> Put<T>(string endpoint, CancellationToken cancellationToken) where T : class;

        Task Put(string endpoint, CancellationToken cancellationToken);

        Task<T> Delete<T>(string endpoint, CancellationToken cancellationToken) where T : class;

        Task Delete(string endpoint, CancellationToken cancellationToken);
    }
}
