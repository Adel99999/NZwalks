using newzelandWalks.Models.Domain;

namespace newzelandWalks.Repository.Base
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
