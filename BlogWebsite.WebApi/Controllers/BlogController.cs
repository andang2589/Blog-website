using BlogWebsite.DTO.Blog;
using BlogWebsite.Service.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogWebsite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateBlog([FromForm]BlogPostDTO blogPostDTO)
        {
            var formCollection = await Request.ReadFormAsync();
            var jsonString = formCollection["id"];
            List<int> numbers= JsonConvert.DeserializeObject<List<int>>(jsonString);
            blogPostDTO.SelectedCategoryIds = numbers;
            //var blog = JsonConvert.DeserializeObject<BlogPostDTO>(blogPostDTO.ToString());
            //List<int> models = JsonConvert.DeserializeObject(blogPostDTO.SelectedCategoryIds.ToString());
            //var cates = blogPostDTO.SelectedCategoryIds.ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _blogService.CreateNewPost(blogPostDTO);
            return Ok();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var result = await _blogService.GetPost(id);
            return Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UpdatePost([FromForm]BlogPostDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var formCollection = await Request.ReadFormAsync();
            var jsonString = formCollection["id"];
            List<int> numbers = JsonConvert.DeserializeObject<List<int>>(jsonString);
            dto.SelectedCategoryIds = numbers;
            var result = await _blogService.UpdatePost(dto);
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("GetPostsList")]
        public async Task<IActionResult> GetPostsList()
        {

            var result = await _blogService.GetPostsList();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var result = await _blogService.DeletePost(id);
            return Ok(result);
        }
    }

    public class TestApi
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    }
}
