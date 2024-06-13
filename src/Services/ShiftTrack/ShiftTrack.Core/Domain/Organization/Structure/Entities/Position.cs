using ShiftTrack.Data.Interfaces;

namespace ShiftTrack.Core.Domain.Organization.Structure.Entities
{
    public class Position : ISoftDeletable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
