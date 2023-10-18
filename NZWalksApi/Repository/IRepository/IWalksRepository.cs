using System;
using NZWalksApi.Models.Domain;

namespace NZWalksApi.Repository.IRepository
{
	public interface IWalksRepository : IRepository<Walk>
	{
		Task<Walk> UpdateAsync(Walk model);
	}
}

