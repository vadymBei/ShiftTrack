using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.Organization.Structure.Entities
{
    public class Unit : ISoftDeletable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string FullName
        {
            get
            {
                return Code + " " + Name;
            }
        }
    }
}
