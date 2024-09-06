using newzelandWalks.Models.Domain;

namespace newzelandWalks.Repository.Base
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk obj);
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true,
            int pageNumber=1,int pageSize=1000);
        Task<Walk?> GetAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk obj);
        Task<Walk> DeleteAsync(Guid id);
    }
}
