using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.Areas.Admin.Helper;
using BlogWebsite.DTO.Role;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Areas.Admin.Controllers
{
    
    public class AdminRoleController : Controller
    {

        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;

        public AdminRoleController(IUserApiClient userApiClient, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _roleApiClient = roleApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.RoleAssign(request.Id, request);

            if (result.IsSucceed)
            {
                TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("RoleAssign","AdminRole");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        public async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }
            await _roleApiClient.CreateRole(roleDto);
            return RedirectToAction("ListRoles","AdminRole");
        }

        public async Task<IActionResult> ListRoles()
        {
            var list = await _roleApiClient.ListRoles();

            var authorizationCheck = AuthorizationHelper.HandleAuthorization(list, this);
            if (authorizationCheck != null)
            {
                return authorizationCheck;
            }
            return View(list.Data);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleApiClient.GetRoleById(id);
            
            return View(role.Data);

        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleDto roleDto)
        {
            if(!ModelState.IsValid)
            {
                return View(roleDto);
            }
            var result = await _roleApiClient.UpdateRole(roleDto);
            return RedirectToAction("ListRoles","AdminRole");
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null)
            {
                return View("Error: Id is null");
            }
            await _roleApiClient.DeleteRole(id);
            return RedirectToAction("ListRoles", "AdminRole");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string id)
        {
            ViewBag.roleId = id;
            if(id == null)
            {
                return BadRequest();
            }
            var result = await _roleApiClient.GetUsersByRole(id);
            return View(result.Data);
        }

        public async Task<IActionResult> EditUsersInRole(List<GetUsersByRoleDto> users, string roleId)
        {
            if(!ModelState.IsValid || users == null || roleId == null)
            {
                return View(ModelState);
            }

            await _roleApiClient.EditUsersInRole(users, roleId);
            return RedirectToAction("EditRole", new {id = roleId});
        }


        public async Task<IActionResult> GetPermissionForRole(string roleId)
        {
            ViewBag.roleId = roleId;
            if(roleId == null)
            {
                return BadRequest(ModelState);
            }
            var result = await _roleApiClient.GetPermissionForRole(roleId);
            return View(result.Data);

        }

        public async Task<IActionResult> AssignPermissionToRole(string roleId, List<int> permissionIds)
        {
            if(roleId == null || !ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            await _roleApiClient.AssignPermissionToRole(roleId, permissionIds);
            return RedirectToAction("GetPermissionForRole", new {roleId = roleId});
        }
    }
}
