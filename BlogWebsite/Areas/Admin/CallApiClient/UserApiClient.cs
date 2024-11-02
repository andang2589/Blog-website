using BlogWebsite.Areas.Admin.Helper;
using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClientHelper _httpClientHelper;
        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, HttpClientHelper httpClientHelper) 
        { 
            
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
            _httpClientHelper = httpClientHelper;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var json =  JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json,Encoding.UTF8,"application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var response = await client.PostAsync("https://localhost:7297/api/User/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<PagedResult<UserDTO>> GetUserAndPaging(GetUserAndPagingRequest request)
        {
            

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",request.BearerToken);
            var response = await client.GetAsync($"/api/user/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<UserDTO>>(body);
            return users;
        }

        public async Task<bool> RegisterUser(RegisterRequest registerRequest)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");

            var json = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await client.PostAsync($"/api/user/Register", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id,UserUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");

            var session = _contextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await client.PutAsync($"/api/user/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSucceedResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<UserDTO>> GetById(Guid id)
        {
            var session = _contextAccessor.HttpContext.Session.GetString("Token");
             var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/user/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if(response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSucceedResult<UserDTO>>(body);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<UserDTO>>(body);
        }

        public async Task<bool> IsEmailInUse(string email)
        {
            var session = _contextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/user/IsEmailInUse/{email}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return true;
        }

        public async Task<ClassResult<UserDTO>> DeleteUser(string userId)
        {
            return await _httpClientHelper.SendDeleteRequest<UserDTO>($"/api/user/Delete/{userId}");   
        }
    }
}