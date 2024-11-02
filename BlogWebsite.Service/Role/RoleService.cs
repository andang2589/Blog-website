using AutoMapper;
using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.Service.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IMapper mapper)
        {

            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAll()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;

        }

        public async Task<ApiResult<bool>> RoleAssign(Guid Id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại!");
            }
            
            var removedRoles = request.Roles.Where(x=>x.Selected==false).Select(x=>x.Name).ToList();
            foreach(var roleName in removedRoles)
            {
                if(await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }

            }

            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addRoles = request.Roles.Where(x=>x.Selected).Select(x=>x.Name).ToList();

            foreach(var roleName in addRoles)
            {
                if(await _userManager.IsInRoleAsync(user,roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSucceedResult<bool>();

        }


        public async Task<ClassResult<RoleDto>> CreateRole(RoleDto roleDto)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleDto?.Name);
            if(roleExist)
            {
                return ClassResult<RoleDto>.FailureResult("Role already exists");
            }
            else
            {
                var map = _mapper.Map<AppRole>(roleDto);                

                var result = await _roleManager.CreateAsync(map);
                if (result.Succeeded)
                {
                    return ClassResult<RoleDto>.SuccessResult(roleDto);
                }
                foreach(IdentityError error in result.Errors)
                {
                    return ClassResult<RoleDto>.FailureResult(error.Description);
                }

            }
            return ClassResult<RoleDto>.SuccessResult(roleDto);

        }

        public async Task<ClassResult<List<RoleDto>>> ListRoles()
        {
            try
            {
                var list = await _roleManager.Roles.ToListAsync();
                var map = _mapper.Map<List<RoleDto>>(list);
                return ClassResult<List<RoleDto>>.SuccessResult(map);

            }
            catch(Exception ex)
            {
                return ClassResult<List<RoleDto>>.FailureResult(ex.ToString());
            }

            
        }

        public async Task<ClassResult<RoleDto>> GetRoleById(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if(role == null)
                {
                    return ClassResult<RoleDto>.FailureResult("Role not exist");
                }
                var map = _mapper.Map<RoleDto>(role);
                foreach(var user in _userManager.Users.ToList())
                {
                    if(await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        
                        map.Users.Add(user.UserName);
                    }
                }
                return ClassResult<RoleDto>.SuccessResult(map);
            }
            catch (Exception ex)
            {
                return ClassResult<RoleDto>.FailureResult(ex.ToString());
            }
        }

        public async Task<ClassResult<RoleDto>> UpdateRole(RoleDto roleDto)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleDto.Id.ToString());
                if(role == null)
                {
                    return ClassResult<RoleDto>.FailureResult("Role is null");
                }
                var map = _mapper.Map(roleDto, role);
                
                var result = await _roleManager.UpdateAsync(role);

                foreach(var user in _userManager.Users.ToList())
                {
                    if(await _userManager.IsInRoleAsync(user,role.Name))
                    {
                        
                    }
                }
                if(result.Succeeded)
                {
                    return ClassResult<RoleDto>.SuccessResult();
                }
                return ClassResult<RoleDto>.FailureResult();
            }
            catch (Exception ex)
            {
                return ClassResult<RoleDto>.FailureResult(ex.ToString());
            }
        }


        public async Task<ClassResult<RoleDto>> DeleteRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if(role == null)
                {
                    return ClassResult<RoleDto>.FailureResult("Id not found");
                }
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return ClassResult<RoleDto>.SuccessResult();
                }
                return ClassResult<RoleDto>.FailureResult(result.Errors.ToString());

            }
            catch(Exception ex)
            {
                return ClassResult<RoleDto>.FailureResult(ex.ToString());
            }
        }

        public async Task<ClassResult<List<GetUsersByRoleDto>>> GetUsersByRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                //var map = _mapper.Map<GetUsersByRoleDto>(role);
                var list = new List<GetUsersByRoleDto>();
                if (role == null)
                {
                    return ClassResult<List<GetUsersByRoleDto>>.FailureResult("Role not found");
                }
                foreach(var user in _userManager.Users.ToList())
                {
                    var map = _mapper.Map<GetUsersByRoleDto>(user);
                    if(await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        map.IsSelected = false;
                    }
                    list.Add(map);
                }
                return ClassResult<List<GetUsersByRoleDto>>.SuccessResult(list);
            }
            catch (Exception ex)
            {
                return ClassResult<List<GetUsersByRoleDto>>.FailureResult(ex.ToString());
            }
        }


        public async Task<ClassResult<GetUsersByRoleDto>> EditUsersInRole(List<GetUsersByRoleDto> list, string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                {
                    return ClassResult<GetUsersByRoleDto>.FailureResult("Role not found");
                }
                for(int i=0; i<list.Count; i++)
                {
                    IdentityResult? result;
                    var user = await _userManager.FindByIdAsync(list[i].Id);
                    if (list[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        //If IsSelected is true and User is not already in this role, then add the user
                         result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!list[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        //If IsSelected is false and User is already in this role, then remove the user
                         result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        //Don't do anything simply continue the loop
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if (i < (list.Count - 1))
                            continue;
                        else
                            return ClassResult<GetUsersByRoleDto>.SuccessResult();
                    }
                }
                return ClassResult<GetUsersByRoleDto>.SuccessResult();
            }
            catch (Exception ex)
            {
                return ClassResult<GetUsersByRoleDto>.FailureResult(ex.ToString());
            }
        }
    }
}
