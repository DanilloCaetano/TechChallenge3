using Domain.Contact.Entity;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MockData.ContactsMock
{
    public static class ContactsMock
    {

        public static List<ContactEntity> GetMockContacts()
        {
            return new List<ContactEntity>
        {
            new ContactEntity
            {
                Id = Guid.Parse("39b557cf-682e-4437-8c4b-db19ee52f25d"),
                Name = "João Silva",
                CellphoneNumber = "123456789",
                Email = "joaosilva@example.com",
                CreationDate = DateTime.UtcNow.AddHours(-3),
                Active = true,
                RegionId = Guid.Parse("efe16b65-feca-402a-8e21-2d1843f0d313"),
            },
            new ContactEntity
            {
                Id = Guid.Parse("d9f37035-4479-4dba-a3b6-f5c918e21736"),
                Name = "Maria Oliveira",
                CellphoneNumber = "123456789",
                Email = "mariaoliveira@example.com",
                CreationDate = DateTime.UtcNow.AddHours(-3),
                Active = true,
                RegionId = Guid.Parse("cefa4129-b1d8-4eb5-b278-86d86d8763c2"),
            },
            new ContactEntity
            {
                Id = Guid.Parse("18887b4a-437d-4c62-949c-00c4a890e35a"),
                Name = "Carlos Pereira",
                CellphoneNumber = "123456789",
                Email = "carlospereira@example.com",
                CreationDate = DateTime.UtcNow.AddHours(-3),
                Active = true,
                RegionId = Guid.Parse("01378b5d-fdc4-4435-b595-6cfad33ef4a6"),
            }
        };
        }
    }
}
