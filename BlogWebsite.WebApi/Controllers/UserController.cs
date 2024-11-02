using Microsoft.AspNetCore.Http;
using BlogWebsite.Data.Models;
using BlogWebsite.Service.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogWebsite.DTO.User;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace BlogWebsite.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");

            // Simulate a variable and log it at Trace level
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Calculation value is {calculation}", calculation);

            _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purposes.");

            // Log some debug information
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Debug information: {@debugInfo}", debugInfo);

            _logger.LogInformation("LogInformation: The LogAllLevels endpoint was reached successfully.");

            // Simulate a condition that might cause an issue
            bool resourceLimitApproaching = true;
            if (resourceLimitApproaching)
            {
                _logger.LogWarning("LogWarning: Resource usage is nearing the limit. Action may be required soon.");
            }

            try
            {
                // Simulate an error scenario
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            // Log a critical error scenario
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }

            return Ok("All logging levels demonstrated in this endpoint.");
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login attempt.");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await _userService.GenerateJwtTokenAsync(user);

                // Trả về token trong response
                var userInfo = _mapper.Map<UserDTO>(user);
                return Ok(new { Token = token, User = userInfo });
            }

            return Unauthorized("Invalid login attempt.");
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userService.Authenticate(request);
            if (resultToken == null)
            {
                return BadRequest("Username or Password is incorrect.");
            }

            return Ok(resultToken);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register is unsuccessful");
            }
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserAndPagingRequest request)
        {
            var user = await _userService.GetUserAndPaging(request);
            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Update(id,request);
            if (!result.IsSucceed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpDelete("Delete/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if(userId == null)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.DeleteUser(userId);
            return Ok();

        }





        [AcceptVerbs("Get", "Post","IsEmailInUse/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            bool check = await _userService.IsEmailInUse(email);
            if (check == true)
            {
                return new JsonResult(true);
            }
            else
            {
                return new JsonResult($"Email {email} is already in use.");
            }
        }
    }
}
