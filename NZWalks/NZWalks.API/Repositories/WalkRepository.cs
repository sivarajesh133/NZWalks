using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Xml.Linq;

namespace NZWalks.API.Repositories
{
    public class WalkRepository :IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            
           this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id=Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);  
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks
                 .Include(x => x.Region)
                 .Include(x => x.WalkDifficulty)
                 .FirstOrDefaultAsync(x => x.Id == id);
                  nZWalksDbContext.Walks.Remove(walk);
           await  nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllWalksAsync()
        {
            return await nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync(); 
        }

        public async Task<Walk> GetWalksAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existWalk=await nZWalksDbContext.Walks
                
                .FindAsync(id);
            if(existWalk==null)
            {
                return null;

            }
            existWalk.Length = walk.Length;
            existWalk.Name=walk.Name;
            existWalk.RegionId=walk.RegionId;
            existWalk.WalkDifficultyId = walk.WalkDifficultyId;
            await  nZWalksDbContext.SaveChangesAsync();
            return existWalk;
               


        }
    }
}
