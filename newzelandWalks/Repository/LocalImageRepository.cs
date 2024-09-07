using newzelandWalks.Data;
using newzelandWalks.Models.Domain;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Repository
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _appDbContext;
        public LocalImageRepository(IWebHostEnvironment obj, IHttpContextAccessor httpContextAccessor,AppDbContext app)
        {
            _webHostEnvironment = obj;
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = app;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath =
                Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
                $"{image.FileName}{image.FileExtension}");

            // upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;


            //add image to the images table in database
            await _appDbContext.Images.AddAsync(image);
            await _appDbContext.SaveChangesAsync();
            return image;
        }
    }
}
