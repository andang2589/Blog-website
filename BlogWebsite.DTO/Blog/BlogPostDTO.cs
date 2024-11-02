using BlogWebsite.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = BlogWebsite.Data.Models;

namespace BlogWebsite.DTO.Blog
{
    public class BlogPostDTO
    {
        public int BlogPostID { get; set; }
        
        public string Title { get; set; }
        public string Content { get; set; }
        
        public IFormFile? Thumbnail { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        
        public List<int>? SelectedCategoryIds { get; set; }

        public List<CategoryDTO>? AllCategories { get; set; }
        
        public List<CategoryDTO>? Categories { get; set; }

        //public List<Comment>? Comments { get; set; }
    }

    public class CategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }


}
