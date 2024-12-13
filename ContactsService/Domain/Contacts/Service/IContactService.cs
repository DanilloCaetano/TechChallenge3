using Domain.Contact.Entity;

namespace Domain.Contact.Service
{
    public interface IContactService
    {
        Task<ContactEntity> GetById(Guid id);
        Task<List<ContactEntity>> GetByRegionId(Guid id);
    }
}
