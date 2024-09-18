using AutoMapper;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
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

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public long? DepartmentId { get; set; }
        public DepartmentVM Department { get; set; }

        public long? UnitId { get; set; }
        public UnitVM Unit { get; set; }

        public long? PositionId { get; set; }
        public PositionVM Position { get; set; }

        public EmployeeGender Gender { get; set; }
    }
}
