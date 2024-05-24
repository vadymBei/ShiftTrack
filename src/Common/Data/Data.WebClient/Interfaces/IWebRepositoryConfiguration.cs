namespace Data.WebClient.Interfaces
{
    public interface IWebRepositoryConfiguration
    {
        public IWebClient WebClient { get; set; }

        public string EndpointGetAll { get; set; }

        public string EndpointGetAllPaged { get; set; }
        
        public string EndpointGetById { get; set; }
    }
}
