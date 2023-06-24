
using DataServices.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly RoomChattingContext _db;
        internal DbSet<T> _dbSet;

        public Repository(RoomChattingContext context)
        {
            _db = context;
            this._dbSet = _db.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public async Task<T> FindAsync(string Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!, string includeProperties = null!)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return (await orderBy(query).ToListAsync());
            }

            if (includeProperties != null)
            {
                string[] splittedProperties = includeProperties.Split(',');
                foreach (string property in splittedProperties)
                {
                    if(property != null)
                    {
                        await query.Include(property).ToListAsync();
                    }
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> FirstOfDefaultAsync(Expression<Func<T, bool>> filter = null!, string includeProperties = null!)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                string[] splittedProperties = includeProperties.Split(',');
                foreach (string property in splittedProperties)
                {
                    if (property != null)
                    {
                        await query.Include(property).FirstOrDefaultAsync();
                    }
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveAsync(string Id)
        {
            T entity = await _dbSet.FindAsync(Id);
            Remove(entity);
            return true;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(List<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
