using Data.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;

namespace ShiftTrack.Core.Domain.System.User.Employees.Entities
{
    public class Employee : ISoftDeletable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string FullName {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }
        }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DepartmentId { get; set; }
        public Department Department { get; set; }

        public EmployeeGender Gender { get; set; }
    }
}
