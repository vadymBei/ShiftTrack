using AutoMapper;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels
{
    [AutoMap(typeof(Unit))]
    public class UnitVM
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }
    }
}
