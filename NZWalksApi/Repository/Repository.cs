using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NZWalksApi.Data;
using NZWalksApi.Repository.IRepository;
namespace NZWalksApi.Repository
{
	public class Repository<T> : IRepository<T> where T:class
	{
        private readonly NZWalksDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(NZWalksDbContext db)
		{
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task AddAsync(T model)
        {
            await dbSet.AddAsync(model);
            await SaveAsync();

        }

        public async Task RemoveAsync(T model)
        {
            dbSet.Remove(model);
            await SaveAsync();
        }

        public async Task<List<T>>GetAllAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

