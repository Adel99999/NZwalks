using Microsoft.EntityFrameworkCore;
using newzelandWalks.Data;
using newzelandWalks.Models.Domain;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Repository
{
    
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDbContext _dbContext;
        public RegionRepository(AppDbContext obj)
        {
            _dbContext = obj;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var exist = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return null;
            _dbContext.Regions.Remove(exist);
            await _dbContext.SaveChangesAsync();
            return exist;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync(); 
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await _dbContext.Regions.FindAsync(id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existRegion = await _dbContext.Regions.FindAsync(id);
            if (existRegion == null) { return null; }
            existRegion.Code= region.Code;
            existRegion.Name= region.Name;
            existRegion.RegionImageUrl= region.RegionImageUrl;
            await _dbContext.SaveChangesAsync();
            return existRegion;
        }
    }
}
