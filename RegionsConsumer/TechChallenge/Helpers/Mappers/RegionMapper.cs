using Domain.Region.Entity;
using TechChallenge.Controllers.Regions.Dto;

namespace TechChallenge.Helpers.Mappers
{
    public static class RegionMapper
    {
        public static RegionEntity MapRegionPost(RegionPostDto postDto) 
        { 
            var region = new RegionEntity
            {
                Id = Guid.NewGuid(),
                Active = postDto.Active,
                CreationDate = DateTime.UtcNow.AddHours(-3),
                DDD = postDto.DDD,
                Description = postDto.Description,
            };

            return region;
        }
    }
}
