using Domain.Contact.Entity;
using Domain.Region.Entity;
using Domain.Region.Repository;

namespace Domain.Region.Service
{
    public interface IRegionService
    {
        Task<RegionEntity> GetById(Guid id);
        Task<List<RegionEntity>> GetAllRegions();
        Task<RegionEntity> GetByDDD(string ddd);
    }
}
