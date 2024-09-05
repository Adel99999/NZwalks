using newzelandWalks.Models.Domain;

namespace newzelandWalks.Repository.Base
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk obj);
        Task<List<Walk>> GetAllAsync();
        Task<Walk?> GetAsync(Guid id);
        Task<Walk> UpdateAsync(Guid id, Walk obj);
        Task<Walk> DeleteAsync(Guid id);
    }
}
