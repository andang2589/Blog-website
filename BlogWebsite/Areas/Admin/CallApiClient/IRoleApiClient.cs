using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.Role;

namespace BlogWebsite.Areas.Admin.CallApiClient
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleDto>>> GetAll();
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ClassResult<RoleDto>> CreateRole(RoleDto roleDto);
        Task<ClassResult<List<RoleDto>>> ListRoles();
        Task<ClassResult<RoleDto>> GetRoleById(string id);
        Task<ClassResult<RoleDto>> UpdateRole(RoleDto roleDto);
        Task<ClassResult<RoleDto>> DeleteRole(string id);
        Task<ClassResult<List<GetUsersByRoleDto>>> GetUsersByRole(string id);
        Task<ClassResult<GetUsersByRoleDto>> EditUsersInRole(List<GetUsersByRoleDto> list, string roleId);
        Task<ClassResult<RolePermissionDto>> GetPermissionForRole(string roleId);
        Task<object> AssignPermissionToRole(string roleId, List<int> permissionIds);
    }
}
