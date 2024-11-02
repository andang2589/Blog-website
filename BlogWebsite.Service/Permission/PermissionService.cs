using M = BlogWebsite.Data.Models;
using BlogWebsite.Service.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogWebsite.Data.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BlogWebsite.DTO.Role;
using BlogWebsite.DTO.Permission;
using BlogWebsite.DTO.Common;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BlogWebsite.Service.Permission
{
    public class PermissionService : IPermissionService
    {
        private readonly ICommonService<AppUserRoles> _userRoleCmSv;
        private readonly ICommonService<RolePermission> _rolePermissionCmSv;
        private readonly ICommonService<M.Permission> _permissionCmSv;
        //private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(60);
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IDistributedCache _cache;
        

        public PermissionService(ICommonService<AppUserRoles> userRoleCmSv, ICommonService<RolePermission> rolePermissionCmSv,
            ICommonService<M.Permission> permissionCmSv, IDistributedCache cache, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) 
        {
            _userRoleCmSv = userRoleCmSv;
            _rolePermissionCmSv = rolePermissionCmSv;
            _permissionCmSv = permissionCmSv;
            _cache = cache;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        

        public async Task AssignPermissionToRoleAsync(string roleId, List<int> permissionIds)
        {
            try
            {
                //var role = await _rolePermissionCmSv.TableT().FindAsync(roleId);

                //if (role == null)
                //{
                //    throw new Exception($"Null");

                //}

                var roleIdGuid = Guid.Parse(roleId);

                // Kiểm tra nếu có các permissionIds
                if (permissionIds == null || permissionIds.Count == 0)
                {
                    throw new Exception($"Null");

                }

                var existingPermissions = await _rolePermissionCmSv.TableT().Where(rp => rp.RoleId == roleIdGuid).ToListAsync();

                _rolePermissionCmSv.TableT().RemoveRange(existingPermissions.Where(ep => !permissionIds.Contains(ep.PermissionId)));


                foreach(var permisionId in permissionIds)
                {
                    if(!existingPermissions.Any(ep=>ep.PermissionId== permisionId))
                    {
                        var newRolePermission = new RolePermission
                        {
                            RoleId = roleIdGuid,
                            PermissionId = permisionId
                        };
                        await _rolePermissionCmSv.AddAs(newRolePermission);
                    }
                }
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

            
        }

        public async Task<ClassResult<RolePermissionDto>> GetPermissionForRoleAsync(string roleId)
        {
            try
            {
                var roleIdGuid = Guid.Parse(roleId);

                var role = await _roleManager.FindByIdAsync(roleId);

                var allPermissions = await _permissionCmSv.TableT().ToListAsync();

                var rolePermissions = await _rolePermissionCmSv.TableT()
                    .Where(rp => rp.RoleId == roleIdGuid)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync();

                var model = new RolePermissionDto
                {
                    RoleId = roleId,
                    RoleName = role.Name,
                    Permissions = allPermissions.Select(p => new DTO.Permission.PermissionDto
                    {
                        PermissionId = p.PermissionId,
                        Name = p.Name,
                        Controller = p.Controller,
                        Action = p.Action,
                        IsSelected = rolePermissions.Contains(p.PermissionId)
                    }).ToList()
                };
                return ClassResult<RolePermissionDto>.SuccessResult(model);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            

            //var result = await _rolePermissionCmSv.TableT()
            //    .Where(rp => rp.RoleId == roleIdGuid).Select(rp => rp.Permission).ToListAsync();
            
        }

        //Luu lai quyen cua user khi dang nhap
        public async Task CacheUserPermissionAsync(string userId)
        {

            try
            {
                var userIdGuid = Guid.Parse(userId);
                var query = _userManager.Users;
                var permission = await _userRoleCmSv.TableT()

                    .Where(ur => ur.UserId == userIdGuid)
                    .SelectMany(ur => ur.Role.RolePermissions)
                    //.Select(rp => new { rp.Permission.Controller, rp.Permission.Action })
                    .Select(rp=> new CachedPermissionDto
                    {
                        Controller = rp.Permission.Controller,
                        Action = rp.Permission.Action
                    })
                    .ToListAsync();

                // Serialize permissions thành JSON trước khi lưu vào cache
                var permissionJson = JsonConvert.SerializeObject(permission);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheDuration
                };

                //Luu vao SQl Server Cache
                await _cache.SetStringAsync(userId,permissionJson, cacheOptions);

                //_cache.Set(userId, permission, _cacheDuration);
                //if(!_cache.TryGetValue(userId, out var permissions))
                //{
                //    throw new Exception();
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("Error caching user permission",ex);
            }
            

            //var permission = query.Where(u=>u.Id == userIdGuid).SelectMany(u=> u.UserRoles)

        }

        //Check Permission From Cache

        public async Task<bool> UserHasPermissionAsync(ClaimsPrincipal user, string controller, string action)
        {
            try
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (String.IsNullOrEmpty(userId))
                {
                    return false;
                }

                //Lay quyen tu cache
                var cachedPermissionJson = await _cache.GetStringAsync(userId);
                List<CachedPermissionDto> cachedPermissions = null;

                if (cachedPermissionJson == null)
                {
                    // Nếu không có trong cache, lấy từ cơ sở dữ liệu và lưu vào cache
                    await CacheUserPermissionAsync(userId);
                    cachedPermissionJson = await _cache.GetStringAsync(userId);
                }

                if(!string.IsNullOrEmpty(cachedPermissionJson))
                {
                    // Deserialize lại đối tượng từ JSON
                    cachedPermissions = JsonConvert.DeserializeObject<List<CachedPermissionDto>>(cachedPermissionJson);
                }

                if(cachedPermissions == null)
                {
                    return false;
                }

                //if (!_cache.TryGetValue(userId, out List<CachedPermissionDto> cachedPermissions))
                //{
                //    //Neu khong co trong cache lay tu trong co so du lieu luu vao cache
                //    await CacheUserPermissionAsync(userId);
                //    _cache.TryGetValue(userId, out cachedPermissions);
                //    if (cachedPermissions == null)
                //    {
                //        return false;
                //    }

                //    //Kiem tra neu user co quyen dang nhap action va controller
                //}

                

                var result = cachedPermissions.Any(p => p.Controller == controller && p.Action == action);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            

        }
    }

    
}
