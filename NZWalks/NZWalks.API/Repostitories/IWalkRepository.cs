using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repostitories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllWalks(string? filterOn = null, string? filterValue = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 10);

        Task<Walk?> GetWalkById(Guid id);

        Task<Walk> AddNewWalk(Walk walk);

        Task<Walk?> UpdateWalk(Guid id, Walk walk);

        Task<Walk?> DeleteWalk(Guid id);
    }
}
