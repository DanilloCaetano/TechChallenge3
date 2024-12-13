using Domain.Region.Entity;
using Domain.Region.Repository;

namespace Domain.Region.Service
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;

        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }


        public async Task<RegionEntity> GetById(Guid id)
        {
            return await _regionRepository.GetByIdAsync(id);
        }

        public async Task<List<RegionEntity>> GetAllRegions()
        {
            var regions = await _regionRepository.GetAsync(r => r.Active, r => r.Description);

            return regions.ToList();
        }

        public async Task<RegionEntity> GetByDDD(string ddd)
        {
            return await _regionRepository.GetFirstAsync(x => x.DDD.ToLower() == ddd.ToLower());
        }
    }
}
