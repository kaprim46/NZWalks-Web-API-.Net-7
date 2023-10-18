using System;
using Microsoft.EntityFrameworkCore;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;
using NZWalksApi.Repository.IRepository;

namespace NZWalksApi.Repository
{
	public class RegionRepository : Repository<Region>,IRegionRepository
	{
        private readonly NZWalksDbContext _db;

        public RegionRepository(NZWalksDbContext db) : base(db)
		{
            _db = db;
        }

        public async Task<Region> UpdateAsync(Region model)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(q => q.Id == model.Id);
            if (region == null)
            {
                return null;
            }
            region.Id = model.Id;
            region.Code = model.Code;
            region.Name = model.Name;
            region.RegionImageUrl = model.RegionImageUrl;

            await SaveAsync();
            return region;
        }
    }
}

