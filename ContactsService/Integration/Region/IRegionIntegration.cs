using Integration.Region.Dtos;
using Refit;

namespace Integration.Region
{
    public interface IRegionIntegration
    {
        [Get("/Regions/id/{id}")]
        Task<RegionRequestGetDto> GetById(Guid id);

        [Get("/Regions/ddd/{ddd}")]
        Task<RegionRequestGetDto> GetByDDD(string ddd);
    }
}
