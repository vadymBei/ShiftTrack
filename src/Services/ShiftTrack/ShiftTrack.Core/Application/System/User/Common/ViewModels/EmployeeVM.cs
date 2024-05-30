using AutoMapper;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;

namespace ShiftTrack.Core.Application.System.User.Common.ViewModels
{
    [AutoMap(typeof(Employee))]
    public class EmployeeVM
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public long? DepartmentId { get; set; }
        public Department Department { get; set; }

        public EmployeeGender Gender { get; set; }
    }
}
