using Newtonsoft.Json;

namespace Location.Infrastructure.Models;

public class GeoLocationAddress
{
    [JsonProperty("city")]
    public string City { get; set; }
    
    [JsonProperty("town")]
    public string Town { get; set; }
    
    [JsonProperty("municipality")]
    public string Municipality { get; set; }
    
    [JsonProperty("district")]
    public string District { get; set; }
    
    [JsonProperty("state")]
    public string State { get; set; }
    
    [JsonProperty("country")]
    public string Country { get; set; }
    
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }
}