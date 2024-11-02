using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Common;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public interface IBlogApiClient
    {
        Task<ClassResult<BlogPostDTO>> CreateNewPost(BlogPostDTO blogPostDTO);

        Task<ClassResult<BlogPostDTO>> GetPost(int id);
        Task<ClassResult<BlogPostDTO>> UpdatePost(BlogPostDTO dto);
        Task<ClassResult<List<BlogPostDTO>>> GetPostsList();
        Task<ClassResult<BlogPostDTO>> DeletePost(int id);
    }
}
