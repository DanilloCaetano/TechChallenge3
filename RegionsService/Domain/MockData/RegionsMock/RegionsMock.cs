using Domain.Contact.Entity;
using Domain.Region.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MockData.RegionsMock
{
    public static class RegionsMock
    {
        public static List<RegionEntity> GetMockRegions()
        {
            return new List<RegionEntity>
        {
            new RegionEntity
            {
                Id = Guid.Parse("efe16b65-feca-402a-8e21-2d1843f0d313"),
                Description = "Baixada Santista",
                DDD = "13",
                Active = true,
                CreationDate = DateTime.UtcNow.AddHours(-3),
            },
            new RegionEntity
            {
                Id = Guid.Parse("cefa4129-b1d8-4eb5-b278-86d86d8763c2"),
                Description = "São Paulo",
                DDD = "11",
                Active = true,
                CreationDate = DateTime.UtcNow.AddHours(-3),
            },
            new RegionEntity
            {
                Id = Guid.Parse("01378b5d-fdc4-4435-b595-6cfad33ef4a6"),
                Description = "Rio de Janeiro",
                DDD = "21",
                Active = true,
                CreationDate = DateTime.UtcNow.AddHours(-3),
            }
        };
        }
    }
}
