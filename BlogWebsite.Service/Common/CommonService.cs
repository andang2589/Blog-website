using BlogWebsite.Data.DAL;
using BlogWebsite.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Service.Common
{
    public class CommonService<T> : GenericRepository<T>, ICommonService<T> where T : class
    {
       public CommonService(BlogWebsiteContext context) : base(context) { }

    }
}
