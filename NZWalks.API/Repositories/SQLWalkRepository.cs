﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

         public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
         public async Task<Walk> CreateAsync(Walk walk)
        {
             await dbContext.Walks.AddAsync(walk);
             await dbContext.SaveChangesAsync();
             return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var exisitingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (exisitingWalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(exisitingWalk);
            await dbContext.SaveChangesAsync();
            return exisitingWalk;

        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            [FromQuery] string? sortBy = null, [FromQuery] bool isAscending = true, int pageNumber = 1, int pageSize = 10)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering
            if( !string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if(filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(walk=>walk.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if(sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
 
        }

        public async Task<Walk?> GetByIdAsync(Guid Id)
        {
           return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == Id);
         
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
