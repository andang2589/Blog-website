using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogWebsite.DTO.Role;
using BlogWebsite.Service.Role;
using System.Reflection.Metadata.Ecma335;
using BlogWebsite.Service.Permission;
using Microsoft.AspNetCore.Authorization;
using BlogWebsite.Data.Models;

namespace BlogWebsite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        public RoleController(IRoleService roleService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var role = await _roleService.GetAll();
            return Ok(role);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _roleService.RoleAssign(id, request);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleService.CreateRole(roleDto);
            return Ok(result);
        }


        [Authorize]
        [HttpGet("ListRoles")]
        public async Task<IActionResult> ListRoles()
        {
            var roles = await _roleService.ListRoles();
            return Ok(roles);
        }


        [AllowAnonymous]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleService.GetRoleById(id);
            return Ok(role);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateRole(RoleDto roleDto)
        {
            var result = await _roleService.UpdateRole(roleDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRole(id);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }
        [HttpGet("GetUsersByRole/{id}")]
        public async Task<IActionResult> GetUsersByRole(string id)
        {
            var result = await _roleService.GetUsersByRole(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("EditUsersInRole/{roleId}")]
        public async Task<IActionResult> EditUsersInRole(List<GetUsersByRoleDto> users, string roleId)
        {
            var result = await _roleService.EditUsersInRole(users, roleId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("GetPermissionForRole/{roleId}")]
        public async Task<IActionResult> GetPermissionForRole(string roleId)
        {
            var result = await _permissionService.GetPermissionForRoleAsync(roleId);
            return Ok(result);
        }

        [HttpPost("AssignPermissionToRole/{roleId}")]
        public async Task<IActionResult> AssignPermissionToRole(string roleId, List<int> permissionIds)
        {
             await _permissionService.AssignPermissionToRoleAsync(roleId, permissionIds);
            return Ok();
        }


        

    }
}
