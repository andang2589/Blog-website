
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using BlogWebsite.DTO.Comment;
using Model = BlogWebsite.Data.Models;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public class CommentApiClient : ICommentApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            
        }

        public async Task<ClassResult<Model.Comment>> CreateNewComment(CreateCommentDto dto)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);


            var json = JsonConvert.SerializeObject(dto);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/Comment", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            //if(response.IsSuccessStatusCode)
            //{
            //    return JsonConvert.DeserializeObject<ClassResult<BlogPostDTO>>(result);
            //}
            return JsonConvert.DeserializeObject<ClassResult<Model.Comment>>(result);
        }



        public async Task<List<Model.Comment>> RootComment()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/Comment/");
            var body = await response.Content.ReadAsStringAsync();

            var rootComment = JsonConvert.DeserializeObject<List<Model.Comment>>(body);
            return rootComment;
        }

    }
}
