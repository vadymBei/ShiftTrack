using AutoMapper;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels
{
    [AutoMap(typeof(Department))]
    public class DepartmentVM
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
