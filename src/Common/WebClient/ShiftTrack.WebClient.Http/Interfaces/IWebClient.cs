using ShiftTrack.WebClient.Http.Configuration;

namespace ShiftTrack.WebClient.Http.Interfaces
{
    public interface IWebClient
    {
        ClientConfiguration Configuration { get; }
        HttpClient GetHttpClient();

        #region Get
        Task<T> Get<T>(string path, CancellationToken cancellationToken) where T : class;

        Task<T> Get<T>(CancellationToken cancellationToken) where T : class;

        Task Get(string path, CancellationToken cancellationToken);

        Task Get(CancellationToken cancellationToken);
        #endregion

        #region Post
        Task<T> Post<T>(string path, CancellationToken cancellationToken) where T : class;

        Task<T> Post<T>(CancellationToken cancellationToken) where T : class;

        Task Post(string path, CancellationToken cancellationToken);

        Task Post(CancellationToken cancellationToken);
        #endregion

        #region Put
        Task<T> Put<T>(string path, CancellationToken cancellationToken) where T : class;

        Task<T> Put<T>(CancellationToken cancellationToken) where T : class;

        Task Put(string path, CancellationToken cancellationToken);

        Task Put(CancellationToken cancellationToken);
        #endregion

        #region Delete
        Task<T> Delete<T>(string path, CancellationToken cancellationToken) where T : class;

        Task<T> Delete<T>(CancellationToken cancellationToken) where T : class;

        Task Delete(string path, CancellationToken cancellationToken);

        Task Delete(CancellationToken cancellationToken);
        #endregion

        #region Patch
        Task<T> Patch<T>(string path, CancellationToken cancellationToken) where T : class;

        Task<T> Patch<T>(CancellationToken cancellationToken) where T : class;

        Task Patch(string path, CancellationToken cancellationToken);

        Task Patch(CancellationToken cancellationToken);
        #endregion
    }
}
