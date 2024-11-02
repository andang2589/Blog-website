using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using M = BlogWebsite.Data.Models;

namespace BlogWebsite.Service.Permission
{
    public interface IPermissionService
    {
        //Task<bool> UserHasPermissionAsync(ClaimsPrincipal user, string action);
        Task<bool> UserHasPermissionAsync(ClaimsPrincipal user, string controller, string action);

        Task<ClassResult<RolePermissionDto>> GetPermissionForRoleAsync(string roleId);
        Task CacheUserPermissionAsync(string userId);
        Task AssignPermissionToRoleAsync(string roleId, List<int> permissionIds);
    }
}
