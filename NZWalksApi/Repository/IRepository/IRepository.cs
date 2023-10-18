using System;
using System.Linq.Expressions;

namespace NZWalksApi.Repository.IRepository
{
	public interface IRepository<T> where T: class
	{
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null);
		Task AddAsync(T model);
		Task RemoveAsync(T model);
		Task SaveAsync();
	}
}

