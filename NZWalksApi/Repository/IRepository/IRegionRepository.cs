using System;
using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repository.IRepository
{
	public interface IRegionRepository : IRepository<Region>
	{
		Task<Region> UpdateAsync(Region model);
	}
}

