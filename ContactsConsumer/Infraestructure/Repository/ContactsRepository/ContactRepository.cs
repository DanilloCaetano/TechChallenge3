using Domain.Contact.Entity;
using Domain.Contact.Repository;
using Infraestructure.Context;
using Infraestructure.Repository.Base;

namespace Infraestructure.Repository.ContactsRepository
{
    public class ContactRepository : BaseRepository<ContactEntity>, IContactRepository
    {
        private readonly TechChallengeContext _context;

        public ContactRepository(TechChallengeContext context) : base(context)
        {
            _context = context;
        }
    }
}
