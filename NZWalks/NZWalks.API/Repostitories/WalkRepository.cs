using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostitories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWAlksDBContext dbContext;

        public WalkRepository(NZWAlksDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> AddNewWalk(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteWalk(Guid id)
        {
            var walk = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

            if (walk != null)
            {
                dbContext.Walks.Remove(walk);
                await dbContext.SaveChangesAsync();
            }

            return walk;
        }

        public async Task<List<Walk>> GetAllWalks(string? filterOn = null, string? filterValue = null, 
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10)
        {
            var walks = dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .AsQueryable();


            //Filter based on Column Name
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterValue) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterValue));
                }
                else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterValue));
                }
                else if (filterOn.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.LengthInKm == Double.Parse(filterValue));
                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Description) : walks.OrderByDescending(x => x.Description);
                }
                else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipPages = (pageNumber - 1) * pageSize;
            walks = walks.Skip(skipPages).Take(pageSize);

            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetWalkById(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateWalk(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalk != null)
            {
                existingWalk.Name = walk.Name;
                existingWalk.Description = walk.Description;
                existingWalk.LengthInKm = walk.LengthInKm;
                existingWalk.WalkImageUrl = walk.WalkImageUrl;
                existingWalk.RegionId = walk.RegionId;
                existingWalk.DifficultyId = walk.DifficultyId;

                await dbContext.SaveChangesAsync();
            }

            return existingWalk;
        }
    }
}
