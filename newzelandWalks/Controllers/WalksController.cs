using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using newzelandWalks.CustomActionFilter;
using newzelandWalks.Models.Domain;
using newzelandWalks.Models.DTO;
using newzelandWalks.Repository;
using newzelandWalks.Repository.Base;

namespace newzelandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper obj,IWalkRepository walk)
        {
            _mapper = obj;
            _walkRepository = walk;
        }
        [HttpPost]
        [ValidateMode]
        public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestDto obj)
        {
            
                // Map DTO to Domain Model
                var walkModel = _mapper.Map<Walk>(obj);
                await _walkRepository.CreateAsync(walkModel);
                //map domainModel to DTO
                var walkDto = _mapper.Map<WalkDto>(walkModel);
                return Ok(walkDto);
           
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery] string?filterOn , [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool isAscending,  
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize =1000)
        {
            var walksDomainModel =await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending,pageNumber,pageSize);
            //map domain to dto
            return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetAsync(id);
            if(walkDomainModel==null)return NotFound();
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateMode]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {

                // Map DTO to Domain Model
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

                walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                // Map Domain Model to DTO
                return Ok(_mapper.Map<WalkDto>(walkDomainModel));
          
        }


        // Delete a Walk By Id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
