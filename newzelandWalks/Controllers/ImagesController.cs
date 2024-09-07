using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using newzelandWalks.Models.Domain;
using newzelandWalks.Models.DTO;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepo;

        public ImagesController(IImageRepository obj)
        {
            _imageRepo = obj;
        }
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                //convert dto to domain model 
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                await _imageRepo.Upload(imageDomainModel);
                return Ok(imageDomainModel);

            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowed = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowed.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "UnSupported file extension");
            }
            if (request.File.Length > 10485760) ModelState.AddModelError("file", "big file size");
        }
    }
}
