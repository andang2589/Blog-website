using BlogWebsite.Areas.Admin.Helper;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Category;
using BlogWebsite.DTO.Common;
//using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
//using NuGet.Configuration;
//using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public class BlogApiClient : IBlogApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClientHelper _httpClientHelper;

        public BlogApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, HttpClientHelper httpClientHelper)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _httpClientHelper = httpClientHelper;
        }

        public async Task<ClassResult<BlogPostDTO>> CreateNewPost(BlogPostDTO blogPostDTO)
        {
            var client =  _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            //byte[] data;
            //using (var br = new BinaryReader(blogPostDTO.ThumbnailUrl.OpenReadStream()))
            //{
            //    data= br.ReadBytes((int)blogPostDTO.ThumbnailUrl.OpenReadStream().Length);
            //}
            //ByteArrayContent bytes = new ByteArrayContent(data);
            //requestContent.Add(bytes, "ThumnailUrl", blogPostDTO.ThumbnailUrl.FileName);
            if(blogPostDTO.Thumbnail != null ) 
            {
                var streamContent = new StreamContent(blogPostDTO.Thumbnail.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blogPostDTO.Thumbnail.ContentType);
                requestContent.Add(streamContent, "thumbnail", blogPostDTO.Thumbnail.FileName);
            }
            
            requestContent.Add(new StringContent(blogPostDTO.Title),"title");
            requestContent.Add(new StringContent(blogPostDTO.Content), "content");
            requestContent.Add(new StringContent(blogPostDTO.Description), "description");
            //requestContent.Add(new StringContent(blogPostDTO.CreateDate.ToString()), "createDate");
            // Chuyển đổi danh sách thành JSON
            var json = JsonConvert.SerializeObject(blogPostDTO.SelectedCategoryIds);
            requestContent.Add(new StringContent(json), "id");

            var response = await client.PostAsync($"/api/Blog", requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ClassResult<BlogPostDTO>>(result);
            }
            return ClassResult<BlogPostDTO>.FailureResult("Error !!!");
        }

        public async Task<ClassResult<BlogPostDTO>> GetPost(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/Blog/{id}");
            var body = await response.Content.ReadAsStringAsync();

            var post = JsonConvert.DeserializeObject<ClassResult<BlogPostDTO>>(body);
            return post;
        }

        public async Task<ClassResult<BlogPostDTO>> UpdatePost(BlogPostDTO dto)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            if (dto.Thumbnail != null)
            {
                var streamContent = new StreamContent(dto.Thumbnail.OpenReadStream());
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.Thumbnail.ContentType);
                requestContent.Add(streamContent, "thumbnail", dto.Thumbnail.FileName);
            }
            //var streamContent = new StreamContent(dto.Thumbnail.OpenReadStream());
            //streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.Thumbnail.ContentType);
            //requestContent.Add(streamContent, "thumbnail", dto.Thumbnail.FileName);
            requestContent.Add(new StringContent(dto.Title), "title");
            requestContent.Add(new StringContent(dto.BlogPostID.ToString()), "BlogPostID");

            requestContent.Add(new StringContent(dto.Content), "content");
            requestContent.Add(new StringContent(dto.ThumbnailUrl), "thumbnailUrl");
            requestContent.Add(new StringContent(dto.Description), "description");

            // Chuyển đổi danh sách thành JSON
            var json = JsonConvert.SerializeObject(dto.SelectedCategoryIds);
            requestContent.Add(new StringContent(json), "id");

            var response = await client.PutAsync($"/api/Blog/", requestContent);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadAsStringAsync();
          
            return JsonConvert.DeserializeObject<ClassResult<BlogPostDTO>>(result);
        }

        public async Task<ClassResult<List<BlogPostDTO>>> GetPostsList()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/Blog/GetPostsList");
            var body = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return ClassResult<List<BlogPostDTO>>.UnauthorizedResult();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return ClassResult<List<BlogPostDTO>>.ForbiddenResult();
            }
            //var jsonString = System.Text.Encoding.UTF8.GetString(body);
            var post = JsonConvert.DeserializeObject<ClassResult<List<BlogPostDTO>>>(body);
            return post;
        }

        public async Task<ClassResult<BlogPostDTO>> DeletePost(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.DeleteAsync($"api/Blog/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return ClassResult<BlogPostDTO>.SuccessResult();
            }
            return ClassResult<BlogPostDTO>.FailureResult(body);
        }
    }
}
