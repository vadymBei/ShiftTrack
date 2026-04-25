namespace Location.Domain.Entities;

public class LocationEntity
{
    public long Id { get; set; }
    public string IntegrationId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
}