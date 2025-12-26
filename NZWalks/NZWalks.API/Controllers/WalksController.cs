using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostitories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //api/walks?filterOn=ColumnName&filterValue=ColumnValue&sortBy=SortOnColumn&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAllWalk([FromQuery] string? filterOn, [FromQuery] string? filterValue, 
                                                    [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
                                                    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var walks = await walkRepository.GetAllWalks(filterOn, filterValue, sortBy, isAscending ?? true, pageNumber, pageSize);

            if (walks == null)
                return NotFound();

            return StatusCode(StatusCodes.Status200OK, mapper.Map<List<Walk>>(walks));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            var walk = await walkRepository.GetWalkById(id);

            if (walk == null)
                return NotFound();

            return StatusCode(StatusCodes.Status200OK, mapper.Map<Walk>(walk));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> AddWalk([FromBody] AddWalkDto addWalkDto)
        {
            
            var walk = mapper.Map<Walk>(addWalkDto);
            await walkRepository.AddNewWalk(walk);

            return Ok(mapper.Map<Walk>(walk));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]   
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            var walk = mapper.Map<Walk>(updateWalkDto);
            walk = await walkRepository.UpdateWalk(id, walk);

            if (walk == null)
                return NotFound();

            return Ok(mapper.Map<Walk>(walk));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var walk = await walkRepository.DeleteWalk(id);

            if (walk == null)
                return NotFound();

            return Ok(mapper.Map<Walk>(walk));
        }
    }
}
