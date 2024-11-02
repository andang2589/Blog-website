using Azure.Core;
using BlogWebsite.Areas.Admin.Helper;
using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.Role;
using BlogWebsite.DTO.User;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using M = BlogWebsite.Data.Models;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public class RoleApiClient : IRoleApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClientHelper _httpClientHelper;

        public RoleApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, HttpClientHelper httpClientHelper) 
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _httpClientHelper = httpClientHelper;
        }
        public async Task<ApiResult<List<RoleDto>>> GetAll()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await client.GetAsync($"/api/role/");
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                List<RoleDto> myDeserializeObjList = (List<RoleDto>)JsonConvert.DeserializeObject(body, typeof(List<RoleDto>));
                return new ApiSucceedResult<List<RoleDto>>(myDeserializeObjList);
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleDto>>>(body);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7297");
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/role/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSucceedResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ClassResult<RoleDto>> CreateRole(RoleDto roleDto)
        {
            return await _httpClientHelper.SendPostRequest<RoleDto>($"/api/role/", roleDto);
        }

        public async Task<ClassResult<List<RoleDto>>> ListRoles()
        {
            return await _httpClientHelper.SendGetRequest<List<RoleDto>>($"/api/role/ListRoles");
        }

        public async Task<ClassResult<RoleDto>> GetRoleById(string id)
        {
            return await _httpClientHelper.SendGetRequest<RoleDto>($"/api/role/GetById/{id}");
        }

        public async Task<ClassResult<RoleDto>> UpdateRole(RoleDto roleDto)
        {
            return await _httpClientHelper.SendPutRequest<RoleDto>($"/api/role/", roleDto);
        }

        public async Task<ClassResult<RoleDto>> DeleteRole(string id)
        {
            return await _httpClientHelper.SendDeleteRequest<RoleDto>($"/api/role/{id}");
        }


        public async Task<ClassResult<List<GetUsersByRoleDto>>> GetUsersByRole(string id)
        {
            return await _httpClientHelper.SendGetRequest<List<GetUsersByRoleDto>>($"/api/role/GetUsersByRole/{id}");
        }

        public async Task<ClassResult<GetUsersByRoleDto>> EditUsersInRole(List<GetUsersByRoleDto> list, string roleId )
        {
            return await _httpClientHelper.SendPostRequest<GetUsersByRoleDto>($"/api/role/EditUsersInRole/{roleId}", list);
        }

        public async Task<ClassResult<RolePermissionDto>> GetPermissionForRole(string roleId)
        {
            return await _httpClientHelper.SendGetRequest<RolePermissionDto>($"/api/role/GetPermissionForRole/{roleId}");  
        }

        public async Task<object> AssignPermissionToRole(string roleId, List<int> permissionIds)
        {
            return await _httpClientHelper.SendPostRequest<object>($"/api/role/AssignPermissionToRole/{roleId}", permissionIds); 
        }
    }
}
