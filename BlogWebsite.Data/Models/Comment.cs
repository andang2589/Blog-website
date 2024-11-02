using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Data.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int? ParentID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;        
        public string Content { get; set; } = string.Empty;
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
        public int BlogPostID { get; set; }
        public Comment Parent { get; set; }
        public ICollection<Comment> Children { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
