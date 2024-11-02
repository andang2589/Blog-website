using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Models
{
    public class BlogPost
    {
        public int BlogPostID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Comment>? Comments { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
