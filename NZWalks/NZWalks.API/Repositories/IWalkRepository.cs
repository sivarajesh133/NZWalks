﻿using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
       Task <IEnumerable<Walk>> GetAllWalksAsync();
       Task <Walk> GetWalksAsync(Guid id);

        Task<Walk> AddAsync(Walk walk);
        Task<Walk> UpdateAsync(Guid id, Walk walk);
        Task<Walk> DeleteAsync(Guid id);  
    }
}
