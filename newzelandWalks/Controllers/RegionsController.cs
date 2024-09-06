using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using newzelandWalks.CustomActionFilter;
using newzelandWalks.Data;
using newzelandWalks.Models.Domain;
using newzelandWalks.Models.DTO;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public RegionsController(IRegionRepository obj, IMapper mapper) {
            _regionRepository = obj;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionDomain = await _regionRepository.GetAllAsync();
            //var regionDtos = new List<RegionDto>();
            //foreach (var region in regionDomain)
            //{
            //    regionDtos.Add(new RegionDto()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}
            var regionDto = _mapper.Map<List<RegionDto>>(regionDomain);
            return Ok(regionDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        // when we work with mvc and click in edit we should first get the item we want to edit so we was use httpGet Action to get this and we should have the id when someone click in edit button so we was make TagHelper in html code so when one submit the button edit for example the id of the item from the route will send to the action and the action return the view and the element
        public async Task<IActionResult> Get([FromRoute] Guid id)   // to make the same thing we was do in mvc we should write [FromRoute] attribute before int id or Guid id and the id will be got from Route
        {
            var region = await _regionRepository.GetAsync(id);
            if (region == null) { return NotFound(); }

            return Ok(_mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        [ValidateMode]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto obj)
        {
           
                // map or convert DTO to Domain Model
                var regionDomainModel = _mapper.Map<Region>(obj);
                regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

                //map domain model back to Dto
                var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(Get), new { id = regionDto.Id }, regionDto);
           
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateMode]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto obj)
        {
            
                var domainModel = _mapper.Map<Region>(obj);
                domainModel = await _regionRepository.UpdateAsync(id, domainModel);
                if (domainModel == null) return NotFound();

                var regionDto = _mapper.Map<RegionDto>(domainModel);
                return Ok(regionDto);
          
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await _regionRepository.DeleteAsync(id);
            if(region==null)return NotFound();
            var regionDto = _mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }
    }
}
