using BlogWebsite.DTO.Comment;
using BlogWebsite.DTO.Common;
using Model = BlogWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Service.Comment
{
    public interface ICommentService
    {
        Task<ClassResult<Model.Comment>> CreateComment(CreateCommentDto dto);
        Task<List<Model.Comment>> RootComment();
    }
}
