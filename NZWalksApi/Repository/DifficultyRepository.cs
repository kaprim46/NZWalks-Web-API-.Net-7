using System;
using Microsoft.EntityFrameworkCore;
using NZWalksApi.Data;
using NZWalksApi.Models.Domain;
using NZWalksApi.Repository.IRepository;

namespace NZWalksApi.Repository
{
	public class DifficultyRepository : Repository<Difficulty>, IDifficultyRepository
    {
        private readonly NZWalksDbContext _db;

        public DifficultyRepository(NZWalksDbContext db) : base(db)
		{
            _db = db;
        }

        public async Task<Difficulty>UpdateAsync(Difficulty model)
        {
            var difficulty = await _db.Difficulties.FirstOrDefaultAsync(q => q.Id == model.Id);
            if (difficulty == null)
            {
                return null;
            }
            difficulty.Name = model.Name;
            await SaveAsync();
            return model;
        }
    }
}

