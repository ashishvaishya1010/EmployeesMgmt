using Employee_Management_System.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Employee_Management_System.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbSet;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            
            //_db.VillaNumbers.Include(u => u.Villa).ToList();
            _dbSet = db;
            //added for testing joint
            _dbSet.empolyees.Include(u => u.Department).ToList();
            _dbSet.empolyees.Include(u => u.Designation).ToList();
            this.dbSet = _dbSet.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            _dbSet.Set<T>().Add(entity);
            await _dbSet.SaveChangesAsync();

        }

        //public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            return await _dbSet.Set<T>().ToListAsync();

        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }


            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Set<T>().Remove(entity);
            _dbSet.SaveChangesAsync();

        }

        public async Task SaveAsync(T entity)
        {
            await _dbSet.SaveChangesAsync();
        }
    }

}