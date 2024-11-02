using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Service.Blog
{
    public interface IBlogService
    {
        Task<ClassResult<BlogPostDTO>> CreateNewPost(BlogPostDTO blogPostDTO);
        Task<ClassResult<BlogPostDTO>> GetPost(int id);
        Task<ClassResult<BlogPostDTO>> UpdatePost(BlogPostDTO dto);
        Task<ClassResult<List<BlogPostDTO>>> GetPostsList();
        Task<ClassResult<BlogPostDTO>> DeletePost(int id);
    }
}
