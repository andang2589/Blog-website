using AutoMapper;
using BlogWebsite.Data.Models;
using BlogWebsite.DTO.Common;
using BlogWebsite.DTO.User;
using BlogWebsite.Service.Permission;
using BlogWebsite.Utilities.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BlogWebsite.Service.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IPermissionService _permissionService;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config,
            IMapper mapper,
            IPermissionService permissionService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _mapper = mapper;
            _permissionService = permissionService;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) 
            {
                return null; 
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password,request.RememberMe,true);
            if(!result.Succeeded)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name,request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials:  creds);
            await _permissionService.CacheUserPermissionAsync(user.Id.ToString());
            return  new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// JWT SIGN IN
        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );
            await _permissionService.CacheUserPermissionAsync(user.Id.ToString());

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task SignInWithJwtToken(HttpContext httpContext, AppUser user)
        {
            var token = await GenerateJwtTokenAsync(user);

            // Lưu token vào cookies
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:DurationInMinutes"])),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            httpContext.Response.Cookies.Append("jwt", token, cookieOptions);
        }


        ///

        public async Task<ApiResult<UserDTO>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserDTO>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);
            userDto.Roles = roles;
            return new ApiSucceedResult<UserDTO>(userDto);


        }

        public async Task<PagedResult<UserDTO>> GetUserAndPaging(GetUserAndPagingRequest request)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x=>x.UserName.Contains(request.Keyword)||x.PhoneNumber.Contains(request.Keyword));
            } 

            //Paging

            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserDTO()
                {
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    Id = x.Id,
                    LastName = x.LastName,
                    Dob = x.DoB
                }).ToListAsync();

            //Select

            var pagedResult = new PagedResult<UserDTO>()
            {
                TotalRecords = totalRow,
                Items = data
            };
            return pagedResult;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            
            var user = _mapper.Map<AppUser>(request);


            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            
            if(await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<bool>("Email existed");
            }


            //var foundUser = await _userManager.FindByIdAsync(id.ToString());
            var user = _mapper.Map<AppUser>(request);
            user.SecurityStamp = Guid.NewGuid().ToString();

            //var user = await _userManager.FindByIdAsync(id.ToString());
            //user.DoB = request.DoB;
            //user.Email = request.Email;
            //user.FirstName = request.FirstName;
            //user.LastName = request.LastName;
            //user.PhoneNumber = request.PhoneNumber;
            //var userfind =await _userManager.FindByIdAsync(id.ToString());
            //var user = _mapper.Map(request,userfind);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSucceedResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");


        }

        public async Task<ClassResult<UserDTO>> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return ClassResult<UserDTO>.FailureResult("User not exist!!");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return ClassResult<UserDTO>.FailureResult("Không thể xóa người dùng!");
            }
            return ClassResult<UserDTO>.SuccessResult();

        }


        public async Task<bool> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}