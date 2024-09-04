using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using newzelandWalks.Data;
using newzelandWalks.Models.Domain;
using newzelandWalks.Models.DTO;

namespace newzelandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public RegionsController(AppDbContext DbContext) {
           _dbContext = DbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionDomain = _dbContext.Regions.ToList();
            var regionDtos = new List<RegionDto>();
            foreach (var region in regionDomain)
            {
                regionDtos.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionDtos);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        // when we work with mvc and click in edit we should first get the item we want to edit so we was use httpGet Action to get this and we should have the id when someone click in edit button so we was make TagHelper in html code so when one submit the button edit for example the id of the item from the route will send to the action and the action return the view and the element
        public IActionResult Get([FromRoute]Guid id)   // to make the same thing we was do in mvc we should write [FromRoute] attribute before int id or Guid id and the id will be got from Route
        {
            var region = _dbContext.Regions.Find(id);
            if (region == null) { return NotFound(); }
            var regionsDto = new RegionDto()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionsDto);
        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto obj)
        {
            // map or convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = obj.Code,
                RegionImageUrl = obj.RegionImageUrl,
                Name = obj.Name
            };
            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

            //map domain model back to Dto
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(Get), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto obj)
        {
            var region=_dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(region==null)return NotFound();
            //map Dto to Domain Model 
            region.Code= obj.Code;
            region.RegionImageUrl= obj.RegionImageUrl;
            region.Name= obj.Name;
            _dbContext.SaveChanges();
            //convert Domain model  to DTO to send it in the Ok message 
            var regionDto = new RegionDto
            {
                Id =region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var region = _dbContext.Regions.FirstOrDefault(x=>x.Id == id);
            if(region==null)return NotFound();
            _dbContext.Regions.Remove(region);
            _dbContext.SaveChanges();
            var regionDto = new RegionDto
            {
                Id =region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}
