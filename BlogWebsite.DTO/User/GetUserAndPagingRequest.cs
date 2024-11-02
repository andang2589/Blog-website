using BlogWebsite.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.User
{
    public class GetUserAndPagingRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
    }
}
