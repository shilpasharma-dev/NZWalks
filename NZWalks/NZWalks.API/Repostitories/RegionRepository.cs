using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repostitories
{
    public class RegionRepository : IRegionRepository

    {
        private readonly NZWAlksDBContext dbContext;

        public RegionRepository(NZWAlksDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> AddNewReion(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegion(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }

            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<List<Region>> GetAllRegions(int pageNumber, int pageSize, 
                                                      string? filterOn = null, string? filterValue = null, 
                                                      string? sortBy = null, bool isAscending = true )
        {
        var regions = dbContext.Regions.AsQueryable();

            //filter
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterValue) == false)
            {
                if (filterOn.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    regions = regions.Where(x => x.Code.Contains(filterValue));
                }
                else if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regions = regions.Where(x => x.Name.Contains(filterValue));
                }
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy)){
                if (sortBy.Equals("Code", StringComparison.OrdinalIgnoreCase))
                {
                    regions = isAscending ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    regions = isAscending ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Name);
                }
            }

            //sorting
            var skipPages = (pageNumber - 1) * pageSize;

            regions = regions.Skip(skipPages).Take(pageSize);

            return await regions.ToListAsync();
        }

        public async Task<Region?> GetRegionById(Guid id)
        {
            //return await dbContext.Regions.FindAsync(id);
           return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Region?> UpdateRegion(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return null;
            }

            //assign value of dto to data model
            //region.Id = regionDto.Id;
            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            //update region with dto vcalues
            await dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
