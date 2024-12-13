using Domain.Contact.Entity;

namespace Domain.Contact.Service
{
    public interface IContactService
    {
        Task AddAsync(ContactEntity users);
        Task<ContactEntity> GetById(Guid id);
        Task<List<ContactEntity>> GetByRegionId(Guid id);
    }
}
