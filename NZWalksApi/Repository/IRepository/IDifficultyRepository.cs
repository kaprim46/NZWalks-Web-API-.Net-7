using System;
using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repository.IRepository
{
	public interface IDifficultyRepository : IRepository<Difficulty>
	{
		Task<Difficulty> UpdateAsync(Difficulty model);
	}
}

