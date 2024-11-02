using BlogWebsite.DTO.Comment;
using BlogWebsite.Service.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService) 
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto dto)
        {
            var result = await _commentService.CreateComment(dto);
            return Ok(result);
        }

        public async Task<IActionResult> RootComment()
        {
            var result = await _commentService.RootComment();
            return Ok(result);
        }
    }
}
