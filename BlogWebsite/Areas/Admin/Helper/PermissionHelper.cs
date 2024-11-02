using Azure.Core;
using BlogWebsite.DTO.Permission;
using BlogWebsite.DTO.User;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace BlogWebsite.Areas.Admin.Helper
{
    public static class PermissionHelper
    {
        private static IDistributedCache _cache;
        private static IHttpContextAccessor _httpContextAccessor;
        public static void Configure(IDistributedCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }

        public static async Task<bool>  HasPermission(this ClaimsPrincipal user, string controller, string action)/////////????
        {
            try
            {
                var userInfoJson = _httpContextAccessor.HttpContext.Session.GetString("UserInfo");
                if (userInfoJson == null)
                {
                    return false;
                }
                var userInfo = JsonConvert.DeserializeObject<UserDTO>(userInfoJson);
                var userId = userInfo.Id;


                //var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"]; // Lấy token từ cookie
                //if (string.IsNullOrEmpty(token))
                //{
                //    return false; // Hoặc xử lý theo cách bạn muốn
                //}


                // Giải mã token và lấy userId (giống như cách đã hướng dẫn trước đó)
                //var handler = new JwtSecurityTokenHandler();
                //var jwtToken = handler.ReadJwtToken(token);
                //var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                //var u = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                //var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                //var token = Request.Cookies["jwt"];

                //var uid = userIdClaim?.Value;

                //var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                //if (string.IsNullOrEmpty(userId) /*|| _cache == null*/)
                //{
                //    return false;
                //}

                var cachedPermissionJson = await _cache.GetStringAsync(userId.ToString());
                List<CachedPermissionDto> cachedPermissions = null;

                if (cachedPermissionJson == null)
                {
                    // Nếu không có trong cache, lấy từ cơ sở dữ liệu và lưu vào cache
                    //await CacheUserPermissionAsync(userId);
                    //cachedPermissionJson = await _cache.GetStringAsync(userId);
                    return false;
                }

                if (!string.IsNullOrEmpty(cachedPermissionJson))
                {
                    // Deserialize lại đối tượng từ JSON
                    cachedPermissions = JsonConvert.DeserializeObject<List<CachedPermissionDto>>(cachedPermissionJson);
                }

                if (cachedPermissions == null)
                {
                    return false;
                }

                var result = cachedPermissions.Any(p => p.Controller == controller && p.Action == action);
                return result;
            }
            catch ( Exception ex )
            {
                throw new Exception("Permission Hellper Error", ex);
            }
            
        }
    }
}
