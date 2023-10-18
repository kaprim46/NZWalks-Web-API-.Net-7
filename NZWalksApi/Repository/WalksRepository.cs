using System;
using Microsoft.EntityFrameworkCore;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;
using NZWalksApi.Models.DTO;
using NZWalksApi.Repository.IRepository;

namespace NZWalksApi.Repository
{
	public class WalksRepository : Repository<Walk>,IWalksRepository
	{
        private readonly NZWalksDbContext _db;

        public WalksRepository(NZWalksDbContext db) : base(db)
		{
            _db = db;
        }

        public async Task<Walk> UpdateAsync(Walk model)
        {
            var walk = await _db.Walks.FirstOrDefaultAsync(q => q.Id == model.Id);
            if (walk == null)
            {
                return null;
            }
            walk.Name = model.Name;
            walk.Description = model.Description;
            walk.LengthInKm = model.LengthInKm;
            walk.DifficultyId = model.DifficultyId;
            walk.RegionId = model.RegionId;
            walk.WalkImage = model.WalkImage;

            await SaveAsync();
            return walk;
        }
    }
}

