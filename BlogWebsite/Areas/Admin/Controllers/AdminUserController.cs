using AutoMapper;
using BlogWebsite.Areas.Admin.CallApiClient;
using BlogWebsite.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Areas.Admin.Controllers
{
    
    public class AdminUserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IMapper _mapper;
        public AdminUserController(IUserApiClient userApiClient, IMapper mapper) 
        {
            _mapper = mapper;
            _userApiClient = userApiClient;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var sessions = HttpContext.Session.GetString("Token");
            var request = new GetUserAndPagingRequest()
            {
                BearerToken = sessions,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUserAndPaging(request);
            return View(data);
        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApiClient.RegisterUser(request);
            if(result)
                return RedirectToAction("Index","AdminUser");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if(result.IsSucceed)
            {

                //var user = result.ResultObj;
                //var userUpdateRequest = new UserUpdateRequest()
                //{
                //    DoB = user.Dob,
                //    Email = user.Email,
                //    FirstName = user.FirstName,
                //    LastName = user.LastName,
                //    PhoneNumber = user.PhoneNumber,
                //    Id = user.Id
                //};
                var userUpdateRequest = _mapper.Map<UserUpdateRequest>(result.ResultObj);
                return View(userUpdateRequest);

            }

            return StatusCode(404);
        }


        [HttpPost]
        public async Task<IActionResult> Edit (UserUpdateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var result = await _userApiClient.UpdateUser(request.Id,request);
            if(result.IsSucceed)
            {
                TempData["result"] = "Cập nhật người dùng thành công";
                return RedirectToAction("Index","AdminUser");

            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var result = await _userApiClient.IsEmailInUse(email);
            if (result)
            {
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult($"Email {email} is already in use.");
            }
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            if(userId == null)
            {
                return BadRequest();
            }
            var user = await _userApiClient.DeleteUser(userId);
            return NoContent();
        }
    }
}
