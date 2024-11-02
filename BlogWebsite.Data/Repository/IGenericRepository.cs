using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        IEnumerable<T> GetAll();
        Task AddAs(T entity);
        Task UpdateAs(T entity);
        Task DeleteAs(T entity);
        DbSet<T> TableT();
    }
}
