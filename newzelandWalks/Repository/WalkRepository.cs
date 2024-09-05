using Microsoft.EntityFrameworkCore;
using newzelandWalks.Data;
using newzelandWalks.Models.Domain;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly AppDbContext _dbContext;
        public WalkRepository(AppDbContext db)
        {
            _dbContext = db;
        }
        public async Task<Walk> CreateAsync(Walk obj)
        {
            await _dbContext.Walks.AddAsync(obj);
            await _dbContext.SaveChangesAsync();
            return obj;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk =await _dbContext.Walks.FindAsync(id);
            if (walk == null) return null;
            _dbContext.Walks.Remove(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetAsync(Guid id)
        {
           return await _dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk obj)
        {
            var exist = await _dbContext.Walks.FindAsync(id);
            if (exist == null) { return null; }
            exist.Name=obj.Name;
            exist.Description=obj.Description;
            exist.LengthInKm = obj.LengthInKm;
            exist.WalkImageUrl=obj.WalkImageUrl;
            exist.DifficultyId=obj.DifficultyId;
            exist.RegionId=obj.RegionId;
            await _dbContext.SaveChangesAsync();
            return exist;
        }
    }
}
