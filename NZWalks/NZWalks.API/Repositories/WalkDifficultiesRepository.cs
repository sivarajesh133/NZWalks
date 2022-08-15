﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultiesRepository : IWalkDifficultiesRepository

    {
        private readonly NZWalksDbContext nZWalksDbContext;
        

        public WalkDifficultiesRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
           
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;

        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficulty = await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(walkDifficulty == null)
            {
                return null;
            }
            nZWalksDbContext.WalkDifficulty.Remove(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
          return  await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
          var walkDifficulty= await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(walkDifficulty==null)
            {
                return null;
            }
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
           var existingWalkDifficulty= await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(existingWalkDifficulty==null)
            {
                return null;
            }
            existingWalkDifficulty.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;

        }
    }
}
