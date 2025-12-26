using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostitories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllRegions(int pageNumber, int pageSize,
            string? filterOn = null, string? filterValue = null, string? sortBy = null, bool isAscending = true);

        Task<Region> GetRegionById(Guid id);

        Task<Region> AddNewReion(Region region);

        Task<Region> UpdateRegion(Guid id, Region region);

        Task<Region> DeleteRegion(Guid id);
    }
}
