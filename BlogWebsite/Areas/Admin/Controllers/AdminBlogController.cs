using AutoMapper;
using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.Areas.Admin.Helper;
using BlogWebsite.DTO.Blog;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Areas.Admin.Controllers
{
    public class AdminBlogController : Controller
    {

        private readonly IBlogApiClient _blogApiClient;
        private readonly IMapper _mapper;
        private readonly ICategoryApiClient _categoryApiClient;

        public AdminBlogController(IBlogApiClient blogApiClient, IMapper mapper, ICategoryApiClient categoryApiClient)
        {
            _blogApiClient = blogApiClient;
            _mapper = mapper;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogApiClient.GetPostsList();

            var authorizationCheck = AuthorizationHelper.HandleAuthorization(posts, this);
            if (authorizationCheck != null)
            {
                return authorizationCheck;
            }

            return View(posts.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreatePost()
        {
            var post = new BlogPostDTO();
            var list = await _categoryApiClient.GetListCategories();

            var model = (post, list.Data.AsEnumerable());
            
            return View(model);
        }


        
        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> CreatePost(BlogPostDTO blogPostDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(blogPostDTO);
            }
            await _blogApiClient.CreateNewPost(blogPostDTO);
            return RedirectToAction("index");
        }


        public IActionResult Test3()
        {
            return View();
        }

        public async Task<IActionResult> DisplayContent(int id)
        {
            var post = await _blogApiClient.GetPost(id);
            
            return View(post.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUpdatePost(int id)
        {
            var post = await _blogApiClient.GetPost(id);
            return Content(post.Data.Content,"text/html");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePost(int id)
        {
            var post = await _blogApiClient.GetPost(id);
            return View(post.Data);
        }
        [HttpPut]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePost(BlogPostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _blogApiClient.UpdatePost(dto);
            return RedirectToAction("Index","AdminBlog");
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
             await _blogApiClient.DeletePost(id);
            return NoContent();

        }

        
    }
}
