using Newtonsoft.Json;

namespace Location.Infrastructure.Models;

public class GeoLocation
{
    [JsonProperty("place_id")]
    public string PlaceId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("display_name")]
    public string DisplayName { get; set; } 
    
    [JsonProperty("addresstype")]
    public string AddressType { get; set; }
    
    [JsonProperty("address")]
    public GeoLocationAddress Address { get; set; }
}