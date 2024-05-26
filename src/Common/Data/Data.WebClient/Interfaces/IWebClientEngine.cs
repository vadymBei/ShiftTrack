namespace Data.WebClient.Interfaces
{
    public interface IWebClientEngine
    {
        void SetWebResource(string webResourcePath);

        void SetNoSslValidation();
    }
}
