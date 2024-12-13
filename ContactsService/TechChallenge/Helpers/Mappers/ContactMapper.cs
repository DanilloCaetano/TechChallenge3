using Domain.Contact.Entity;
using Domain.Region.Entity;
using Integration.Region.Dtos;
using TechChallenge.Controllers.Contacts.Dto;
using TechChallenge1.Controllers.Contacts.Dto;

namespace TechChallenge1.Helpers.Mappers
{
    public static class ContactMapper
    {

        public static ContactEntity ContactMapPostDto(ContactPostDto postDto)
        {
            var contact = new ContactEntity
            {
                Id = Guid.NewGuid(),
                Active = postDto.Active,
                CellphoneNumber = postDto.CellphoneNumber,
                CreationDate = DateTime.UtcNow.AddHours(-3),
                Email = postDto.Email,
                Name = postDto.Name,
                RegionId = postDto.RegionId,
            };

            return contact;
        }

        public static ContactsGetDto MapContactGetDto(List<ContactEntity> contacts, RegionRequestGetDto region)
        {
            var contactsMap = contacts.Select(c => new ContactGetDto
            {
                Active = c.Active,
                CellphoneNumber = c.CellphoneNumber,
                CreationDate = c.CreationDate,
                Email = c.Email,
                Name = c.Name,
                Id = c.Id
            }).ToList();

            var regionMap = new RegionGetDto
            {
                Id = region.Id,
                Active = region.Active,
                DDD = region.DDD,
                Description = region.Description,
            };

            return new ContactsGetDto
            {
                Contacts = contactsMap,
                Region = regionMap
            };
        }



        //ContactsGetDto
    }
}
