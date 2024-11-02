using BlogWebsite.Data.DAL;
using BlogWebsite.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BlogWebsiteContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(BlogWebsiteContext context) 
        {
            _context = context;
            _dbSet = _context.Set<T>(); 
        }

        
        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
            //return default(T);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task AddAs(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAs(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            if(entity is BlogPost blogPost)
            {
                _context.Entry(blogPost).Property(p => p.CreateDate).IsModified = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAs(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

        }

        public DbSet<T> TableT() { return _dbSet; }
    }
}
