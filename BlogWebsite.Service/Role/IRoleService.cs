using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Service.Role
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAll();
        Task<ApiResult<bool>> RoleAssign(Guid Id,RoleAssignRequest request);
        Task<ClassResult<RoleDto>> CreateRole(RoleDto roleDto);
        Task<ClassResult<List<RoleDto>>> ListRoles();
        Task<ClassResult<RoleDto>> GetRoleById(string id);
        Task<ClassResult<RoleDto>> UpdateRole(RoleDto roleDto);
        Task<ClassResult<RoleDto>> DeleteRole(string id);
        Task<ClassResult<List<GetUsersByRoleDto>>> GetUsersByRole(string id);
        Task<ClassResult<GetUsersByRoleDto>> EditUsersInRole(List<GetUsersByRoleDto> list, string roleId);
    }
}
