using Domain.Base.Entity;
using Domain.Contact.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Region.Entity
{
    [Table("Region")]
    public class RegionEntity : BaseEntity
    {
        public string Description {  get; set; }
        public string DDD {  get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<ContactEntity> Contacts { get; set; }
    }
}
