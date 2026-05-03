using Location.Application.Common.Models;
using Location.Domain.Entities;

namespace Location.Application.Common.Extensions;

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