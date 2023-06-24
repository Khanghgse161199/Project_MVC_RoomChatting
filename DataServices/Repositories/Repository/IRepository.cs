using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<bool> AddAsync(T entity);
        Task<T> FindAsync(string Id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!, string includeProperties = null!);
        Task<T> FirstOfDefaultAsync(Expression<Func<T, bool>> filter = null!, string includeProperties = null!);
        Task<bool> RemoveAsync(string Id);
    }
}
