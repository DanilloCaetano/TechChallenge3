using Domain.Region.Entity;
using Domain.Region.Repository;
using Infraestructure.Context;
using Infraestructure.Repository.Base;

namespace Infraestructure.Repository.RegionRepository
{
    public class RegionRepository : BaseRepository<RegionEntity>, IRegionRepository
    {
        private readonly TechChallengeContext _context;

        public RegionRepository(TechChallengeContext context) : base(context)
        {
            _context = context;
        }
    }
}
