using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostitories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private NZWAlksDBContext dbContext;
        private readonly IMapper mapper;
        public IRegionRepository regionRepository;

        public RegionsController(NZWAlksDBContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        //Custom validator
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddRegion([FromBody] AddRegionDto addRegionRequestDto)
        {
           if (addRegionRequestDto == null)
            {
                return StatusCode(StatusCodes.Status204NoContent);
            }

            //var region = mapper.Map<Region>

            var region = mapper.Map<Region>(addRegionRequestDto);

            await regionRepository.AddNewReion(region);
            //await dbContext.Regions.AddAsync(region);
            //await dbContext.SaveChangesAsync();


            var regionDto = mapper.Map<RegionDto>(region);

            return StatusCode(StatusCodes.Status201Created, regionDto);
            
        }

        //api/regions?filterOn=ColumnName&filterValue=ColumnValue
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegions([FromQuery] string? filterOn, [FromQuery] string? filterValue,
                                                        [FromQuery] string? sortBy, [FromQuery] bool? isAscending = true,
                                                        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize =2)
        {
            //getting data in domain model
            // var regions = await dbContext.Regions.ToListAsync();
            var regions = await regionRepository.GetAllRegions(pageNumber, pageSize, filterOn, filterValue, sortBy, isAscending ?? true);

            //passing data from domain model to DTO
            var regionsDto = mapper.Map<List<RegionDto>>(regions);                      
            
            //return dto to client
            return Ok(regionsDto);
        }

        ////https://localhost:7279/api/Regions/{id}
        /////swagger. postman returning wrong result. returing everything without filter
        [HttpGet ]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            //getting data in domain model
            var region = await regionRepository.GetRegionById(id);
            if (region == null)
            {
                return NotFound();
            }

            //passing data from domain model to DTO            
            var regionsDto = mapper.Map<RegionDto>(region);
            
            //return dto to client
            return Ok(regionsDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //Custom validator
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute]Guid id, [FromBody]UpdateRegionDto updateRegionDto)
        {
            var region = mapper.Map<Region>(updateRegionDto);
            
            //find the region from database
            region = await regionRepository.UpdateRegion(id, region);
            
            if (region == null)
            {
                return NotFound();
            } 

            //return dto to client
            return Ok(mapper.Map<RegionDto>(region));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            //getting data in domain model
            var region = await regionRepository.DeleteRegion(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionsDto = mapper.Map<RegionDto>(region);
           
            return StatusCode(StatusCodes.Status200OK, regionsDto);
        }

        ////https://localhost:7279/api/Regions/{id}}
        //swagger. postman returning wrong result. returing everything without filter
        //[HttpGet("{id:Guid}")]
        //public async Task<IActionResult> GetByRegionId(Guid id)
        //{
        //    var region = await dbContext.Regions.FindAsync(id);
        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(region);
        //}


        ////https://localhost:7279/api/Regions/GetRegionById?id={id}
        ////postman & swagger working alright
        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetRegionById(Guid id)
        //{
        //    var region = await dbContext.Regions.FindAsync(id);
        //    if (region == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(region);
        //}
    }
}
