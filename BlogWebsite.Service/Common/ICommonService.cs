using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Data.Repository;

namespace BlogWebsite.Service.Common
{
    public interface ICommonService<T> : IGenericRepository<T> where T : class
    {
    }
}
