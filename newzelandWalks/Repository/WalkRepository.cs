using Microsoft.EntityFrameworkCore;
using newzelandWalks.Data;
using newzelandWalks.Models.Domain;
using newzelandWalks.Repository.Base;
using System;

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

        public async Task<List<Walk>> GetAllAsync(
            string?filterOn=null,string?filterQuery = null,
            string?sortBy=null,bool isAscending=true,
            int pageNumber=1,int pageSize=1000)
        {
            //return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            var obj = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering
            if(string.IsNullOrEmpty(filterOn)==false && string.IsNullOrEmpty(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    obj = obj.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrEmpty(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    obj = isAscending ? obj.OrderBy(x => x.Name) : obj.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    obj=isAscending?obj.OrderBy(x=>x.LengthInKm):obj.OrderByDescending(x=>x.LengthInKm);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await obj.Skip(skipResults).Take(pageSize).ToListAsync();
            
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
