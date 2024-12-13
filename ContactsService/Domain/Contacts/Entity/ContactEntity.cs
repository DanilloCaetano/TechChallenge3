using Domain.Base.Entity;
using Domain.Region.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Contact.Entity
{
    [Table("Contact")]
    public class ContactEntity : BaseEntity
    {
        public string Name { get; set; }
        public string CellphoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active {  get; set; }
        public Guid RegionId { get; set; }
        public RegionEntity Region { get; set; }

    }
}
