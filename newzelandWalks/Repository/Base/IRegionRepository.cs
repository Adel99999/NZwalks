using newzelandWalks.Models.Domain;

namespace newzelandWalks.Repository.Base
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region> GetAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id ,Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
