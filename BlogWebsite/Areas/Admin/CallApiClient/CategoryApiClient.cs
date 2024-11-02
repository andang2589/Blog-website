using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Blog;
using BlogWebsite.DTO.Category;
using BlogWebsite.DTO.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using BlogWebsite.DTO.Role;
using System.Net;
using BlogWebsite.Areas.Admin.Helper;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClientHelper _httpClientHelper;
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, HttpClientHelper httpClientHelper) 
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _httpClientHelper = httpClientHelper;
        }


        public async Task<ClassResult<CategoryDto>> CreateCategory(CategoryDto dto)
        {
            return await _httpClientHelper.SendPostRequest<CategoryDto>("/api/Category", dto);
        }

        public async Task<ClassResult<List<CategoryDto>>> GetListCategories()
        {
            return await _httpClientHelper.SendGetRequest<List<CategoryDto>>("/api/Category");
        }

        public async Task<ClassResult<CategoryDto>> DeleteCategory(int id)
        {
            return await _httpClientHelper.SendDeleteRequest<CategoryDto>($"/api/Category/{id}");
        }
    }
}
