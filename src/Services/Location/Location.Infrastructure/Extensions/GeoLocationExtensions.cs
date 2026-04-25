using Location.Domain.Entities;
using Location.Infrastructure.Models;

namespace Location.Infrastructure.Extensions;

public static class GeoLocationExtensions
{
    public static LocationEntity ToLocationEntity(this GeoLocation sourse)
    {
        return new LocationEntity
        {
            IntegrationId = sourse.PlaceId,
            Name = sourse.Name,
            DisplayName = sourse.DisplayName,
            Country = sourse.Address.Country,
            CountryCode = sourse.Address.CountryCode
        };
    }
}