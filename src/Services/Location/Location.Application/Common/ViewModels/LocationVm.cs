using AutoMapper;
using Location.Domain.Entities;

namespace Location.Application.Common.ViewModels;

[AutoMap(typeof(LocationEntity))]
public class LocationVm
{
    public string IntegrationId { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Country { get; set; }
    public string CountryCode { get; set; }
}