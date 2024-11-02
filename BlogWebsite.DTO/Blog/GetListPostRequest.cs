using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = BlogWebsite.Data.Models;

namespace BlogWebsite.DTO.Blog
{
    public class GetListPostRequest
    {
        public int BlogPostID { get; set; }

        public string Title { get; set; }

        public IFormFile? Thumbnail { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<Model.Category>? Categories { get; set; }
        //public List<Model.Category>? AllCategories { get; set; }
        //public string Content { get; set; }
        //public List<int>? SelectedCategoryIds { get; set; }
    }
}
