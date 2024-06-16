using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Core.Domain.System.User.Employees.Enums;
using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.System.User.Employees.Entities
{
    public class Employee : ISoftDeletable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string FullName
        {
            get
            {
                return Surname + " " + Name + " " + Patronymic;
            }
        }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string IntegrationId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DepartmentId { get; set; }
        public Department Department { get; set; }

        public long? PositionId { get; set; }
        public Position Position { get; set; }

        public EmployeeGender Gender { get; set; }
    }
}
