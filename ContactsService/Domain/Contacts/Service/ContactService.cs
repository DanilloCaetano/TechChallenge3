using Domain.Contact.Entity;
using Domain.Contact.Repository;

namespace Domain.Contact.Service
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContactEntity> GetById(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<ContactEntity>> GetByRegionId(Guid id)
        {
            var contacts = await _repository.GetAsync(c => c.RegionId == id);

            return contacts.ToList();
        }
    }
}
