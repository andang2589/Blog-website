using BlogWebsite.DTO.Comment;
using BlogWebsite.DTO.Common;
using Model = BlogWebsite.Data.Models;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public interface ICommentApiClient
    {
        Task<ClassResult<Model.Comment>> CreateNewComment(CreateCommentDto dto);

        Task<List<Model.Comment>> RootComment();
    }
}
