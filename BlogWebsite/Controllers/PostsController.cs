using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Comment;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace BlogWebsite.Controllers
{
    public class PostsController : Controller
    {
        private readonly ICommentApiClient _commentApiClient;
        private readonly IBlogApiClient _blogApiClient;
        public PostsController(ICommentApiClient commentApiClient, IBlogApiClient blogApiClient) 
        {
            _commentApiClient = commentApiClient;
            _blogApiClient = blogApiClient;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateComment(/*int postId,*/ CreateCommentDto dto)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }
            await _commentApiClient.CreateNewComment(dto);
            return RedirectToAction("Index","Posts");
        }


        public async Task<IActionResult> ShowPostDetail(int id)
        {
            var post = await _blogApiClient.GetPost(id);
            var comments = await _commentApiClient.RootComment();
            var model = (comments.AsEnumerable(), post.Data); //?????
            //dynamic mymodel = new ExpandoObject();
            return View(model);
        }
    }
}
