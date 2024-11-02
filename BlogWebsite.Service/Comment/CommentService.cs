using AutoMapper;
using BlogWebsite.DTO.Comment;
using Model = BlogWebsite.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Service.Common;
using BlogWebsite.DTO.Common;
using Microsoft.EntityFrameworkCore;

namespace BlogWebsite.Service.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommonService<Model.Comment> _commentCmSv; //Đặt là commentCS thì sao???
        private readonly IMapper _mapper;
        public CommentService(IMapper mapper, ICommonService<Model.Comment> commentCmSv) 
        {
            _commentCmSv = commentCmSv;
            _mapper = mapper;
        }

        public async Task<ClassResult<Model.Comment>> CreateComment(CreateCommentDto dto)
        {
            

            try
            {
                var newComment = _mapper.Map<Model.Comment>(dto);
                newComment.DateCreated = DateTimeOffset.Now;
                newComment.DateModified = DateTimeOffset.Now;
                await _commentCmSv.AddAs(newComment);
                return ClassResult<Model.Comment>.SuccessResult();
            }
            catch (Exception ex)
            {
                return ClassResult<Model.Comment>.FailureResult(ex.ToString());
            }
        }

        public async Task<List<Model.Comment>> RootComment()
        {
            List<Model.Comment> comments = await _commentCmSv.TableT().AsNoTrackingWithIdentityResolution().Include(c=>c.Children).ToListAsync();


            List<Model.Comment> rootComments = comments.Where(c=>c.ParentID==null).AsParallel().ToList();

            return rootComments;
        }
    }
}
