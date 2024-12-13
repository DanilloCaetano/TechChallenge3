using Domain.Base.Repository;
using Domain.Contact.Entity;

namespace Domain.Contact.Repository
{
    public interface IContactRepository : IBaseRepository<ContactEntity>
    {
    }
}
